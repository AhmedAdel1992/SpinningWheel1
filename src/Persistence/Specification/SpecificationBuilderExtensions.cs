using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text.Json;
using Ardalis.SmartEnum;
using Ardalis.Specification;
using Common.Extensions;
using Domain.Extensions.Models;

namespace Persistence.Specification;

// See https://github.com/ardalis/Specification/issues/53
public static class SpecificationBuilderExtensions
{
    public static ISpecificationBuilder<T> SearchBy<T>(this ISpecificationBuilder<T> query, BaseFilter filter) =>
        query
            .SearchByKeyword(filter.Keyword)
            .AdvancedSearch(filter.AdvancedSearch)
            .AdvancedFilter(filter.AdvancedFilter);

    public static ISpecificationBuilder<T> PaginateBy<T>(this ISpecificationBuilder<T> query, PaginationFilter filter)
    {
        if (filter.PageNumber <= 0)
        {
            filter.PageNumber = 1;
        }

        if (filter.PageSize <= 0)
        {
            filter.PageSize = 10;
        }

        if (filter.PageNumber > 1)
        {
            query = query.Skip((filter.PageNumber - 1) * filter.PageSize);
        }

        return query
            .Take(filter.PageSize)
            .OrderBy(filter.OrderBy);
    }

    public static IOrderedSpecificationBuilder<T> SearchByKeyword<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        string? keyword) =>
        specificationBuilder.AdvancedSearch(new Search { Keyword = keyword });

    public static IOrderedSpecificationBuilder<T> AdvancedSearch<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Search? search)
    {
        if (!string.IsNullOrEmpty(search?.Keyword))
        {
            if (search.Fields?.Any() is true)
            {
                // search seleted fields (can contain deeper nested fields)
                foreach (string field in search.Fields)
                {
                    var paramExpr = Expression.Parameter(typeof(T));
                    MemberExpression propertyExpr = GetPropertyExpression(field, paramExpr);

                    specificationBuilder.AddSearchPropertyByKeyword(propertyExpr, paramExpr, search.Keyword);
                }
            }
            else
            {
                // search all fields (only first level)
                foreach (var property in typeof(T).GetProperties()
                             .Where(prop => (Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType) is
                                            { } propertyType
                                            && !propertyType.IsEnum
                                            && Type.GetTypeCode(propertyType) != TypeCode.Object))
                {
                    var paramExpr = Expression.Parameter(typeof(T));
                    var propertyExpr = Expression.Property(paramExpr, property);

                    specificationBuilder.AddSearchPropertyByKeyword(propertyExpr, paramExpr, search.Keyword);
                }
            }
        }

        return new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);
    }

    private static void AddSearchPropertyByKeyword<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Expression propertyExpr,
        ParameterExpression paramExpr,
        string keyword,
        string operatorSearch = FilterOperator.CONTAINS)
    {
        if (propertyExpr is not MemberExpression memberExpr || memberExpr.Member is not PropertyInfo property)
        {
            throw new ArgumentException("propertyExpr must be a property expression.", nameof(propertyExpr));
        }

        string searchTerm = operatorSearch switch
        {
            FilterOperator.STARTSWITH => $"{keyword}%",
            FilterOperator.ENDSWITH => $"%{keyword}",
            FilterOperator.CONTAINS => $"%{keyword}%",
            _ => throw new ArgumentException("operatorSearch is not valid.", nameof(operatorSearch))
        };

        // Generate lambda [ x => x.Property ] for string properties
        // or [ x => ((object)x.Property) == null ? null : x.Property.ToString() ] for other properties
        Expression selectorExpr =
            property.PropertyType == typeof(string)
                ? propertyExpr
                : Expression.Condition(
                    Expression.Equal(Expression.Convert(propertyExpr, typeof(object)),
                        Expression.Constant(null, typeof(object))),
                    Expression.Constant(null, typeof(string)),
                    Expression.Call(propertyExpr, "ToString", null, null));

        var selector = Expression.Lambda<Func<T, string>>(selectorExpr, paramExpr);

        ((List<SearchExpressionInfo<T>>)specificationBuilder.Specification.SearchCriterias)
            .Add(new SearchExpressionInfo<T>(selector, searchTerm, 1));
    }

    public static IOrderedSpecificationBuilder<T> AdvancedFilter<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        Filter? filter)
    {
        if (filter is not null)
        {
            var parameter = Expression.Parameter(typeof(T));

            Expression binaryExpressionFilter;

            if (!string.IsNullOrEmpty(filter.Logic))
            {
                if (filter.Filters is null)
                {
                    // throw new MissingArgumentException("The Filters attribute is required when declaring a logic");
                }

                binaryExpressionFilter = CreateFilterExpression(filter.Logic, filter.Filters, parameter);
            }
            else
            {
                var filterValid = GetValidFilter(filter);
                binaryExpressionFilter = CreateFilterExpression(filterValid.Field!, filterValid.Operator!,
                    filterValid.Value, parameter, filter);
            }

            if (binaryExpressionFilter is not null)
                ((List<WhereExpressionInfo<T>>)specificationBuilder.Specification.WhereExpressions)
                    .Add(new WhereExpressionInfo<T>(Expression.Lambda<Func<T, bool>>(binaryExpressionFilter,
                        parameter)));
        }

        return new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);
    }

    private static Expression CreateFilterExpression(
        string logic,
        IEnumerable<Filter> filters,
        ParameterExpression parameter)
    {
        try
        {
            Expression filterExpression = default!;

            foreach (var filter in filters)
            {
                Expression bExpressionFilter;

                if (!string.IsNullOrEmpty(filter.Logic) && filter.Filters != null)
                {
                    if (filter.Filters is null)
                    {
                        // throw new MissingArgumentException("The Filters attribute is required when declaring a logic");
                    }

                    bExpressionFilter = CreateFilterExpression(filter.Logic, filter.Filters, parameter);
                }
                else
                {
                    var filterValid = GetValidFilter(filter);
                    bExpressionFilter = CreateFilterExpression(filterValid.Field!, filterValid.Operator!,
                        filterValid.Value,
                        parameter, null);
                }

                filterExpression = filterExpression is null
                    ? bExpressionFilter
                    : CombineFilter(logic, filterExpression, bExpressionFilter);
            }

            return filterExpression;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static Expression CreateFilterExpression(string field,
        string filterOperator,
        object? value,
        ParameterExpression parameter, Filter? filter)
    {
        var propertyExpresion = GetPropertyExpression(field, parameter);
        if (propertyExpresion is not null)
        {
            var valueExpresion = GeValueExpression(field, value, propertyExpresion.Type, filter);
            return CreateFilterExpression(propertyExpresion, valueExpresion, filterOperator);
        }

        return null;
    }

    private static Expression CreateFilterExpression(
        MemberExpression memberExpression,
        ConstantExpression constantExpression,
        string filterOperator)
    {
        return filterOperator switch
        {
            FilterOperator.EQ => Expression.Equal(memberExpression, constantExpression),
            FilterOperator.NEQ => Expression.NotEqual(memberExpression, constantExpression),
            FilterOperator.LT => Expression.LessThan(memberExpression, constantExpression),
            FilterOperator.LTE => Expression.LessThanOrEqual(memberExpression, constantExpression),
            FilterOperator.GT => Expression.GreaterThan(memberExpression, constantExpression),
            FilterOperator.GTE => Expression.GreaterThanOrEqual(memberExpression, constantExpression),
            FilterOperator.CONTAINS => Expression.Call(memberExpression, "Contains", null, constantExpression),
            FilterOperator.STARTSWITH => Expression.Call(memberExpression, "StartsWith", null, constantExpression),
            FilterOperator.ENDSWITH => Expression.Call(memberExpression, "EndsWith", null, constantExpression),
            // _ => throw new UnsupportedOperator("Filter Operator is not valid."),
        };
    }

    private static Expression CombineFilter(
        string filterOperator,
        Expression bExpresionBase,
        Expression bExpresion)
    {
        return filterOperator switch
        {
            FilterLogic.AND => Expression.And(bExpresionBase, bExpresion),
            FilterLogic.OR => Expression.Or(bExpresionBase, bExpresion),
            FilterLogic.XOR => Expression.ExclusiveOr(bExpresionBase, bExpresion),
            _ => throw new ArgumentException("FilterLogic is not valid.", nameof(FilterLogic)),
        };
    }

    private static MemberExpression GetPropertyExpression(
        string propertyName,
        ParameterExpression parameter)
    {
        Expression propertyExpression = parameter;
        if (propertyName.IsNullOrEmpty())
            return null;
        foreach (string member in propertyName.Split('.'))
        {
            propertyExpression = Expression.PropertyOrField(propertyExpression, member);
        }

        return (MemberExpression)propertyExpression;
    }

    private static string GetStringFromJsonElement(object value)
        => ((JsonElement)value).GetString()!;


    private static MethodInfo? GetBaseFromValueMethod(Type objectType, Type valueType)
    {
        try
        {
            Type? baseType = objectType.BaseType;
            if (baseType == null)
                return null;
            for (; baseType != typeof(object); baseType = baseType.BaseType)
            {
                if (baseType is null) break;
                if (baseType.IsGenericType &&
                    baseType.GetGenericTypeDefinition() == typeof(Ardalis.SmartEnum.SmartEnum<,>))
                {
                    return baseType.GetMethod("FromValue", new Type[1]
                    {
                        valueType
                    });
                }
            }

            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private static MethodInfo? GetBaseFromNameMethod(Type objectType, Type valueType)
    {
        try
        {
            Type? baseType = objectType.BaseType;
            if (baseType == null)
                return null;
            for (; baseType != typeof(object); baseType = baseType.BaseType)
            {
                if (baseType is null) break;
                if (baseType.IsGenericType &&
                    baseType.GetGenericTypeDefinition() == typeof(Ardalis.SmartEnum.SmartEnum<,>))
                {
                    return baseType.GetMethod("FromName", new Type[2]
                    {
                        typeof(string),
                        typeof(bool)
                    });
                }
            }

            return null;
        }
        catch (Exception e)
        {
            return null;
        }
    }


    private static object? GetSmartEnumValue(Type objectType, Type valueType, object value)
    {
        try
        {
            MethodInfo baseFromValueMethod = GetBaseFromValueMethod(objectType, valueType);

            object? result = TryGetByValue(value, baseFromValueMethod);
            if (result is not null)
                return result;

            baseFromValueMethod = GetBaseFromNameMethod(objectType, valueType);
            if (baseFromValueMethod is not null)
            {
                return baseFromValueMethod.Invoke((object)null, new object[2]
                {
                    value, true
                });
            }

            return (object)null;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private static object? TryGetByValue(object value, MethodInfo baseFromValueMethod)
    {
        try
        {
            if (baseFromValueMethod is null) return null;
            if (int.TryParse(value.ToString(), out var intValue))
            {
                return baseFromValueMethod.Invoke((object)null, new object[1]
                {
                    intValue
                });
            }

            return baseFromValueMethod.Invoke((object)null, new object[1]
            {
                value
            });
        }
        catch (Exception e)
        {
            return null;
        }
    }

    private static Type GetValueType(Type objectType, Type mainType)
    {
        Type baseType = objectType.BaseType;
        if (baseType == (Type)null)
            return (Type)null;
        for (; baseType != typeof(object); baseType = baseType.BaseType)
        {
            if (baseType.IsGenericType && baseType.GetGenericTypeDefinition() == mainType)
                return baseType.GenericTypeArguments[1];
        }

        return (Type)null;
    }

    public static ConstantExpression GeValueExpression(string field, object? value, Type propertyType, Filter? filter)
    {
        if (value == null) return Expression.Constant(null, propertyType);

        if (propertyType.GetInterfaces().Contains(typeof(ISmartEnum)))
        {
            string? stringEnum = GetStringFromJsonElement(value);
            var valuetype = GetValueType(propertyType, typeof(SmartEnum<,>));
            var enumInstance = GetSmartEnumValue(propertyType, valuetype, stringEnum);
            return Expression.Constant(enumInstance, propertyType);
        }

        if (propertyType == typeof(DateTime))
        {
            string? stringDate = GetStringFromJsonElement(value);

            if (!DateTime.TryParse(stringDate, out DateTime valueparsed))
            {
                // throw new InvalidValueException($"Value {value} is not valid for {field}");
            }

            return Expression.Constant(valueparsed.Date, propertyType);
        }

        if (propertyType.IsEnum)
        {
            string? stringEnum = GetStringFromJsonElement(value);

            if (!Enum.TryParse(propertyType, stringEnum, true, out object? valueparsed))
            {
                // throw new InvalidValueException($"Value {value} is not valid for {field}");
            }

            return Expression.Constant(valueparsed, propertyType);
        }

        if (propertyType == typeof(Guid))
        {
            string? stringGuid = GetStringFromJsonElement(value);

            if (!Guid.TryParse(stringGuid, out Guid valueparsed))
            {
                // throw new InvalidValueException($"Value {value} is not valid for {field}");
            }

            return Expression.Constant(valueparsed, propertyType);
        }

        if (propertyType == typeof(string))
        {
            string? text = GetStringFromJsonElement(value);

            return Expression.Constant(text, propertyType);
        }

        return Expression.Constant(Convert.ChangeType(((JsonElement)value).GetRawText(), propertyType), propertyType);
    }

    private static Filter GetValidFilter(Filter filter)
    {
        if (string.IsNullOrEmpty(filter.Field))
        {
            // throw new MissingArgumentException("The field attribute is required when declaring a filter");
        }

        if (string.IsNullOrEmpty(filter.Operator))
        {
            // throw new MissingArgumentException("The Operator attribute is required when declaring a filter");
        }

        return filter;
    }

    public static IOrderedSpecificationBuilder<T> OrderBy<T>(
        this ISpecificationBuilder<T> specificationBuilder,
        string[]? orderByFields)
    {
        if (orderByFields is not null)
        {
            foreach (var field in ParseOrderBy(orderByFields))
            {
                var paramExpr = Expression.Parameter(typeof(T));

                Expression propertyExpr = paramExpr;
                foreach (string member in field.Key.Split('.'))
                {
                    propertyExpr = Expression.PropertyOrField(propertyExpr, member);
                }

                var keySelector = Expression.Lambda<Func<T, object?>>(
                    Expression.Convert(propertyExpr, typeof(object)),
                    paramExpr);

                ((List<OrderExpressionInfo<T>>)specificationBuilder.Specification.OrderExpressions)
                    .Add(new OrderExpressionInfo<T>(keySelector, field.Value));
            }
        }

        return new OrderedSpecificationBuilder<T>(specificationBuilder.Specification);
    }

    private static Dictionary<string, OrderTypeEnum> ParseOrderBy(string[] orderByFields) =>
        new(orderByFields.Select((orderByfield, index) =>
        {
            string[] fieldParts = orderByfield.Split(' ');
            string field = fieldParts[0];
            bool descending = fieldParts.Length > 1 &&
                              fieldParts[1].StartsWith("Desc", StringComparison.OrdinalIgnoreCase);
            var orderBy = index == 0
                ? descending
                    ? OrderTypeEnum.OrderByDescending
                    : OrderTypeEnum.OrderBy
                : descending
                    ? OrderTypeEnum.ThenByDescending
                    : OrderTypeEnum.ThenBy;

            return new KeyValuePair<string, OrderTypeEnum>(field, orderBy);
        }));
}
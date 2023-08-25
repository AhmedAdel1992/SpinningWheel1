using System.Runtime.CompilerServices;
using LanguageExt.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Common.Extensions;

public static class ResultExtension
{

    // public static T Value<T>(this Result<T> result) where T : class
    // {
    //     return result.Match<T>(r => r, f => null);
    // }   
    public static T Value<T>(this Result<T> result) 
    {
        return result.Match<T>(r => r, f => default);
    }
    
    public static Exception Error<T>(this Result<T> result)
    {
        return result.Match<Exception>(r => default, f => f);
    }
    
    public static IResult ToOk<T>(this Result<T> result)
    {
        return result.Match<IResult>(r =>
        {
            var result = r;
            return Results.Ok(r);
        }, f =>
        {
            if(f is FileNotFoundException)
                return Results.NotFound(f.Message);

            return Results.BadRequest(f.Message);
        });
    }
}
namespace Common.Extensions;

public static class DateTimeExtensions
{
    public static DateOnly ToDateOnly(this DateTime date)
    {
        return DateOnly.FromDateTime(date);
    }

    public static TimeOnly ToTimeOnly(this DateTime date)
    {
        return TimeOnly.FromDateTime(date);
    }
}
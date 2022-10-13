namespace WorkdayCalendarLibrary.Util;

public static class DateTimeExtension
{
    /// <summary>
    /// Creates and returns a new DateTime object with the hour and minute updated to the supplied values. Seconds will be set to 0.
    /// </summary>
    /// <param name="date">The date to use as a base.</param>
    /// <param name="hour">The hour to be set.</param>
    /// <param name="minute">The minute to be set.</param>
    /// <returns>A new updated DateTime object.</returns>
    public static DateTime SetTime(this DateTime date, int hour, int minute)
    {
        return new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
    }

    /// <summary>
    /// Creates and returns a new DateTime object with the same date and time but seconds set to 0.
    /// </summary>
    /// <param name="date">The date to use as a base.</param>
    /// <returns>A new updated DateTime object.</returns>
    public static DateTime SetSecondsToZero(this DateTime date)
    {
        return new DateTime(date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
    }
}

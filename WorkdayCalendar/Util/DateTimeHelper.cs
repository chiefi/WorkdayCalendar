namespace WorkdayCalendar.Util;

/// <summary>
/// Helper class for managing DateTime, DateOnly and TimeOnly and their formatting.
/// </summary>
internal static class DateTimeHelper
{
    /// <summary>
    /// The format to use when formatting DateTime.
    /// </summary>
    public const string DateTimeFormat = "dd-MM-yyyy HH:mm";
    /// <summary>
    /// The format to use when formatting Date.
    /// </summary>
    public const string DateFormat = "dd-MM-yyyy";
    /// <summary>
    /// The format to use when formatting RecurringDate.
    /// </summary>
    public const string RecurringDateFormat = "dd-MM";
    /// <summary>
    /// The format to use when formatting Time.
    /// </summary>
    public const string TimeFormat = "HH:mm";

    /// <summary>
    /// Validates if the input is a valid DateTime according to <see cref="DateTimeFormat"/>.
    /// </summary>
    /// <param name="input">The text to validate.</param>
    /// <returns>True if valid, else false.</returns>
    public static bool IsValidDateTime(string? input)
    {
        return DateTime.TryParseExact(input, DateTimeFormat, null, System.Globalization.DateTimeStyles.None, out _);
    }

    /// <summary>
    /// Parses the input to a DateTime according to <see cref="DateTimeFormat"/>.
    /// </summary>
    /// <param name="input">The text to parse.</param>
    /// <returns>A DateTime object.</returns>
    public static DateTime ParseDateTime(string input)
    {
        return DateTime.ParseExact(input, DateTimeFormat, null, System.Globalization.DateTimeStyles.None);
    }

    /// <summary>
    /// Validates if the input is a valid Time according to <see cref="TimeFormat"/>.
    /// </summary>
    /// <param name="input">The text to validate.</param>
    /// <returns>True if valid, else false.</returns>
    public static bool IsValidTime(string? input)
    {
        return TimeOnly.TryParseExact(input, TimeFormat, out _);
    }

    /// <summary>
    /// Parses the input to a Time according to <see cref="TimeFormat"/>.
    /// </summary>
    /// <param name="input">The text to parse.</param>
    /// <returns>A TimeOnly object.</returns>
    public static TimeOnly ParseTime(string input)
    {
        return TimeOnly.ParseExact(input, TimeFormat);
    }

    /// <summary>
    /// Validates if the input is a valid Date according to <see cref="DateFormat"/>.
    /// </summary>
    /// <param name="input">The text to validate.</param>
    /// <returns>True if valid, else false.</returns>
    public static bool IsValidDate(string? input)
    {
        return DateOnly.TryParseExact(input, DateFormat, out _);
    }

    /// <summary>
    /// Parses the input to a Date according to <see cref="DateFormat"/>.
    /// </summary>
    /// <param name="input">The text to parse.</param>
    /// <returns>A DateOnly object.</returns>
    public static DateOnly ParseDate(string input)
    {
        return DateOnly.ParseExact(input, DateFormat);
    }

    /// <summary>
    /// Validates if the input is a valid RecurringDate according to <see cref="RecurringDateFormat"/>.
    /// </summary>
    /// <param name="input">The text to validate.</param>
    /// <returns>True if valid, else false.</returns>
    public static bool IsValidRecurringDate(string? input)
    {
        return DateOnly.TryParseExact(input, RecurringDateFormat, out _);
    }

    /// <summary>
    /// Parses the input to a DateOnly according to <see cref="RecurringDateFormat"/>.
    /// </summary>
    /// <param name="input">The text to parse.</param>
    /// <returns>A DateOnly object.</returns>
    public static DateOnly ParseRecurringDate(string input)
    {
        return DateOnly.ParseExact(input, RecurringDateFormat);
    }
}

namespace WorkdayCalendarLibrary.Model;

/// <summary>
/// Represents a holiday, this means that the holiday is taking place on the set year, month and day.
/// </summary>
public class Holiday
{
    /// <summary>
    /// Gets the year component of the holiday represented by this instance.
    /// </summary>
    /// <returns>The year component, expressed as a value between 1 and 9999.</returns>
    public int Year { get; init; }

    /// <summary>
    /// Gets the month component of the holiday represented by this instance.
    /// </summary>
    /// <returns>The month component, expressed as a value between 1 and 12.</returns>
    public int Month { get; init; }

    /// <summary>
    /// Gets the day component of the holiday represented by this instance.
    /// </summary>
    /// <returns>The day component, expressed as a value between 1 and 31.</returns>
    public int Day { get; init; }

    /// <summary>
    /// Checks if the supplied date object is matching the holiday.
    /// </summary>
    /// <param name="date">The DateTime object to check.</param>
    /// <returns>Returns true if the date matches the holiday, else false.</returns>
    public bool IsDate(DateTime date)
    {
        return date.Day == Day && date.Month == Month && date.Year == Year;
    }

    /// <summary>
    /// Checks if the supplied holiday object is matching this holiday.
    /// </summary>
    /// <param name="date">The Holiday object to check.</param>
    /// <returns>Returns true if the this object matches the holiday, else false.</returns>
    public bool Equals(Holiday date)
    {
        return date.Day == Day & date.Month == Month && date.Year == Year;
    }
}

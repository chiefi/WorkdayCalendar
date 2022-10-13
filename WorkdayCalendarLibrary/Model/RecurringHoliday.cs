namespace WorkdayCalendarLibrary.Model;

/// <summary>
/// Represents a recurring holiday, this means that the holiday is taking place each year on the set month and day.
/// </summary>
public class RecurringHoliday
{
    /// <summary>
    /// Gets the month component of the recurring holiday represented by this instance.
    /// </summary>
    /// <returns>The month component, expressed as a value between 1 and 12.</returns>
    public int Month { get; init; }

    /// <summary>
    /// Gets the day component of the recurring holiday represented by this instance.
    /// </summary>
    /// <returns>The day component, expressed as a value between 1 and 31.</returns>
    public int Day { get; init; }

    /// <summary>
    /// Checks if the supplied date object is matching the recurring holiday.
    /// </summary>
    /// <param name="date">The DateTime object to check.</param>
    /// <returns>Returns true if the date matches the recurring holiday, else false.</returns>
    public bool IsDate(DateTime date)
    {
        return date.Day == Day && date.Month == Month;
    }

    /// <summary>
    /// Checks if the supplied recurring holiday object is matching this holiday.
    /// </summary>
    /// <param name="date">The RecurringHoliday object to check.</param>
    /// <returns>Returns true if the this object matches the recurring holiday, else false.</returns>
    public bool Equals(RecurringHoliday recurringHoliday)
    {
        return recurringHoliday.Day == Day && recurringHoliday.Month == Month;
    }
}

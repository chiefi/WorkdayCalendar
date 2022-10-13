using WorkdayCalendarLibrary.Util;

namespace WorkdayCalendarLibrary.Model;

/// <summary>
/// Calendar to represent working days by excluding registered holidays.
/// By default Saturdays and Sundays are considered holidays.
/// </summary>
public class Calendar
{
    private readonly List<Holiday> holidays = new();
    private readonly List<RecurringHoliday> recurringHolidays = new();

    /// <summary>
    /// Gets and sets the time for the start of the workday.
    /// Default value is 08:00.
    /// </summary>
    public TimeOnly StartOfWorkday { get; set; } = new(08, 00);

    /// <summary>
    /// Gets and sets the time for the end of the workday.
    /// Default value is 16:00.
    /// </summary>
    public TimeOnly EndOfWorkday { get; set; } = new(16, 00);

    /// <summary>
    /// Calculates the total time of the workday based on StartOfWorkday and EndOfWorkday.
    /// </summary>
    /// <returns>The double representation of the workday in minutes.</returns>
    /// <exception cref="InvalidOperationException">StartOfWorkday must be set earlier than EndOfWorkday.</exception>
    public double GetLengthOfWorkday()
    {
        if (StartOfWorkday > EndOfWorkday)
            throw new InvalidOperationException("StartOfWorkday must be set earlier than EndOfWorkday.");

        return (EndOfWorkday - StartOfWorkday).TotalMinutes;
    }

    /// <summary>
    /// Gets all holidays registered in the calendar ordered by Year/Month/Day.
    /// </summary>
    /// <returns>A enumerable list of holidays.</returns>
    public IEnumerable<Holiday> GetHolidays() => holidays
        .OrderBy(x => x.Year)
        .ThenBy(x => x.Month)
        .ThenBy(x => x.Day);

    /// <summary>
    /// Gets all recurring holidays registered in the calendar ordered by Month/Day.
    /// </summary>
    /// <returns>A enumerable list of recurring holidays.</returns>
    public IEnumerable<RecurringHoliday> GetRecurringHolidays() => recurringHolidays
        .OrderBy(x => x.Month)
        .ThenBy(x => x.Day);

    /// <summary>
    /// Registers a Holiday in the Calendar.
    /// </summary>
    /// <param name="holiday">The Holiday object to register.</param>
    /// <returns>True if the holiday was registered successfully and false if it could not be added due to already being registered.</returns>
    public bool RegisterHoliday(Holiday holiday)
    {
        if (holidays.Any(h => h.Equals(holiday)))
            return false;

        holidays.Add(holiday);

        return true;
    }

    /// <summary>
    /// Registers a Recurring Holiday in the Calendar.
    /// </summary>
    /// <param name="recurringHoliday">The Recurring Holiday object to register.</param>
    /// <returns>True if the holiday was registered successfully and false if it could not be added due to already being registered.</returns>
    public bool RegisterRecurringHoliday(RecurringHoliday recurringHoliday)
    {
        if (recurringHolidays.Any(h => h.Equals(recurringHoliday)))
            return false;

        recurringHolidays.Add(recurringHoliday);

        return true;
    }

    /// <summary>
    /// Removes the holiday from the calendar.
    /// </summary>
    /// <param name="holiday">The holiday to remove</param>
    /// <returns>True if successful, else false</returns>
    public bool RemoveHoliday(Holiday holiday)
    {
        return holidays.Remove(holiday);
    }

    /// <summary>
    /// Removes the recurring holiday from the calendar.
    /// </summary>
    /// <param name="recurringHoliday">The recurring holiday to remove</param>
    /// <returns>True if successful, else false</returns>
    public bool RemoveRecurringHoliday(RecurringHoliday recurringHoliday)
    {
        return recurringHolidays.Remove(recurringHoliday);
    }

    /// <summary>
    /// Validates if the supplied DateTime object is a workday or not according to registered holidays and recurring holidays in the calendar.
    /// Saturdays and Sundays are not considered workdays.
    /// </summary>
    /// <param name="date">DateTime object to validate.</param>
    /// <returns>True if the date is considered a workday and false if it is not.</returns>
    public bool IsWorkday(DateTime date)
    {
        if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday)
            return false;

        if (holidays.Any(h => h.IsDate(date)))
            return false;

        if (recurringHolidays.Any(h => h.IsDate(date)))
            return false;

        return true;
    }

    /// <summary>
    /// Finds and returns the first valid workday after the passed in DateTime object.
    /// </summary>
    /// <param name="date">The date we should start at</param>
    /// <returns>A DateTime object with the next working day occurring after the date object.</returns>
    public DateTime MoveToNextWorkday(DateTime date)
    {
        date = AdjustDay(date, Adjustment.Increment);
        date = date.SetTime(StartOfWorkday.Hour, StartOfWorkday.Minute);

        return date;
    }

    /// <summary>
    /// Finds and returns the first valid workday before the passed in DateTime object.
    /// </summary>
    /// <param name="date">The date we should start at</param>
    /// <returns>A DateTime object with the next working day occurring before the date object.</returns>
    public DateTime MoveToPreviousWorkday(DateTime date)
    {
        date = AdjustDay(date, Adjustment.Decrement);
        date = date.SetTime(EndOfWorkday.Hour, EndOfWorkday.Minute);

        return date;
    }

    private DateTime AdjustDay(DateTime date, Adjustment adjustment)
    {
        date = date.AddDays((int)adjustment);

        while (!IsWorkday(date))
            date = date.AddDays((int)adjustment);

        return date;
    }
}

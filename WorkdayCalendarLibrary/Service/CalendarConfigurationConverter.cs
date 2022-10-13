using WorkdayCalendarLibrary.Model;

namespace WorkdayCalendarLibrary.Service;

/// <summary>
/// Converter for converting between Calendar and CalendarConfiguration, and vice-versa.
/// </summary>
public interface ICalendarConfigurationConverter
{
    /// <summary>
    /// Converts a Calendar to a CalendarConfiguration.
    /// </summary>
    /// <param name="calendar">The calendar to convert.</param>
    /// <returns>A CalendarConfiguration object.</returns>
    CalendarConfiguration ToConfiguration(Calendar calendar);
    /// <summary>
    /// Converts a CalendarConfiguration to a Calendar.
    /// </summary>
    /// <param name="configuration">The calendar configuration to convert.</param>
    /// <returns>A Calendar object.</returns>
    Calendar ToCalendar(CalendarConfiguration configuration);
}

public class CalendarConfigurationConverter : ICalendarConfigurationConverter
{
    public CalendarConfiguration ToConfiguration(Calendar calendar)
    {
        var configuration = new CalendarConfiguration
        {
            Holidays = calendar.GetHolidays().ToList(),
            RecurringHolidays = calendar.GetRecurringHolidays().ToList(),
            StartOfWorkday = calendar.StartOfWorkday,
            EndOfWorkday = calendar.EndOfWorkday
        };

        return configuration;
    }

    public Calendar ToCalendar(CalendarConfiguration configuration)
    {
        var calendar = new Calendar
        {
            StartOfWorkday = configuration.StartOfWorkday,
            EndOfWorkday = configuration.EndOfWorkday
        };

        foreach (var holiday in configuration.Holidays)
            calendar.RegisterHoliday(holiday);

        foreach (var recurringHoliday in configuration.RecurringHolidays)
            calendar.RegisterRecurringHoliday(recurringHoliday);

        return calendar;
    }
}

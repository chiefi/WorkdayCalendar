using System.Text.Json;
using WorkdayCalendarLibrary.Model;

namespace WorkdayCalendarLibrary.Service;

/// <summary>
/// A repository for the calendar. Handles saving and getting a calendar from storage.
/// </summary>
public interface ICalendarRepository
{
    /// <summary>
    /// Returns the active calendar. If noone active, retrieves it from storage.
    /// </summary>
    /// <returns>A calendar object.</returns>
    Calendar Get();
    /// <summary>
    /// Saves the active calendar to storage.
    /// </summary>
    void Save();
    /// <summary>
    /// Resets the active calendar to a default calendar object.
    /// Default calendar is working time 08:00 to 16:00.
    /// Holiday is 27-05-2004.
    /// Recurring holiday is 17-05-*.
    /// </summary>
    void Reset();
}

public class CalendarRepository : ICalendarRepository
{
    private Calendar? calendar;
    private readonly IFileService fileService;
    private readonly ICalendarConfigurationConverter calendarConfigurationConverter;

    public CalendarRepository(IFileService fileService,
        ICalendarConfigurationConverter calendarConfigurationConverter)
    {
        this.fileService = fileService;
        this.calendarConfigurationConverter = calendarConfigurationConverter;
    }

    public Calendar Get()
    {
        if (calendar != null)
            return calendar;

        string jsonString;
        try
        {
            jsonString = fileService.ReadSettings();
        }
        catch (Exception)
        {
            Reset();
            return calendar!;
        }

        // TODO: Handle faulty json here!
        CalendarConfiguration config = JsonSerializer.Deserialize<CalendarConfiguration>(jsonString)!;

        calendar = calendarConfigurationConverter.ToCalendar(config);

        return calendar;
    }

    public void Save()
    {
        if (calendar == null)
            return;
                
        var config = calendarConfigurationConverter.ToConfiguration(calendar);

        var options = new JsonSerializerOptions { WriteIndented = true };
        string jsonString = JsonSerializer.Serialize(config, options);
        fileService.WriteSettings(jsonString);
    }

    public void Reset()
    {
        calendar = GetDefaultCalendar();
    }

    private Calendar GetDefaultCalendar()
    {
        var cal = new Calendar();

        cal.StartOfWorkday = new TimeOnly(8, 0);
        cal.EndOfWorkday = new TimeOnly(16, 0);
        cal.RegisterRecurringHoliday(new RecurringHoliday { Month = 5, Day = 17 });
        cal.RegisterHoliday(new Holiday { Year = 2004, Month = 5, Day = 27 });

        return cal;
    }
}
using System.Text.Json.Serialization;
using WorkdayCalendarLibrary.Util;

namespace WorkdayCalendarLibrary.Model;

/// <summary>
/// JSON DTO representation of Calendar <see cref="Calendar"/>
/// </summary>
public class CalendarConfiguration
{
    public IReadOnlyList<Holiday> Holidays { get; set; } = new List<Holiday>();
    public IReadOnlyList<RecurringHoliday> RecurringHolidays { get; set; } = new List<RecurringHoliday>();

    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly StartOfWorkday { get; set; }

    [JsonConverter(typeof(TimeOnlyJsonConverter))]
    public TimeOnly EndOfWorkday { get; set; }
}
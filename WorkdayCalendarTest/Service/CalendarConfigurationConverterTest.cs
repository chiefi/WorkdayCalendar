using WorkdayCalendarLibrary.Model;
using WorkdayCalendarLibrary.Service;

namespace WorkdayCalendarTest.Service;

[TestClass]
public class CalendarConfigurationConverterTest
{
    [TestMethod]
    public void Convert_To_Calendar()
    {
        // Arrange
        var configuration = new CalendarConfiguration
        {
            StartOfWorkday = new TimeOnly(9, 0),
            EndOfWorkday = new TimeOnly(22, 0),
            Holidays = new List<Holiday>
            {
                new Holiday{ Year = 2005, Month = 10, Day = 2 },
                new Holiday{ Year = 2006, Month = 3, Day = 1 }
            },
            RecurringHolidays = new List<RecurringHoliday>
            {
                new RecurringHoliday{ Month = 1, Day = 2 },
                new RecurringHoliday{ Month = 3, Day = 4 },
                new RecurringHoliday{ Month = 5, Day = 6 }
            }
        };

        var converter = new CalendarConfigurationConverter();

        // Act
        var result = converter.ToCalendar(configuration);

        // Assert
        Assert.AreEqual(new TimeOnly(9, 0), result.StartOfWorkday);
        Assert.AreEqual(new TimeOnly(22, 0), result.EndOfWorkday);
        Assert.AreEqual(2, result.GetHolidays().Count());
        Assert.AreEqual(3, result.GetRecurringHolidays().Count());

    }

    [TestMethod]
    public void Convert_To_CalendarConfiguration()
    {
        // Arrange
        var calendar = new Calendar();

        calendar.StartOfWorkday = new TimeOnly(4, 0);
        calendar.EndOfWorkday = new TimeOnly(6, 0);
        calendar.RegisterRecurringHoliday(new RecurringHoliday { Month = 3, Day = 4 });
        calendar.RegisterHoliday(new Holiday { Year = 2005, Month = 6, Day = 6 });

        var converter = new CalendarConfigurationConverter();

        // Act
        var result = converter.ToConfiguration(calendar);

        // Assert
        Assert.AreEqual(new TimeOnly(4, 0), result.StartOfWorkday);
        Assert.AreEqual(new TimeOnly(6, 0), result.EndOfWorkday);
        Assert.AreEqual(1, result.Holidays.Count);
        Assert.AreEqual(1, result.RecurringHolidays.Count);
    }
}

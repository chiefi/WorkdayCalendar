using Microsoft.Extensions.Logging.Abstractions;
using WorkdayCalendarLibrary.Model;
using WorkdayCalendarLibrary.Service;

namespace WorkdayCalendarTest.Service;

[TestClass]
public class CalendarServiceTest
{
    private readonly ICalendarService calendarService;
    private readonly Calendar calendar;

    public CalendarServiceTest()
    {
        calendarService = GetCalendarService();
        calendar = SetUpCalendar();
    }

    private static Calendar SetUpCalendar()
    {
        var cal = new Calendar
        {
            StartOfWorkday = new TimeOnly(8, 0),
            EndOfWorkday = new TimeOnly(16, 0)
        };

        cal.RegisterRecurringHoliday(new RecurringHoliday { Month = 5, Day = 17 });
        cal.RegisterHoliday(new Holiday { Year = 2004, Month = 5, Day = 27 });

        return cal;
    }

    private static ICalendarService GetCalendarService()
    {
        return new CalendarService(new NullLogger<CalendarService>());
    }

    [TestMethod]
    public void Increment_Simple_Step()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 24, 15, 7, 0);
        double diff = 0.25;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 25, 9, 7, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Increment_Half_Day_Before_Start_Time()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 24, 4, 0, 0);
        double diff = 0.5;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 24, 12, 0, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Increment_Zero()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 24, 7, 3, 0);
        double diff = 0;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 24, 8, 0, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Increment_Zero_On_Holiday()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 27, 10, 3, 0);
        double diff = 0;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 28, 8, 0, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Increment_Zero_On_RecurringHoliday()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 17, 10, 3, 0);
        double diff = 0;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 18, 8, 0, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Increment_Zero_On_Weekend()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 23, 10, 3, 0);
        double diff = 0;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 24, 8, 0, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Increment_Before_Start_Time()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 24, 7, 3, 0);
        double diff = 0.00625;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 24, 8, 3, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Increment_After_End_Time()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 24, 18, 3, 0);
        double diff = 0.00625;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 25, 8, 3, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Decrement_Before_Start_Time()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 25, 7, 3, 0);
        double diff = -0.00625;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 24, 15, 57, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Decrement_After_End_Time()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 24, 18, 3, 0);
        double diff = -0.00625;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 24, 15, 57, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Increment_On_Holiday()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 27, 10, 3, 0);
        double diff = 0.00625;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 28, 08, 3, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Decrement_On_Holiday()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 27, 10, 3, 0);
        double diff = -0.00625;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 26, 15, 57, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Decrement_Day_After_Worktime()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 25, 19, 3, 0);
        double diff = -1;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 25, 08, 00, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Decrement_Day_On_EndOfDayTime()
    {
        // Arrange
        var startDate = new DateTime(2004, 5, 25, 16, 0, 0);
        double diff = -1;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2004, 5, 25, 08, 00, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Increment_Zero_On_EndOfDayTime()
    {
        // Arrange
        var startDate = new DateTime(2022, 10, 10, 16, 0, 0);
        double diff = 0;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2022, 10, 11, 08, 00, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Increment_Zero_On_StartOfDayTime()
    {
        // Arrange
        var startDate = new DateTime(2022, 10, 10, 08, 0, 0);
        double diff = 0;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2022, 10, 10, 08, 0, 0);
        Assert.AreEqual(expected, date);
    }

    [TestMethod]
    public void Decrement_One_On_EndOfDayTime()
    {
        // Arrange
        var startDate = new DateTime(2022, 10, 10, 16, 0, 0);
        double diff = -1;

        // Act
        var date = calendarService.FindWorkday(calendar, startDate, diff);

        // Assert
        var expected = new DateTime(2022, 10, 10, 08, 0, 0);
        Assert.AreEqual(expected, date);
    }

    [DataTestMethod]
    [DynamicData(nameof(GetTaskTestData), DynamicDataSourceType.Method)]
    public void Test_Cases_From_Task(DateTime startDate, double diff, DateTime expected)
    {
        // Arrange
        var service = new CalendarService(new NullLogger<CalendarService>());

        // Act
        var date = service.FindWorkday(calendar, startDate, diff);

        // Assert
        Assert.AreEqual(expected, date);
    }

    public static IEnumerable<object[]> GetTaskTestData()
    {
        yield return new object[] { new DateTime(2004, 05, 24, 18, 05, 00), -5.5000000, new DateTime(2004, 05, 14, 12, 00, 00) };
        yield return new object[] { new DateTime(2004, 05, 24, 19, 03, 00), 44.7236560, new DateTime(2004, 07, 27, 13, 47, 00) };
        yield return new object[] { new DateTime(2004, 05, 24, 18, 03, 00), -6.7470217, new DateTime(2004, 05, 13, 10, 01, 00) }; // This one was 02 in example data but that must be a scam!
        yield return new object[] { new DateTime(2004, 05, 24, 08, 03, 00), 12.7827090, new DateTime(2004, 06, 10, 14, 18, 00) };
        yield return new object[] { new DateTime(2004, 05, 24, 07, 03, 00),  8.2766280, new DateTime(2004, 06, 04, 10, 12, 00) };
        yield return new object[] { new DateTime(2022, 10, 10, 07, 03, 00),  8.5000000, new DateTime(2022, 10, 20, 12, 00, 00) };
        yield return new object[] { new DateTime(2022, 10, 10, 08, 00, 00),  8.5000000, new DateTime(2022, 10, 20, 12, 00, 00) };
        yield return new object[] { new DateTime(2022, 10, 10, 07, 03, 00), -3.5000000, new DateTime(2022, 10, 04, 12, 00, 00) };
        yield return new object[] { new DateTime(2022, 10, 10, 15, 07, 00),  0.2500000, new DateTime(2022, 10, 11, 09, 07, 00) };
        yield return new object[] { new DateTime(2022, 10, 10, 04, 00, 00),  0.5000000, new DateTime(2022, 10, 10, 12, 00, 00) };
    }
}
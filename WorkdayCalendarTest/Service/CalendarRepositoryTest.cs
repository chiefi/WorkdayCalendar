using Moq;
using System.Text.Json;
using WorkdayCalendarLibrary.Model;
using WorkdayCalendarLibrary.Service;

namespace WorkdayCalendarTest.Service;

[TestClass]
public class CalendarRepositoryTest
{
    [TestMethod]
    public void Get()
    {
        // Arrange
        var calendar = new Calendar();
        var fileServiceMoq = new Mock<IFileService>();
        fileServiceMoq.Setup(x => x.ReadSettings()).Returns("{\"Holidays\":[{\"Year\":2004,\"Month\":5,\"Day\":27},{\"Year\":2004,\"Month\":6,\"Day\":6},{\"Year\":2004,\"Month\":12,\"Day\":24}],\"RecurringHolidays\":[{\"Month\":5,\"Day\":17}],\"StartOfWorkday\":\"08:00\",\"EndOfWorkday\":\"16:00\"}");

        var calendarConfigurationConverter = new Mock<ICalendarConfigurationConverter>();
        calendarConfigurationConverter.Setup(x => x.ToCalendar(It.IsAny<CalendarConfiguration>())).Returns(calendar);

        var calendarRepository = new CalendarRepository(fileServiceMoq.Object, calendarConfigurationConverter.Object);
        
        // Act
        var result = calendarRepository.Get();

        // Assert
        Assert.AreEqual(calendar, result);

        fileServiceMoq.Verify(x => x.ReadSettings(), Times.AtMostOnce());
        calendarConfigurationConverter.Verify(x => x.ToCalendar(It.IsAny<CalendarConfiguration>()), Times.AtMostOnce());
    }

    [TestMethod]
    public void Get_Multiple()
    {
        // Arrange
        var calendar = new Calendar();
        var fileServiceMoq = new Mock<IFileService>();
        fileServiceMoq.Setup(x => x.ReadSettings()).Returns("{\"Holidays\":[{\"Year\":2004,\"Month\":5,\"Day\":27},{\"Year\":2004,\"Month\":6,\"Day\":6},{\"Year\":2004,\"Month\":12,\"Day\":24}],\"RecurringHolidays\":[{\"Month\":5,\"Day\":17}],\"StartOfWorkday\":\"08:00\",\"EndOfWorkday\":\"16:00\"}");

        var calendarConfigurationConverter = new Mock<ICalendarConfigurationConverter>();
        calendarConfigurationConverter.Setup(x => x.ToCalendar(It.IsAny<CalendarConfiguration>())).Returns(calendar);

        var calendarRepository = new CalendarRepository(fileServiceMoq.Object, calendarConfigurationConverter.Object);

        // Act
        var first = calendarRepository.Get();
        var second = calendarRepository.Get();

        // Assert
        Assert.AreEqual(calendar, first);
        Assert.AreEqual(calendar, second);

        fileServiceMoq.Verify(x => x.ReadSettings(), Times.AtMostOnce());
        calendarConfigurationConverter.Verify(x => x.ToCalendar(It.IsAny<CalendarConfiguration>()), Times.AtMostOnce());
    }

    [TestMethod]
    public void Get_FileReadError()
    {
        // Arrange
        var calendar = new Calendar();
        var fileServiceMoq = new Mock<IFileService>();
        fileServiceMoq.Setup(x => x.ReadSettings()).Throws<FileNotFoundException>();

        var calendarConfigurationConverter = new Mock<ICalendarConfigurationConverter>();
        calendarConfigurationConverter.Setup(x => x.ToCalendar(It.IsAny<CalendarConfiguration>())).Returns(calendar);

        var calendarRepository = new CalendarRepository(fileServiceMoq.Object, calendarConfigurationConverter.Object);

        // Act
        var result = calendarRepository.Get();

        // Assert
        Assert.AreNotEqual(calendar, result);

        fileServiceMoq.Verify(x => x.ReadSettings(), Times.AtMostOnce());
        calendarConfigurationConverter.Verify(x => x.ToCalendar(It.IsAny<CalendarConfiguration>()), Times.Never());
    }

    [TestMethod]
    public void Get_Json_Format_Error()
    {
        // Arrange
        var calendar = new Calendar();
        var fileServiceMoq = new Mock<IFileService>();
        fileServiceMoq.Setup(x => x.ReadSettings()).Returns("THISISNOTJSON");

        var calendarConfigurationConverter = new Mock<ICalendarConfigurationConverter>();
        calendarConfigurationConverter.Setup(x => x.ToCalendar(It.IsAny<CalendarConfiguration>())).Returns(calendar);

        var calendarRepository = new CalendarRepository(fileServiceMoq.Object, calendarConfigurationConverter.Object);

        // Assert
        Assert.ThrowsException<JsonException>(() => calendarRepository.Get());

        fileServiceMoq.Verify(x => x.ReadSettings(), Times.AtMostOnce());
        calendarConfigurationConverter.Verify(x => x.ToCalendar(It.IsAny<CalendarConfiguration>()), Times.Never());
    }

    [TestMethod]
    public void Save_Empty()
    {
        // Arrange
        var fileServiceMoq = new Mock<IFileService>();
        var calendarConfigurationConverter = new Mock<ICalendarConfigurationConverter>();

        var calendarRepository = new CalendarRepository(fileServiceMoq.Object, calendarConfigurationConverter.Object);

        // Act
        calendarRepository.Save();

        // Assert
        fileServiceMoq.Verify(x => x.WriteSettings(It.IsAny<string>()), Times.Never());
        calendarConfigurationConverter.Verify(x => x.ToConfiguration(It.IsAny<Calendar>()), Times.Never());
    }

    [TestMethod]
    public void Save()
    {
        // Arrange
        var fileServiceMoq = new Mock<IFileService>();
        fileServiceMoq.Setup(x => x.WriteSettings(It.IsAny<string>()));

        var calendarConfigurationConverter = new Mock<ICalendarConfigurationConverter>();
        calendarConfigurationConverter.Setup(x => x.ToConfiguration(It.IsAny<Calendar>())).Returns(new CalendarConfiguration());

        var calendarRepository = new CalendarRepository(fileServiceMoq.Object, calendarConfigurationConverter.Object);

        // Act
        calendarRepository.Reset();
        calendarRepository.Save();

        // Assert
        fileServiceMoq.Verify(x => x.WriteSettings(It.IsAny<string>()), Times.Once());
        calendarConfigurationConverter.Verify(x => x.ToConfiguration(It.IsAny<Calendar>()), Times.Once());
    }

    [TestMethod]
    public void Save_Fail()
    {
        // Arrange
        var fileServiceMoq = new Mock<IFileService>();
        fileServiceMoq.Setup(x => x.WriteSettings(It.IsAny<string>())).Throws<IOException>();

        var calendarConfigurationConverter = new Mock<ICalendarConfigurationConverter>();
        calendarConfigurationConverter.Setup(x => x.ToConfiguration(It.IsAny<Calendar>())).Returns(new CalendarConfiguration());

        var calendarRepository = new CalendarRepository(fileServiceMoq.Object, calendarConfigurationConverter.Object);

        // Act
        calendarRepository.Reset();

        // Assert
        Assert.ThrowsException<IOException>(() => calendarRepository.Save());
    }
}

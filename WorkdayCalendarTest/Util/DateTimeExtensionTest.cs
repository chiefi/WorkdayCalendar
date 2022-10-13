using WorkdayCalendarLibrary.Util;

namespace WorkdayCalendarTest.Util;

[TestClass]
public class DateTimeExtensionTest
{
    [TestMethod]
    public void Validate_SetTime()
    {
        // Arrange
        var date = new DateTime(2020, 10, 11, 12, 13, 14);

        // Act
        var updatedDate = date.SetTime(15, 16);

        // Assert
        Assert.AreNotEqual(date, updatedDate);
        Assert.AreEqual("2020-10-11 15:16:00", updatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    [TestMethod]
    public void Validate_SetSecondsToZero()
    {
        // Arrange
        var date = new DateTime(2020, 10, 11, 12, 13, 14);

        // Act
        var updatedDate = date.SetSecondsToZero();

        // Assert
        Assert.AreNotEqual(date, updatedDate);
        Assert.AreEqual("2020-10-11 12:13:00", updatedDate.ToString("yyyy-MM-dd HH:mm:ss"));
    }
}

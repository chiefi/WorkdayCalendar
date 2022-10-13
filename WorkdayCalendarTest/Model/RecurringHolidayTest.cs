using WorkdayCalendarLibrary.Model;

namespace WorkdayCalendarTest.Model;

[TestClass]
public class RecurringHolidayTest
{
    [TestMethod]
    public void RecurringHoliday_Overkill_Test()
    {
        // Arrange
        var holiday = new RecurringHoliday { Month = 5, Day = 17 };

        // Assert
        Assert.AreEqual(5, holiday.Month);
        Assert.AreEqual(17, holiday.Day);
    }

    [TestMethod]
    public void RecurringHoliday_Compare_Holiday()
    {
        // Arrange
        var holiday = new RecurringHoliday { Month = 5, Day = 17 };

        var holiday1 = new RecurringHoliday { Month = 5, Day = 17 };
        var holiday2 = new RecurringHoliday { Month = 6, Day = 17 };
        var holiday3 = new RecurringHoliday { Month = 5, Day = 18 };

        // Act
        var result1 = holiday.Equals(holiday1);
        var result2 = holiday.Equals(holiday2);
        var result3 = holiday.Equals(holiday3);

        // Assert
        Assert.IsTrue(result1);
        Assert.IsFalse(result2);
        Assert.IsFalse(result3);
    }

    [TestMethod]
    public void RecurringHoliday_Compare_DateTime()
    {
        // Arrange
        var holiday = new RecurringHoliday { Month = 5, Day = 17 };

        var date1 = new DateTime(2004, 5, 17);
        var date2 = new DateTime(2005, 5, 17);
        var date3 = new DateTime(2004, 6, 17);
        var date4 = new DateTime(2004, 5, 18);

        // Act
        var result1 = holiday.IsDate(date1);
        var result2 = holiday.IsDate(date2);
        var result3 = holiday.IsDate(date3);
        var result4 = holiday.IsDate(date4);

        // Assert
        Assert.IsTrue(result1);
        Assert.IsTrue(result2);
        Assert.IsFalse(result3);
        Assert.IsFalse(result4);
    }
}

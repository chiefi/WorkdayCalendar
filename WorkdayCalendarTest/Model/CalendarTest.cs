using WorkdayCalendarLibrary.Model;

namespace WorkdayCalendarTest.Model;

[TestClass]
public class CalendarTest
{
    [TestMethod]
    public void Add_Holiday()
    {
        // Arrange
        var calendar = new Calendar();

        // Act
        var added = calendar.RegisterHoliday(new Holiday { Year = 2004, Month = 5, Day = 17 });

        // Assert
        Assert.IsTrue(added, "Expected result to be true for adding a new holiday.");
        Assert.IsFalse(calendar.IsWorkday(new DateTime(2004, 5, 17)));
        Assert.IsTrue(calendar.IsWorkday(new DateTime(2022, 5, 17)));
        Assert.AreEqual(1, calendar.GetHolidays().Count());
    }

    [TestMethod]
    public void Add_Duplicate_Holiday()
    {
        // Arrange
        var calendar = new Calendar();

        // Act
        var first = calendar.RegisterHoliday(new Holiday { Year = 2004, Month = 5, Day = 17 });
        var second = calendar.RegisterHoliday(new Holiday { Year = 2004, Month = 5, Day = 17 });

        // Assert
        Assert.IsTrue(first, "Expected result to be true for adding a holiday.");
        Assert.IsFalse(second, "Expected result to be false for adding a duplicate holiday.");
        Assert.IsFalse(calendar.IsWorkday(new DateTime(2004, 5, 17)));
        Assert.AreEqual(1, calendar.GetHolidays().Count());
    }

    [TestMethod]
    public void Add_RecurringHoliday()
    {
        // Arrange
        var calendar = new Calendar();

        // Act
        var added = calendar.RegisterRecurringHoliday(new RecurringHoliday { Month = 5, Day = 17 });

        // Assert
        Assert.IsTrue(added, "Expected result to be true for adding a new recurring holiday.");
        Assert.IsFalse(calendar.IsWorkday(new DateTime(2004, 5, 17)));
        Assert.IsFalse(calendar.IsWorkday(new DateTime(2022, 5, 17)));
        Assert.AreEqual(1, calendar.GetRecurringHolidays().Count());
    }

    [TestMethod]
    public void Add_Duplicate_RecurringHoliday()
    {
        // Arrange
        var calendar = new Calendar();

        // Act
        var first = calendar.RegisterRecurringHoliday(new RecurringHoliday { Month = 5, Day = 17 });
        var second = calendar.RegisterRecurringHoliday(new RecurringHoliday { Month = 5, Day = 17 });

        // Assert
        Assert.IsTrue(first, "Expected result to be true for adding a recurring holiday.");
        Assert.IsFalse(second, "Expected result to be false for adding a duplicate recurring holiday.");
        Assert.IsFalse(calendar.IsWorkday(new DateTime(2004, 5, 17)));
        Assert.AreEqual(1, calendar.GetRecurringHolidays().Count());
    }

    [TestMethod]
    public void Remove_Holiday()
    {
        // Arrange
        var calendar = new Calendar();
        var holiday = new Holiday { Year = 2004, Month = 5, Day = 17 };
        calendar.RegisterHoliday(holiday);

        // Act
        var removed = calendar.RemoveHoliday(holiday);

        // Assert
        Assert.IsTrue(removed, "Expected result to be true for removing a holiday.");
        Assert.IsTrue(calendar.IsWorkday(new DateTime(2004, 5, 17)));
        Assert.IsTrue(calendar.IsWorkday(new DateTime(2022, 5, 17)));
        Assert.AreEqual(0, calendar.GetHolidays().Count());
    }

    [TestMethod]
    public void Remove_NonExisting_Holiday()
    {
        // Arrange
        var calendar = new Calendar();
        var holiday = new Holiday { Year = 2005, Month = 5, Day = 17 };
        calendar.RegisterHoliday(new Holiday { Year = 2004, Month = 5, Day = 17 });

        // Act
        var removed = calendar.RemoveHoliday(holiday);

        // Assert
        Assert.IsFalse(removed, "Expected result to be false for removing a non-existing holiday.");
        Assert.AreEqual(1, calendar.GetHolidays().Count());
    }

    [TestMethod]
    public void Remove_RecurringHoliday()
    {
        // Arrange
        var calendar = new Calendar();
        var recurringHoliday = new RecurringHoliday { Month = 5, Day = 17 };
        calendar.RegisterRecurringHoliday(recurringHoliday);

        // Act
        var removed = calendar.RemoveRecurringHoliday(recurringHoliday);

        // Assert
        Assert.IsTrue(removed, "Expected result to be true for removing a recurring holiday.");
        Assert.IsTrue(calendar.IsWorkday(new DateTime(2004, 5, 17)));
        Assert.IsTrue(calendar.IsWorkday(new DateTime(2022, 5, 17)));
        Assert.AreEqual(0, calendar.GetHolidays().Count());
    }

    [TestMethod]
    public void Remove_NonExisting_RecurringHoliday()
    {
        // Arrange
        var calendar = new Calendar();
        var recurringHoliday = new RecurringHoliday { Month = 5, Day = 17 };
        calendar.RegisterRecurringHoliday(new RecurringHoliday { Month = 5, Day = 18 });

        // Act
        var removed = calendar.RemoveRecurringHoliday(recurringHoliday);

        // Assert
        Assert.IsFalse(removed, "Expected result to be false for removing a non-existing recurring holiday.");
        Assert.AreEqual(1, calendar.GetRecurringHolidays().Count());
    }

    [TestMethod]
    public void Is_Saturday_Workday()
    {
        // Arrange
        var calendar = new Calendar();

        // Assert
        Assert.IsFalse(calendar.IsWorkday(new DateTime(2022, 10, 15)));
    }

    [TestMethod]
    public void Is_Sunday_Workday()
    {
        // Arrange
        var calendar = new Calendar();

        // Assert
        Assert.IsFalse(calendar.IsWorkday(new DateTime(2022, 10, 16)));
    }

    [TestMethod]
    public void Is_Monday_Workday()
    {
        // Arrange
        var calendar = new Calendar();

        // Assert
        Assert.IsTrue(calendar.IsWorkday(new DateTime(2022, 10, 17)));
    }

    [TestMethod]
    public void Set_StartOfWorkday()
    {
        // Arrange
        var calendar = new Calendar
        {
            StartOfWorkday = new TimeOnly(8, 0),
            EndOfWorkday = new TimeOnly(10, 0)
        };

        // Act
        calendar.StartOfWorkday = new TimeOnly(10, 0);

        // Assert
        Assert.IsTrue(calendar.StartOfWorkday == new TimeOnly(10, 0));
    }

    [TestMethod]
    public void Set_EndOfWorkday()
    {
        // Arrange
        var calendar = new Calendar
        {
            StartOfWorkday = new TimeOnly(8, 0),
            EndOfWorkday = new TimeOnly(16, 0)
        };

        // Act
        calendar.EndOfWorkday = new TimeOnly(10, 0);

        // Assert
        Assert.IsTrue(calendar.EndOfWorkday == new TimeOnly(10, 0));
    }

    [TestMethod]
    public void Test_GetLengthOfWorkday_Full_Hours()
    {
        // Arrange
        var calendar = new Calendar
        {
            StartOfWorkday = new TimeOnly(8, 0),
            EndOfWorkday = new TimeOnly(10, 0)
        };

        // Act
        var length = calendar.GetLengthOfWorkday();

        // Assert
        Assert.AreEqual(120, length);
    }

    [TestMethod]
    public void Test_GetLengthOfWorkday_Minute()
    {
        // Arrange
        var calendar = new Calendar
        {
            StartOfWorkday = new TimeOnly(8, 0),
            EndOfWorkday = new TimeOnly(8, 1)
        };

        // Act
        var length = calendar.GetLengthOfWorkday();

        // Assert
        Assert.AreEqual(1, length);
    }

    [TestMethod]
    public void Test_GetLengthOfWorkday_Minutes()
    {
        // Arrange
        var calendar = new Calendar
        {
            StartOfWorkday = new TimeOnly(8, 0),
            EndOfWorkday = new TimeOnly(10, 1)
        };

        // Act
        var length = calendar.GetLengthOfWorkday();

        // Assert
        Assert.AreEqual(121, length);
    }

    [TestMethod]
    public void Test_GetLengthOfWorkday_Zero_Case()
    {
        // Arrange
        var calendar = new Calendar
        {
            StartOfWorkday = new TimeOnly(8, 0),
            EndOfWorkday = new TimeOnly(8, 0)
        };

        // Act
        var length = calendar.GetLengthOfWorkday();

        // Assert
        Assert.AreEqual(0, length);
    }

    [TestMethod]
    public void Test_GetLengthOfWorkday_Invalid_Workday()
    {
        // Arrange
        var calendar = new Calendar
        {
            StartOfWorkday = new TimeOnly(8, 01),
            EndOfWorkday = new TimeOnly(8, 00)
        };

        // Assert
        Assert.ThrowsException<InvalidOperationException>(() => calendar.GetLengthOfWorkday());
    }
}

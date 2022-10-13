using System.Text.Json;
using System.Text.Json.Serialization;
using WorkdayCalendarLibrary.Util;

namespace WorkdayCalendarTest.Util;

[TestClass]
public class TimeOnlyJsonConverterTest
{
    [TestMethod]
    public void Serialization_Test()
    {
        // Arrange
        var dataObject = new TimeOnlyData { TimeOfDay = new TimeOnly(10, 03) };
        
        // Act
        var jsonString = JsonSerializer.Serialize(dataObject)!;

        // Assert
        var expected = "{\"TimeOfDay\":\"10:03\"}";
        Assert.AreEqual(jsonString, expected);
    }

    [TestMethod]
    public void DeserializationTest()
    {
        // Arrange
        var jsonString = "{\"TimeOfDay\":\"10:03\"}";

        // Act
        var dataObject = JsonSerializer.Deserialize<TimeOnlyData>(jsonString)!;

        // Assert
        Assert.AreEqual(dataObject.TimeOfDay.Hour, 10);
        Assert.AreEqual(dataObject.TimeOfDay.Minute, 03);
    }

    private class TimeOnlyData
    {
        [JsonConverter(typeof(TimeOnlyJsonConverter))]
        public TimeOnly TimeOfDay { get; set; }
    }
}


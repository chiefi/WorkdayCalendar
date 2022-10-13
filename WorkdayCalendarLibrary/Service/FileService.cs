namespace WorkdayCalendarLibrary.Service;

/// <summary>
/// Reads or writes JSON to File.
/// </summary>
public interface IFileService
{
    /// <summary>
    /// Reads JSON from file.
    /// </summary>
    /// <returns>The JSON text as a string.</returns>
    string ReadSettings();
    /// <summary>
    /// Writes JSON to file.
    /// </summary>
    /// <param name="json">The JSON text as a string.</param>
    void WriteSettings(string json);
}

public class FileService : IFileService
{
    private const string FILENAME = "calendarsettings.json";

    public string ReadSettings()
    {
        string jsonString = File.ReadAllText(FILENAME);

        return jsonString;
    }

    public void WriteSettings(string json)
    {
        File.WriteAllText(FILENAME, json);
    }
}

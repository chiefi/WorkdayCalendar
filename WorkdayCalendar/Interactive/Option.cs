namespace WorkdayCalendar.Interactive;

/// <summary>
/// Support class for combining Enum with a descriptive text when creating selection prompts.
/// </summary>
/// <typeparam name="T"></typeparam>
internal sealed class Option<T> where T : Enum
{
    /// <summary>
    /// Enum value of the Option.
    /// </summary>
    public T Value { get; }

    /// <summary>
    /// Description text of the Option.
    /// </summary>
    public string Text { get; }

    /// <summary>
    /// Instantiates an Option object with an enum and a description.
    /// </summary>
    /// <param name="value">The enum value.</param>
    /// <param name="text">The description text.</param>
    public Option(T value, string text)
    {
        Value = value;
        Text = text;
    }
}

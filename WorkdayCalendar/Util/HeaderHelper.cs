using Spectre.Console;

namespace WorkdayCalendar.Util;

/// <summary>
/// Helper class to print console header.
/// </summary>
internal static class HeaderHelper
{
    /// <summary>
    /// Clears the console and prints a header.
    /// </summary>
    internal static void ShowHeader()
    {
        AnsiConsole.Clear();

        AnsiConsole.Write(
            new FigletText("Workday Calendar")
                .LeftAligned()
                .Color(Color.BlueViolet));
    }
}

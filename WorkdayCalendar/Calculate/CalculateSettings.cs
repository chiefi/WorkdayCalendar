using Spectre.Console;
using Spectre.Console.Cli;
using System.ComponentModel;
using WorkdayCalendar.Util;

namespace WorkdayCalendar.Calculate;

/// <summary>
/// Cli settings for the Calculate Command.
/// </summary>
internal sealed class CalculateSettings : CommandSettings
{
    [Description($"Date to start calculation from. Should be in format {DateTimeHelper.DateTimeFormat}")]
    [CommandOption("--startDate")]
    public string? StartDate { get; init; }

    [Description("Number of days to increment, can be negative. Should be expressed as a double.")]
    [CommandOption("--increment")]
    public double? Increment { get; init; }

    public override ValidationResult Validate()
    {
        if (!DateTimeHelper.IsValidDateTime(StartDate))
            return ValidationResult.Error($"StartDate must be in the format {DateTimeHelper.DateTimeFormat}");
        if (Increment == null)
            return ValidationResult.Error("Increment must be set. Supply an increment value using --increment.");

        return ValidationResult.Success();
    }
}
using Spectre.Console;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using WorkdayCalendar.Util;
using WorkdayCalendarLibrary.Service;

namespace WorkdayCalendar.Calculate;

/// <summary>
/// Handler for the Calculate command.
/// Performs calculation of workdays using supplied parameters.
/// </summary>
internal sealed class CalculateCommand : Command<CalculateSettings>
{
    private readonly ICalendarService calendarService;
    private readonly ICalendarRepository calendarRepository;

    public CalculateCommand(
        ICalendarService calendarService,
        ICalendarRepository calendarRepository)
    {
        this.calendarService = calendarService;
        this.calendarRepository = calendarRepository;
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] CalculateSettings settings)
    {
        var calendar = calendarRepository.Get();
        var startDate = DateTimeHelper.ParseDateTime(settings.StartDate!);
        var increment = settings.Increment!.Value;

        var date = calendarService.FindWorkday(calendar, startDate, increment);

        AnsiConsole.MarkupLine($"[darkolivegreen1_1]{startDate.ToString(DateTimeHelper.DateTimeFormat)}[/] with the addition of [darkolivegreen1_1]{increment}[/] working days is [green]{date.ToString(DateTimeHelper.DateTimeFormat)}[/]");

        return 0;
    }
}

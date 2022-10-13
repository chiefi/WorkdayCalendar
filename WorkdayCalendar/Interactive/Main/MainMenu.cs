using Spectre.Console;
using WorkdayCalendar.Interactive.Holidays;
using WorkdayCalendar.Util;
using WorkdayCalendarLibrary.Service;
using Calendar = WorkdayCalendarLibrary.Model.Calendar;

namespace WorkdayCalendar.Interactive.Main;

/// <summary>
/// Handler for showing and handling everything around the Main Menu.
/// </summary>
internal interface IMainMenu
{
    /// <summary>
    /// Shows the Main Menu.
    /// </summary>
    /// <returns>True if the prompt should be shown again, else false.</returns>
    bool ShowPrompt();
}

internal sealed class MainMenu : IMainMenu
{
    private readonly ICalendarService calendarService;
    private readonly ICalendarRepository calendarRepository;
    private readonly IHolidayMenu holidayMenu;

    public MainMenu(ICalendarService calendarService,
        ICalendarRepository calendarRepository,
        IHolidayMenu holidayMenu)
    {
        this.calendarService = calendarService;
        this.calendarRepository = calendarRepository;
        this.holidayMenu = holidayMenu;
    }

    public bool ShowPrompt()
    {
        HeaderHelper.ShowHeader();

        var calendar = calendarRepository.Get();

        var selection = AnsiConsole.Prompt(
            new SelectionPrompt<Option<MainOptions>>()
                .Title("What do you want to do?")
                .AddChoices(new[] {
                    new Option<MainOptions>(MainOptions.Calculate, "Calculate Workday"),
                    new Option<MainOptions>(MainOptions.ChangeStartOfWorkday, $"Change start of workday ([darkolivegreen1_1]{calendar.StartOfWorkday}[/])"),
                    new Option<MainOptions>(MainOptions.ChangeEndofWorkday, $"Change end of workday ([darkolivegreen1_1]{calendar.EndOfWorkday}[/])"),
                    new Option<MainOptions>(MainOptions.ManageHolidays, "Manage Holidays"),
                    new Option<MainOptions>(MainOptions.Reset, "Reset all settings to default"),
                    new Option<MainOptions>(MainOptions.Exit, "Exit")
                })
                .UseConverter(x => x.Text)
            ).Value;

        switch (selection)
        {
            case MainOptions.Calculate:
                Calculate(calendar);
                break;
            case MainOptions.ChangeStartOfWorkday:
                ChangeStartOfWorkday(calendar);
                break;
            case MainOptions.ChangeEndofWorkday:
                ChangeEndOfWorkday(calendar);
                break;
            case MainOptions.ManageHolidays:
                while (holidayMenu.ShowPrompt());
                break;
            case MainOptions.Reset:
                Reset();
                break;
            case MainOptions.Exit:
                return false;
            default:
                break;
        }

        return true;
    }

    private void Calculate(Calendar calendar)
    {
        var input = AnsiConsole.Prompt(
                new TextPrompt<string>($"Enter [darkolivegreen1_1]start time[/] ({DateTimeHelper.DateTimeFormat}):")
                    .PromptStyle("darkolivegreen1_1")
                    .ValidationErrorMessage("[red]That's not valid input[/]")
                    .Validate(time =>
                    {
                        if (!DateTimeHelper.IsValidDateTime(time))
                            return ValidationResult.Error($"[red]You must enter a time in the format {DateTimeHelper.DateTimeFormat}[/]");
                        return ValidationResult.Success();
                    }));
        var startDate = DateTimeHelper.ParseDateTime(input);

        var increment = AnsiConsole.Prompt(
            new TextPrompt<double>("Enter [darkolivegreen1_1]increment[/] (positive or negative decimal number):")
                .PromptStyle("darkolivegreen1_1")
                .ValidationErrorMessage("[red]That's not valid input[/]")
                );

        var date = calendarService.FindWorkday(calendar, startDate, increment);

        AnsiConsole.MarkupLine($"[darkolivegreen1_1]{startDate.ToString(DateTimeHelper.DateTimeFormat)}[/] with the addition of [darkolivegreen1_1]{increment}[/] working days is [green]{date.ToString(DateTimeHelper.DateTimeFormat)}[/]");

        Console.ReadKey();
    }

    private void ChangeStartOfWorkday(Calendar calendar)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>($"Enter new [darkolivegreen1_1]start[/] of workday in format ({DateTimeHelper.TimeFormat}):")
                .PromptStyle("darkolivegreen1_1")
                .ValidationErrorMessage("[red]That's not a valid time[/]")
                .Validate(time =>
                {
                    if (!DateTimeHelper.IsValidTime(time))
                        return ValidationResult.Error($"[red]You must enter a time in the format {DateTimeHelper.TimeFormat}[/]");
                    return ValidationResult.Success();
                }));

        var newTime = DateTimeHelper.ParseTime(input);
        calendar.StartOfWorkday = newTime;

        calendarRepository.Save();
    }

    private void ChangeEndOfWorkday(Calendar calendar)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>($"Enter new [darkolivegreen1_1]end[/] of workday in format ({DateTimeHelper.TimeFormat}):")
                .PromptStyle("darkolivegreen1_1")
                .ValidationErrorMessage("[red]That's not a valid time[/]")
                .Validate(time =>
                {
                    if (!DateTimeHelper.IsValidTime(time))
                        return ValidationResult.Error($"[red]You must enter a time in the format {DateTimeHelper.TimeFormat}[/]");
                    return ValidationResult.Success();
                }));

        var newTime = DateTimeHelper.ParseTime(input);
        calendar.EndOfWorkday = newTime;

        calendarRepository.Save();
    }

    private void Reset()
    {
        var result = AnsiConsole.Confirm("Are you sure you want to reset all settings?", false);
        
        if (!result)
            return;

        calendarRepository.Reset();
        calendarRepository.Save();
    }
}

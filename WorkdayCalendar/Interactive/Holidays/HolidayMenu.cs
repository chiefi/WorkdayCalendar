using Spectre.Console;
using WorkdayCalendar.Util;
using WorkdayCalendarLibrary.Model;
using WorkdayCalendarLibrary.Service;
using Calendar = WorkdayCalendarLibrary.Model.Calendar;

namespace WorkdayCalendar.Interactive.Holidays;

/// <summary>
/// Handler for showing and handling everything around the Holiday Menu.
/// </summary>
internal interface IHolidayMenu
{
    /// <summary>
    /// Shows the Holiday Menu.
    /// </summary>
    /// <returns>True if the prompt should be shown again, else false.</returns>
    bool ShowPrompt();
}

internal sealed class HolidayMenu : IHolidayMenu
{
    private readonly ICalendarRepository calendarRepository;

    public HolidayMenu(ICalendarRepository calendarRepository)
    {
        this.calendarRepository = calendarRepository;
    }

    public bool ShowPrompt()
    {
        HeaderHelper.ShowHeader();

        var calendar = calendarRepository.Get();

        var holidayselection = AnsiConsole.Prompt(
            new SelectionPrompt<Option<HolidayOptions>>()
                .Title("What do you want to do?")
                .AddChoices(new[] {
                    new Option<HolidayOptions>(HolidayOptions.ViewHolidays, "View Holidays"),
                    new Option<HolidayOptions>(HolidayOptions.AddHoliday, "Add Holiday"),
                    new Option<HolidayOptions>(HolidayOptions.AddRecurringHoliday, "Add Recurring Holiday"),
                    new Option<HolidayOptions>(HolidayOptions.RemoveHoliday, "Remove Holiday"),
                    new Option<HolidayOptions>(HolidayOptions.RemoveRecurringHoliday, "Remove Recurring Holiday"),
                    new Option<HolidayOptions>(HolidayOptions.Back, "Back")
                })
                .UseConverter(x => x.Text)
            ).Value;

        switch (holidayselection)
        {
            case HolidayOptions.ViewHolidays:
                ViewHolidays(calendar);
                break;
            case HolidayOptions.AddHoliday:
                AddHoliday(calendar);
                break;
            case HolidayOptions.AddRecurringHoliday:
                AddRecurringHoliday(calendar);
                break;
            case HolidayOptions.RemoveHoliday:
                RemoveHoliday(calendar);
                break;
            case HolidayOptions.RemoveRecurringHoliday:
                RemoveRecurringHoliday(calendar);
                break;
            case HolidayOptions.Back:
                return false;
            default:
                break;
        }

        return true;
    }

    private void ViewHolidays(Calendar calendar)
    {
        var holidayTable = new Table();
        holidayTable.Title = new TableTitle("[red]Holidays[/]");
        holidayTable.Border = TableBorder.MinimalHeavyHead;

        holidayTable.AddColumn("[green]Day[/]");
        holidayTable.AddColumn("[green]Month[/]");
        holidayTable.AddColumn("[green]Year[/]");
        
        foreach (var holiday in calendar.GetHolidays())
            holidayTable.AddRow(holiday.Day.ToString("d2"), holiday.Month.ToString("d2"), holiday.Year.ToString("d4"));

        holidayTable.Expand();

        AnsiConsole.Write(holidayTable);

        var recurringHolidayTable = new Table();
        recurringHolidayTable.Title = new TableTitle("[red]Recurring Holidays[/]");
        recurringHolidayTable.Border = TableBorder.MinimalHeavyHead;

        recurringHolidayTable.AddColumn("[green]Day[/]");
        recurringHolidayTable.AddColumn("[green]Month[/]");
        recurringHolidayTable.AddColumn("[green]Year[/]");
        
        foreach (var holiday in calendar.GetRecurringHolidays())
            recurringHolidayTable.AddRow(holiday.Day.ToString("d2"), holiday.Month.ToString("d2"), "*");

        recurringHolidayTable.Expand();

        AnsiConsole.Write(recurringHolidayTable);

        Console.ReadKey();
    }

    private void AddHoliday(Calendar calendar)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>($"Enter new holiday in format ([darkolivegreen1_1]{DateTimeHelper.DateFormat}[/]):")
                .PromptStyle("darkolivegreen1_1")
                .ValidationErrorMessage("[red]That's not a valid date[/]")
                .Validate(date =>
                {
                    if (!DateTimeHelper.IsValidDate(date))
                        return ValidationResult.Error($"[red]You must enter a date in the format {DateTimeHelper.DateFormat}[/]");
                    return ValidationResult.Success();
                }));

        var newDate = DateTimeHelper.ParseDate(input);

        var result = calendar.RegisterHoliday(new Holiday { Year = newDate.Year, Month = newDate.Month, Day = newDate.Day });
        if (!result)
        {
            AnsiConsole.MarkupLine($"[red]Unable to add holiday, since it already exists in calendar.[/] Press any key to continue...");
            Console.ReadKey();
            return;
        }

        calendarRepository.Save();
    }

    private void AddRecurringHoliday(Calendar calendar)
    {
        var input = AnsiConsole.Prompt(
            new TextPrompt<string>($"Enter new recurring holiday in format ([darkolivegreen1_1]{DateTimeHelper.RecurringDateFormat}[/]):")
                .PromptStyle("darkolivegreen1_1")
                .ValidationErrorMessage("[red]That's not a valid date[/]")
                .Validate(date =>
                {
                    if (!DateTimeHelper.IsValidRecurringDate(date))
                        return ValidationResult.Error($"[red]You must enter a date in the format {DateTimeHelper.RecurringDateFormat}[/]");
                    return ValidationResult.Success();
                }));

        var newDate = DateTimeHelper.ParseRecurringDate(input);

        var result = calendar.RegisterRecurringHoliday(new RecurringHoliday { Month = newDate.Month, Day = newDate.Day });
        if (!result)
        {
            AnsiConsole.MarkupLine($"[red]Unable to add recurring holiday, since it already exists in calendar.[/] Press any key to continue...");
            Console.ReadKey();
            return;
        }

        calendarRepository.Save();
    }

    private void RemoveHoliday(Calendar calendar)
    {
        var markedForRemoval = AnsiConsole.Prompt(
                new MultiSelectionPrompt<Holiday>()
                    .Title("Select [darkolivegreen1_1]holidays[/] to remove")
                    .NotRequired()
                    .PageSize(10)
                    .UseConverter(x => $"{x.Day:d2}-{x.Month:d2}-{x.Year:d4}")
                    .MoreChoicesText("[grey](Move up and down to reveal more)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a holiday for removal, " +
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(calendar.GetHolidays()));

        foreach (var holiday in markedForRemoval)
            calendar.RemoveHoliday(holiday);

        calendarRepository.Save();
    }

    private void RemoveRecurringHoliday(Calendar calendar)
    {
        var markedForRemoval = AnsiConsole.Prompt(
                new MultiSelectionPrompt<RecurringHoliday>()
                    .Title("Select [darkolivegreen1_1]recurring holidays[/] to remove")
                    .NotRequired()
                    .PageSize(10)
                    .UseConverter(x => $"{x.Day:d2}-{x.Month:d2}-*")
                    .MoreChoicesText("[grey](Move up and down to reveal more)[/]")
                    .InstructionsText(
                        "[grey](Press [blue]<space>[/] to toggle a holiday for removal, " +
                        "[green]<enter>[/] to accept)[/]")
                    .AddChoices(calendar.GetRecurringHolidays()));

        foreach (var recurringHoliday in markedForRemoval)
            calendar.RemoveRecurringHoliday(recurringHoliday);

        calendarRepository.Save();
    }
}

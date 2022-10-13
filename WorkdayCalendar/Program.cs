using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Spectre.Console.Cli;
using WorkdayCalendar.Calculate;
using WorkdayCalendar.Interactive;
using WorkdayCalendar.Interactive.Holidays;
using WorkdayCalendar.Interactive.Main;
using WorkdayCalendar.Util;
using WorkdayCalendarLibrary.Service;

var host = CreateHostBuilder(args);
var app = CreateCommandApp(host);

HeaderHelper.ShowHeader();

return app.Run(args);

static CommandApp CreateCommandApp(IHostBuilder host)
{
    var registrar = new CustomRegistrar(host);
    var app = new CommandApp(registrar);

    app.Configure(config =>
    {
        config.SetApplicationName("WorkdayCalendar");
        config.ValidateExamples();
        config.AddCommand<InteractiveCommand>("interactive")
            .WithDescription("Starts in interactive mode.")
            .WithExample(new[] { "interactive" });
        config.AddCommand<CalculateCommand>("calc")
            .WithDescription("Performs workday calculation based on the supplied parameters.")
            .WithExample(new[] { "calc", "--startDate=\"27-04-2004 12:00\" --increment=-3.12341" });
    });

    return app;
}

static IHostBuilder CreateHostBuilder(string[] args)
{
    return Host.CreateDefaultBuilder(args)
    .ConfigureServices((_, services) =>
    {
        services.AddSingleton<ICalendarRepository, CalendarRepository>();
        services.AddTransient<IMainMenu, MainMenu>();
        services.AddTransient<IHolidayMenu, HolidayMenu>();
        services.AddTransient<ICalendarService, CalendarService>();
        services.AddTransient<ICalendarConfigurationConverter, CalendarConfigurationConverter>();
        services.AddTransient<IFileService, FileService>();
    });
}
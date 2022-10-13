using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;
using System.Diagnostics.CodeAnalysis;
using WorkdayCalendar.Interactive.Main;

namespace WorkdayCalendar.Interactive;

/// <summary>
/// Handler for the Interactive command.
/// Runs the tool in interactive mode allowing input from user using console.
/// </summary>
internal sealed class InteractiveCommand : Command<InteractiveSettings>
{
    private readonly ILogger<InteractiveCommand> logger;
    private readonly IMainMenu mainMenu;

    public InteractiveCommand(ILogger<InteractiveCommand> logger,
        IMainMenu mainMenu)
    {
        this.logger = logger;
        this.mainMenu = mainMenu;
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] InteractiveSettings settings)
    {
        logger.LogDebug("Starting Interactive Mode...");

        while (mainMenu.ShowPrompt());

        return 0;
    }
}
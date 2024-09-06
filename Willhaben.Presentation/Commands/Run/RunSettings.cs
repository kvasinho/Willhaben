using System.ComponentModel;
using Spectre.Console.Cli;
using Willhaben.Domain.Models;

namespace Willhaben.Presentation.Commands.Run;

public class RunSettings: CommandSettings
{
    [CommandOption("--monitor")]
    [DefaultValue(false)]
    public bool Monitor { get; init; }
}
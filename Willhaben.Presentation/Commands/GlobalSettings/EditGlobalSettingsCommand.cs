using System.ComponentModel;
using System.Reflection;
using Spectre.Console;
using Spectre.Console.Cli;
using Willhaben.Domain.Exceptions;
using Willhaben.Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.VisualBasic;
using Willhaben.Domain.StronglyTypedIds;
using Willhaben.Domain.Utils;
using Willhaben.Domain.Utils.Converters;
using Willhaben.Presentation.Commands;

namespace Willhaben.Presentation.Commands;



public class EditGlobalSettingsCommand : Command<EditSettings>
{
    public override int Execute(CommandContext context, EditSettings settings)
    {
        
        var global = new GlobalSettings();
            global.MinBreakBetweenScrapes = SharedPrompts.PromptForInteger(
                "How much time should be between 2 scrapes of the same page? ", (i => i > 0),
                global.MinBreakBetweenScrapes);
            global.Logging.LogLevel = Commands.SharedPrompts.PromptForSingleEnum<LogLevel>("At which level should be logged?");
            global.Logging.LogRotation =
                Commands.SharedPrompts.PromptForSingleEnum<LogRotation>("How frequently should logs be rotated");
            global.Connection.ConnectionTimeout = Commands.SharedPrompts.PromptForInteger("Whats the connection timeout on your side?",
                (i => i > 0), global.Connection.ConnectionTimeout);
            global.Connection.RequestTimeout = Commands.SharedPrompts.PromptForInteger("Whats the connection timeout on your side?",
                (i => i > 0), global.Connection.RequestTimeout);
            global.Connection.MaxRetries = Commands.SharedPrompts.PromptForInteger("Whats the connection timeout on your side?",
                (i => i > 0), global.Connection.MaxRetries);

                        
            global.ToJson(@$"../../Settings/Global/globalSettings.json");

            return 0;
        }
    }


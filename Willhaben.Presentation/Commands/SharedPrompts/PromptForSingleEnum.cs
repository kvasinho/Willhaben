using Spectre.Console;
using Willhaben.Domain.Utils;

namespace Willhaben.Presentation.Commands;

static class SharedPrompts
{
    public static TEnum PromptForSingleEnum<TEnum>(string prompt) where TEnum : Enum
    {
        var choice = AnsiConsole.Prompt(
            new SelectionPrompt<TEnum>()
                .Title(prompt)
                .PageSize(3)
                .AddChoices(EnumExtensions.GetAllValues<TEnum>()));
        return choice;
    }

    public static int PromptForInteger(string question, Func<int, bool> validation, int defaultValue = 0)
    {
        while (true)
        {
            var input = AnsiConsole.Prompt(
                new TextPrompt<string>(question)
                    .AllowEmpty()
            );

            if (string.IsNullOrEmpty(input))
            {
                if (validation(defaultValue))
                {
                    return defaultValue;
                }
                else
                {
                    AnsiConsole.MarkupLine("[red]Invalid input. Please enter a valid integer.[/]");
                    continue;
                }
            }

            if (int.TryParse(input, out int result) && validation(result))
            {
                return result;
            }
            else
            {
                AnsiConsole.MarkupLine("[red]Invalid input. Please enter a valid integer.[/]");
            }
        }
    }
}
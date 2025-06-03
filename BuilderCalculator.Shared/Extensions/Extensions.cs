using Spectre.Console;

namespace Calculators.Shared.Extensions
{
    public static class Extensions
    {
        private static readonly Style Style = new Style(Color.Aquamarine1);
        
        public static T EnterParameter<T>(this string text, T defaultValue)
        {
            return AnsiConsole
                .Prompt(new TextPrompt<T>(text.MarkupSecondaryColor())
                    .PromptStyle(Style)
                    .DefaultValue(defaultValue));
        }
        
        public static string MarkupSecondaryColor(this string str)
        {
            return $"[yellow1]{str}[/]";
        }
    
        public static string MarkupPrimaryColor(this string str)
        {
            return $"[aquamarine1]{str}[/]";
        }
    
        public static string MarkupErrorColor(this string str)
        {
            return $"[red3_1]{str}[/]";
        }
    }
}
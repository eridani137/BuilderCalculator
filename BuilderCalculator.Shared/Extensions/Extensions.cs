using Spectre.Console;

namespace Calculators.Shared.Extensions
{
    public static class Extensions
    {
        private static readonly Style Style = new Style(Color.Aquamarine1);
        
        public static double EnterDoubleParameter(this string text, double defaultValue)
        {
            return AnsiConsole
                .Prompt(new TextPrompt<double>(text.MarkupSecondaryColor())
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
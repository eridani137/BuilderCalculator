using System;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace Calculators.KZH_04
{
    internal static class Program
    {
        public static void Main()
        {
            var calc = new CheckingCrackAndOpeningWidth();
            
            calc.EnteringParameters();
            var result = calc.Calculate();
            result?.PrintSummary();

            AnsiConsole.MarkupLine("Нажмите любую клавишу для выхода...".MarkupPrimaryColor());
            Console.ReadKey(false);
        }
    }
}
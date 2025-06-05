using System;
using Calculators.Shared.Enums;
using Spectre.Console;

namespace Calculators.Shared.Extensions
{
    public static class Extensions
    {
        private static readonly Style Style = new Style(Color.Aquamarine1);
        
        public static double Clamp(this double value, double min, double max)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }

        public static double GetRb(this ConcreteClass concreteClass)
        {
            switch (concreteClass)
            {
                case ConcreteClass.B10: return 61.2;
                case ConcreteClass.B15: return 86.6;
                case ConcreteClass.B20: return 117.2;
                case ConcreteClass.B25: return 147.8;
                case ConcreteClass.B30: return 173.3;
                case ConcreteClass.B35: return 198.8;
                case ConcreteClass.B40: return 224.3;
                case ConcreteClass.B45: return 254.8;
                case ConcreteClass.B50: return 280.3;
                case ConcreteClass.B55: return 305.8;
                case ConcreteClass.B70: return 377.2;
                case ConcreteClass.B80: return 417.9;
                case ConcreteClass.B90: return 448.5;
                case ConcreteClass.B100: return 484.2;
                default:
                    throw new ArgumentOutOfRangeException(nameof(concreteClass), concreteClass, null);
            }
        }
        
        public static double GetEb(this ConcreteClass concreteClass)
        {
            switch (concreteClass)
            {
                case ConcreteClass.B10: return 1.94e5;
                case ConcreteClass.B15: return 2.45e5;
                case ConcreteClass.B20: return 2.80e5;
                case ConcreteClass.B25: return 3.06e5;
                case ConcreteClass.B30: return 3.31e5;
                case ConcreteClass.B35: return 3.52e5;
                case ConcreteClass.B40: return 3.67e5;
                case ConcreteClass.B45: return 3.77e5;
                case ConcreteClass.B50: return 3.87e5;
                case ConcreteClass.B55: return 3.98e5;
                case ConcreteClass.B60: return 4.03e5;
                default:
                    throw new ArgumentOutOfRangeException(nameof(concreteClass), concreteClass, null);
            }
        }
        
        public static double GetRbt(this ConcreteClass concreteClass)
        {
            switch (concreteClass)
            {
                case ConcreteClass.B10: return 5.71;
                case ConcreteClass.B15: return 7.65;
                case ConcreteClass.B20: return 9.18;
                case ConcreteClass.B25: return 10.71;
                case ConcreteClass.B30: return 11.73;
                case ConcreteClass.B35: return 13.26;
                case ConcreteClass.B40: return 14.28;
                case ConcreteClass.B45: return 15.30;
                case ConcreteClass.B50: return 16.32;
                case ConcreteClass.B55: return 17.34;
                case ConcreteClass.B60: return 18.35;
                case ConcreteClass.B70: return 19.37;
                case ConcreteClass.B80: return 21.41;
                case ConcreteClass.B90: return 21.92;
                case ConcreteClass.B100: return 22.43;
                default:
                    throw new ArgumentOutOfRangeException(nameof(concreteClass), concreteClass, null);
            }
        }
        
        public static double GetRs(this ReinforcementClass reinforcementClass)
        {
            switch (reinforcementClass)
            {
                case ReinforcementClass.A240: return 2141;
                case ReinforcementClass.A400: return 3569;
                case ReinforcementClass.A500: return 4436;
                case ReinforcementClass.A500SP: return 4589;
                case ReinforcementClass.A600SP: return 5303;
                case ReinforcementClass.AU500SP: return 4589;
                case ReinforcementClass.B500: return 4436;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reinforcementClass), reinforcementClass, null);
            }
        }
        
        public static double GetRsw(this ReinforcementClass reinforcementClass)
        {
            switch (reinforcementClass)
            {
                case ReinforcementClass.A240: return 1734;
                case ReinforcementClass.A400: return 2855;
                case ReinforcementClass.A500:
                case ReinforcementClass.A500SP:
                case ReinforcementClass.A600SP:
                case ReinforcementClass.AU500SP:
                case ReinforcementClass.B500: return 3059;
                case ReinforcementClass.A600:
                default:
                    throw new ArgumentOutOfRangeException(nameof(reinforcementClass), reinforcementClass, null);
            }
        }
        
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
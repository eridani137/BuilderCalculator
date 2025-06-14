﻿using System;
using System.Linq.Expressions;
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
                case ConcreteClass.B60:
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
                case ConcreteClass.B70:
                case ConcreteClass.B80:
                case ConcreteClass.B90:
                case ConcreteClass.B100:
                default:
                    throw new ArgumentOutOfRangeException(nameof(concreteClass), concreteClass, null);
            }
        }

        public static double GetRbt1(this ConcreteClass concreteClass)
        {
            switch (concreteClass)
            {
                case ConcreteClass.B10: return 0.056;
                case ConcreteClass.B15: return 0.075;
                case ConcreteClass.B20: return 0.090;
                case ConcreteClass.B25: return 0.105;
                case ConcreteClass.B30: return 0.115;
                case ConcreteClass.B35: return 0.130;
                case ConcreteClass.B40: return 0.140;
                case ConcreteClass.B45: return 0.150;
                case ConcreteClass.B50: return 0.160;
                case ConcreteClass.B55: return 0.170;
                case ConcreteClass.B60: return 0.180;
                case ConcreteClass.B70:
                case ConcreteClass.B80:
                case ConcreteClass.B90:
                case ConcreteClass.B100:
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

        public static double GetPhibcr(this ConcreteClass concreteClass)
        {
            switch (concreteClass)
            {
                case ConcreteClass.B10: return 3.9;
                case ConcreteClass.B15: return 3.4;
                case ConcreteClass.B20: return 2.8;
                case ConcreteClass.B25: return 2.5;
                case ConcreteClass.B30: return 2.3;
                case ConcreteClass.B35: return 2.1;
                case ConcreteClass.B40: return 1.9;
                case ConcreteClass.B45: return 1.8;
                case ConcreteClass.B50: return 1.6;
                case ConcreteClass.B55: return 1.5;
                case ConcreteClass.B60: return 1.4;
                case ConcreteClass.B70:
                case ConcreteClass.B80:
                case ConcreteClass.B90:
                case ConcreteClass.B100:
                default:
                    throw new ArgumentOutOfRangeException(nameof(concreteClass), concreteClass, null);
            }
        }

        public static double GetRbtser(this ConcreteClass concreteClass)
        {
            switch (concreteClass)
            {
                case ConcreteClass.B10: return 8.7;
                case ConcreteClass.B15: return 11.2;
                case ConcreteClass.B20: return 13.8;
                case ConcreteClass.B25: return 15.8;
                case ConcreteClass.B30: return 17.8;
                case ConcreteClass.B35: return 19.9;
                case ConcreteClass.B40: return 21.4;
                case ConcreteClass.B45: return 22.9;
                case ConcreteClass.B50: return 25.0;
                case ConcreteClass.B55: return 26.5;
                case ConcreteClass.B60: return 28.0;
                case ConcreteClass.B70:
                case ConcreteClass.B80:
                case ConcreteClass.B90:
                case ConcreteClass.B100:
                default:
                    throw new ArgumentOutOfRangeException(nameof(concreteClass), concreteClass, null);
            }
        }
        
        public static double GetRbser(this ConcreteClass concreteClass)
        {
            switch (concreteClass)
            {
                case ConcreteClass.B10: return 76.5;
                case ConcreteClass.B15: return 112.0;
                case ConcreteClass.B20: return 153.0;
                case ConcreteClass.B25: return 188.0;
                case ConcreteClass.B30: return 224.0;
                case ConcreteClass.B35: return 260.0;
                case ConcreteClass.B40: return 296.0;
                case ConcreteClass.B45: return 326.0;
                case ConcreteClass.B50: return 367.0;
                case ConcreteClass.B55: return 403.0;
                case ConcreteClass.B60: return 438.0;
                case ConcreteClass.B70:
                case ConcreteClass.B80:
                case ConcreteClass.B90:
                case ConcreteClass.B100:
                default:
                    throw new ArgumentOutOfRangeException(nameof(concreteClass), concreteClass, null);
            }   
        }

        public static double GetEs(this ReinforcementClass reinforcementClass)
        {
            switch (reinforcementClass)
            {
                case ReinforcementClass.A240:
                case ReinforcementClass.A400:
                case ReinforcementClass.A500:
                case ReinforcementClass.B500: return 2.04e6;
                case ReinforcementClass.A500SP:
                case ReinforcementClass.A600:
                case ReinforcementClass.A600SP:
                case ReinforcementClass.AU500SP:
                case ReinforcementClass.A800:
                case ReinforcementClass.A1000:
                default:
                    throw new ArgumentOutOfRangeException(nameof(reinforcementClass), reinforcementClass, null);
            }
        }
        
        public static double GetRs(this ReinforcementClass reinforcementClass)
        {
            switch (reinforcementClass)
            {
                case ReinforcementClass.A240: return 2190;
                case ReinforcementClass.A400: return 3620;
                case ReinforcementClass.A500: return 4434;
                case ReinforcementClass.A500SP: return 4589;
                case ReinforcementClass.A600SP: return 5303;
                case ReinforcementClass.AU500SP: return 4589;
                case ReinforcementClass.B500: return 4434;
                case ReinforcementClass.A600:
                case ReinforcementClass.A800:
                case ReinforcementClass.A1000:
                default:
                    throw new ArgumentOutOfRangeException(nameof(reinforcementClass), reinforcementClass, null);
            }
        }

        public static double GetRs1(this ReinforcementClass reinforcementClass)
        {
            switch (reinforcementClass)
            {
                case ReinforcementClass.A240: return 21.00;
                case ReinforcementClass.A400: return 35.00;
                case ReinforcementClass.A500: return 43.50;
                case ReinforcementClass.A500SP: return 45.00;
                case ReinforcementClass.A600:
                case ReinforcementClass.A600SP: return 52.00;
                case ReinforcementClass.AU500SP: return 45.00;
                case ReinforcementClass.B500: return 43.50;
                case ReinforcementClass.A800: return 69.50;
                case ReinforcementClass.A1000: return 87.00;
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
                case ReinforcementClass.A800:
                case ReinforcementClass.A1000:
                default:
                    throw new ArgumentOutOfRangeException(nameof(reinforcementClass), reinforcementClass, null);
            }
        }
        
        public static double GetRsw1(this ReinforcementClass reinforcementClass)
        {
            switch (reinforcementClass)
            {
                case ReinforcementClass.A240:
                case ReinforcementClass.A400:
                case ReinforcementClass.A500:
                case ReinforcementClass.A500SP:
                case ReinforcementClass.A600SP:
                case ReinforcementClass.AU500SP:
                case ReinforcementClass.B500:
                case ReinforcementClass.A600:
                case ReinforcementClass.A800:
                case ReinforcementClass.A1000:
                default:
                    throw new ArgumentOutOfRangeException(nameof(reinforcementClass), reinforcementClass, null);
            }
        }

        public static double GetRsc(this ReinforcementClass reinforcementClass)
        {
            switch (reinforcementClass)
            {
                case ReinforcementClass.A240: return 2190;
                case ReinforcementClass.A400: return 3620;
                case ReinforcementClass.A500: return 4080;
                case ReinforcementClass.A500SP: return 4589;
                case ReinforcementClass.A600SP: return 5303;
                case ReinforcementClass.AU500SP: return 4589;
                case ReinforcementClass.B500: return 4080;
                case ReinforcementClass.A600:
                case ReinforcementClass.A800:
                case ReinforcementClass.A1000:
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
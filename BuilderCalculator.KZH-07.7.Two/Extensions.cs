using System;
using Calculators.Shared.Enums;

namespace BuilderCalculator.KZH_07._7.Two
{
    public static class Extensions
    {
        /// <summary>
        /// Возвращает сопротивление растяжению, кг/см²
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double GetRbt(this ConcreteClass concreteClass,  double gammaBi)
        {
            switch (concreteClass)
            {
                case ConcreteClass.B10: return 5.71 * gammaBi;
                case ConcreteClass.B15: return 7.65 * gammaBi;
                case ConcreteClass.B20: return 9.18 * gammaBi;
                case ConcreteClass.B25: return 10.71 * gammaBi;
                case ConcreteClass.B30: return 11.73 * gammaBi;
                case ConcreteClass.B35: return 13.26 * gammaBi;
                case ConcreteClass.B40: return 14.28 * gammaBi;
                case ConcreteClass.B45: return 15.30 * gammaBi;
                case ConcreteClass.B50: return 16.32 * gammaBi;
                case ConcreteClass.B55: return 17.34 * gammaBi;
                case ConcreteClass.B60: return 18.35 * gammaBi;
                case ConcreteClass.B70: return 19.37 * gammaBi;
                case ConcreteClass.B80: return 21.41 * gammaBi;
                case ConcreteClass.B90: return 21.92 * gammaBi;
                case ConcreteClass.B100: return 22.43 * gammaBi;
                default:
                    throw new ArgumentOutOfRangeException(nameof(concreteClass), concreteClass, null);
            }
        }
        
        /// <summary>
        /// Возвращает модуль упругости арматуры, кг/см²
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
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
    }
}
using System;
using Calculators.Shared.Enums;

namespace Calculators.Shared.Extensions
{
    public static class ConcreteExtensions
    {
        /// <summary>
        /// Возвращает сопротивление растяжению, кг/см²
        /// KZH-04 - Rbt_ser
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double GetStretchResistance(this ConcreteClass concreteClass)
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
                default:
                    throw new ArgumentOutOfRangeException(nameof(concreteClass), concreteClass, null);
            }
        }

        /// <summary>
        /// Возвращает расчетное сопротивление сжатию, кг/см²
        /// KZH-04 - Rb_ser
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double GetCompressionResistance(this ConcreteClass concreteClass)
        {
            switch (concreteClass)
            {
                case ConcreteClass.B10: return 76.5;
                case ConcreteClass.B15: return 112.2;
                case ConcreteClass.B20: return 153.0;
                case ConcreteClass.B25: return 188.6;
                case ConcreteClass.B30: return 224.3;
                case ConcreteClass.B35: return 260.0;
                case ConcreteClass.B40: return 295.7;
                case ConcreteClass.B45: return 326.3;
                case ConcreteClass.B50: return 367.1;
                case ConcreteClass.B55: return 402.8;
                case ConcreteClass.B60: return 438.5;
                default:
                    throw new ArgumentOutOfRangeException(nameof(concreteClass), concreteClass, null);
            }
        }

        /// <summary>
        /// Возвращает модуль упругости бетона, кг/см²
        /// KZH-04 - Eb
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double GetElasticityModule(this ConcreteClass concreteClass)
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
    }
}
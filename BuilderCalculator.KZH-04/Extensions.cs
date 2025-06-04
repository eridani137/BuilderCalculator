using System;
using Calculators.Shared.Enums;

namespace Calculators.KZH_04
{
    public static class Extensions
    {
        /// <summary>
        /// Возвращает сопротивление растяжению, кг/см²
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double GetRbt_ser(this ConcreteClass concreteClass)
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
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double GetRb_ser(this ConcreteClass concreteClass)
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
        
        /// <summary>
        /// Возвращает модуль упругости арматуры, кг/см²
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double GetElasticityModule(this ReinforcementClass reinforcementClass)
        {
            switch (reinforcementClass)
            {
                case ReinforcementClass.A240:
                case ReinforcementClass.A400:
                case ReinforcementClass.A500:
                case ReinforcementClass.A500SP:
                case ReinforcementClass.A600:
                case ReinforcementClass.A600SP:
                case ReinforcementClass.AU500SP:
                case ReinforcementClass.B500: return 2.04e6;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reinforcementClass), reinforcementClass, null);
            }
        }
    }
}
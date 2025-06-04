using System;
using Calculators.Shared.Enums;

namespace Calculators.Shared.Extensions
{
    public static class ReinforcementExtensions
    {
        /// <summary>
        /// Возвращает модуль упругости арматуры, кг/см²
        /// KZH-04 - Es
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
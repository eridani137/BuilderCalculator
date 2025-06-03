using System;
using Calculators.Shared.Enums;

namespace Calculators.Shared.Extensions
{
    public static class ArmatureExtensions
    {
        /// <summary>
        /// Возвращает модуль упругости арматуры, кг/см²
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static double GetElasticityModule(this ArmatureClass armatureClass)
        {
            switch (armatureClass)
            {
                case ArmatureClass.A240:
                case ArmatureClass.A400:
                case ArmatureClass.A500:
                case ArmatureClass.A500SP:
                case ArmatureClass.A600:
                case ArmatureClass.A600SP:
                case ArmatureClass.AU500SP:
                case ArmatureClass.B500: return 2.04 * 1e6;
                default:
                    throw new ArgumentOutOfRangeException(nameof(armatureClass), armatureClass, null);
            }
        }
    }
}
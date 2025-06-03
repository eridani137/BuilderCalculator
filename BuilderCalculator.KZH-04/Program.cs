using System;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace Calculators.KZH_04
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            // var concreteClass = ConcreteClass.B15; // Класс бетона
            // var armatureClass = ArmatureClass.A400; // Класс арматуры
            //
            // var calc = new CheckingCrackAndOpeningWidth
            // {
            //     M = 27.70 * 1e5, // Момент, кг*см
            //     Ml = 25.00 * 1e5, // Длительный момент, кг*см
            //     N = 0, // Продольная сила (не используется)
            //     IncludeLongitudinalForce = false,
            //     b = 150.0, // Ширина, см
            //     h = 60.0, // Высота, см
            //     a = 5.0, // Защитный слой, см
            //     h0 = 55.0, // Рабочая высота, см
            //     a_prime = 5.0, // Защитный слой сжатой арматуры, см
            //     h0_prime = 55.0, // Рабочая высота сжатой арматуры, см
            //     As = 11.31, // Площадь растянутой арматуры, см²
            //     As_prime = 2.50, // Площадь сжатой арматуры, см²
            //     ds = 1.2, // Диаметр арматуры, см
            //     ConcreteClass = concreteClass,
            //     Rbt_ser = concreteClass.GetStretchResistance(), // Сопротивление растяжению, кг/см²
            //     Rb_ser = concreteClass.GetCompressionResistance(), // Сопротивление сжатию, кг/см²
            //     Eb = concreteClass.GetElasticityModule(), // Модуль упругости бетона, кг/см²
            //     ArmatureClass = armatureClass,
            //     Es = armatureClass.GetElasticityModule(), // Модуль упругости арматуры, кг/см²
            //     phi2 = 0.5, // Коэффициент phi2
            //     phi3 = 1, // Коэффициент phi3
            //     epsilon_b1_red = 0.0015, // Условная деформация
            //     a_crc_ult = 0.0400, // Предельная ширина раскрытия, см
            //     a_crc_ult_l = 0.0300 // Предельная ширина для длительных нагрузок, см
            // };

            var calc = new CheckingCrackAndOpeningWidth();
            
            calc.EnteringParameters();
            calc.Calculate();
            calc.PrintResults();
            
            Console.ReadKey(false);
        }
    }
}
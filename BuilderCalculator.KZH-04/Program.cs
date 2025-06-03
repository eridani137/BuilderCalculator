using System;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace Calculators.KZH_04
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var concreteClass = ConcreteClass.B15; // Класс бетона
            var armatureClass = ArmatureClass.A400; // Класс арматуры
            
            var calc = new CheckingCrackAndOpeningWidth
            {
                M = 27.70 * 1e5, // Момент, кг*см
                Ml = 25.00 * 1e5, // Длительный момент, кг*см
                N = 0, // Продольная сила (не используется)
                IncludeLongitudinalForce = false,
                b = 150.0, // Ширина, см
                h = 60.0, // Высота, см
                a = 5.0, // Защитный слой, см
                h0 = 55.0, // Рабочая высота, см
                a_prime = 5.0, // Защитный слой сжатой арматуры, см
                h0_prime = 55.0, // Рабочая высота сжатой арматуры, см
                As = 11.31, // Площадь растянутой арматуры, см²
                As_prime = 2.50, // Площадь сжатой арматуры, см²
                ds = 1.2, // Диаметр арматуры, см
                ConcreteClass = concreteClass,
                Rbt_ser = concreteClass.GetStretchResistance(), // Сопротивление растяжению, кг/см²
                Rb_ser = concreteClass.GetCompressionResistance(), // Сопротивление сжатию, кг/см²
                Eb = concreteClass.GetElasticityModule(), // Модуль упругости бетона, кг/см²
                ArmatureClass = armatureClass,
                Es = armatureClass.GetElasticityModule(), // Модуль упругости арматуры, кг/см²
                phi2 = 0.5, // Коэффициент phi2
                phi3 = 1, // Коэффициент phi3
                epsilon_b1_red = 0.0015, // Условная деформация
                a_crc_ult = 0.0400, // Предельная ширина раскрытия, см
                a_crc_ult_l = 0.0300 // Предельная ширина для длительных нагрузок, см
            };

            calc.Calculate();

            // Вывод результатов
            Console.WriteLine("Результаты расчета:");
            Console.WriteLine($"I_red: {calc.I_red:F2} см⁴");
            Console.WriteLine($"W_red: {calc.W_red:F2} см³");
            Console.WriteLine($"W_pl: {calc.W_pl:F2} см³");
            Console.WriteLine($"e_x: {calc.e_x:F2} см");
            Console.WriteLine($"M_crc: {calc.M_crc:F2} кг*см");
            Console.WriteLine($"Eb_red: {calc.Eb_red:F2} кг/см²");
            Console.WriteLine($"alpha_s1: {calc.alpha_s1:F2}");
            Console.WriteLine($"x_M: {calc.x_M:F2} см");
            Console.WriteLine($"x_m: {calc.x_m:F2} см");
            Console.WriteLine($"A_red: {calc.A_red:F2} см²");
            Console.WriteLine($"S_t_red: {calc.S_t_red:F2} см³");
            Console.WriteLine($"y_c: {calc.y_c:F2} см");
            Console.WriteLine($"I_red_cracked: {calc.I_red_cracked:F2} см⁴");
            Console.WriteLine($"x_t: {calc.x_t:F2} см");
            Console.WriteLine($"sigma_s: {calc.sigma_s:F2} кг/см²");
            Console.WriteLine($"sigma_s_crc: {calc.sigma_s_crc:F2} кг/см²");
            Console.WriteLine($"psi_s: {calc.psi_s:F2}");
            Console.WriteLine($"a_crc1: {calc.a_crc1:F4} см");
            Console.WriteLine($"a_crc2: {calc.a_crc2:F4} см");
            Console.WriteLine($"a_crc3: {calc.a_crc3:F4} см");
            Console.WriteLine($"Result: {(calc.Result ? "Прочность обеспечена" : "Прочность не обеспечена")}");

            Console.ReadKey(false);
        }
    }
}
using System;
using Calculators.Shared;
using Calculators.Shared.Enums;

namespace Calculators.KZH_04
{
    public class CheckingCrackAndOpeningWidth : IBuilderCalculator
    {
        // Внешние усилия
        
        /// <summary>
        /// Момент, кг*см
        /// </summary>
        public double M { get; set; }
        
        /// <summary>
        /// Длительный момент, кг*см
        /// </summary>
        public double Ml { get; set; }
        
        /// <summary>
        /// Продольная сила, кг (опционально)
        /// </summary>
        public double N { get; set; }
        
        /// <summary>
        /// Учет продольной силы
        /// </summary>
        public bool IncludeLongitudinalForce { get; set; }

        
        // Геометрические характеристики
        
        /// <summary>
        /// Ширина сечения, см
        /// </summary>
        public double b { get; set; }
        
        /// <summary>
        /// Высота сечения, см
        /// </summary>
        public double h { get; set; }
        
        /// <summary>
        /// Защитный слой до растянутой арматуры, см
        /// </summary>
        public double a { get; set; }
        
        /// <summary>
        /// Рабочая высота, см
        /// </summary>
        public double h0 { get; set; }
        
        /// <summary>
        /// Защитный слой до сжатой арматуры, см
        /// </summary>
        public double a_prime { get; set; }
        
        /// <summary>
        /// Рабочая высота для сжатой арматуры, см
        /// </summary>
        public double h0_prime { get; set; }

        
        // Характеристики арматуры
        
        /// <summary>
        /// Площадь растянутой арматуры, см²
        /// </summary>
        public double As { get; set; }
        
        /// <summary>
        /// Площадь сжатой арматуры, см²
        /// </summary>
        public double As_prime { get; set; }
        
        /// <summary>
        /// Диаметр арматуры, см
        /// </summary>
        public double ds { get; set; }
        
        /// <summary>
        /// Класс арматуры
        /// </summary>
        public ArmatureClass ArmatureClass { get; set; }

        
        // Характеристики бетона
        
        /// <summary>
        /// Класс бетона
        /// </summary>
        public ConcreteClass ConcreteClass { get; set; }
        
        /// <summary>
        /// Расчетное сопротивление растяжению, кг/см²
        /// </summary>
        public double Rbt_ser { get; set; }
        
        /// <summary>
        /// Расчетное сопротивление сжатию, кг/см²
        /// </summary>
        public double Rb_ser { get; set; }
        
        /// <summary>
        /// Модуль упругости бетона, кг/см²
        /// </summary>
        public double Eb { get; set; }
        
        /// <summary>
        /// Модуль упругости арматуры, кг/см²
        /// </summary>
        public double Es { get; set; }

        
        // Коэффициенты и предельные значения
        
        /// <summary>
        /// Коэффициент phi2
        /// </summary>
        public double phi2 { get; set; }
        
        /// <summary>
        /// Коэффициент phi3
        /// </summary>
        public double phi3 { get; set; }
        
        /// <summary>
        /// Условная деформация
        /// </summary>
        public double epsilon_b1_red { get; set; }
        
        /// <summary>
        /// Предельная ширина раскрытия трещин, см
        /// </summary>
        public double a_crc_ult { get; set; }
        
        /// <summary>
        /// Предельная ширина для длительных нагрузок, см
        /// </summary>
        public double a_crc_ult_l { get; set; }

        
        // Вычисляемые свойства
        
        /// <summary>
        /// Приведенный момент инерции, см⁴
        /// </summary>
        public double I_red { get; private set; }
        
        /// <summary>
        /// Приведенный момент сопротивления, см³
        /// </summary>
        public double W_red { get; private set; }
        
        /// <summary>
        /// Пластический момент сопротивления, см³
        /// </summary>
        public double W_pl { get; private set; }
        
        /// <summary>
        /// Эксцентриситет (не используется в примере) // TODO
        /// </summary>
        public double e_x { get; private set; }
        
        /// <summary>
        /// Момент образования трещин, кг*см
        /// </summary>
        public double M_crc { get; private set; }
        
        /// <summary>
        /// Приведенный модуль упругости бетона, кг/см²
        /// </summary>
        public double Eb_red { get; private set; }
        
        /// <summary>
        /// Отношение модулей упругости
        /// </summary>
        public double alpha_s1 { get; private set; }
        
        /// <summary>
        /// Высота сжатой зоны для M (не используется отдельно) // TODO
        /// </summary>
        public double x_M { get; private set; }
        
        /// <summary>
        /// Высота сжатой зоны, см
        /// </summary>
        public double x_m { get; private set; } 
        
        /// <summary>
        /// Приведенная площадь сечения, см²
        /// </summary>
        public double A_red { get; private set; }
        
        /// <summary>
        /// Приведенный статический момент, см³
        /// </summary>
        public double S_t_red { get; private set; }
        
        /// <summary>
        /// Положение нейтральной оси, см
        /// </summary>
        public double y_c { get; private set; }
        
        /// <summary>
        /// Приведенный момент инерции с трещинами, см⁴
        /// </summary>
        public double I_red_cracked { get; private set; }
        
        /// <summary>
        /// Высота растянутой зоны, см
        /// </summary>
        public double x_t { get; private set; }
        
        /// <summary>
        /// Напряжение в арматуре, кг/см²
        /// </summary>
        public double sigma_s { get; private set; }
        
        /// <summary>
        /// Напряжение в арматуре при трещинах (не используется отдельно) // TODO
        /// </summary>
        public double sigma_s_crc { get; private set; }
        
        /// <summary>
        /// Коэффициент учета трещин
        /// </summary>
        public double psi_s { get; private set; }
        
        /// <summary>
        /// Ширина раскрытия трещин (длительная), см
        /// </summary>
        public double a_crc1 { get; private set; }
        
        /// <summary>
        /// Ширина раскрытия трещин (полная), см
        /// </summary>
        public double a_crc2 { get; private set; }
        
        /// <summary>
        /// Ширина раскрытия трещин (непродолжительная), см
        /// </summary>
        public double a_crc3 { get; private set; }
        
        /// <summary>
        /// Результат: true - прочность обеспечена, false - нет
        /// </summary>
        public bool Result { get; private set; }
        
        
        public void Calculate()
        {
            // Шаг 1: Расчет момента образования трещин
            double alpha = Es / Eb; // Отношение модулей упругости
            double A = b * h; // Площадь сечения
            A_red = A + As * alpha + As_prime * alpha; // Приведенная площадь
            S_t_red = A * h / 2 + As * a * alpha + As_prime * h0_prime * alpha; // Приведенный статический момент
            double y_t = S_t_red / A_red; // Положение центра тяжести
            double I = (b * Math.Pow(h, 3)) / 12 + A * Math.Pow(h / 2 - y_t, 2); // Момент инерции бетона
            double I_s = As * Math.Pow(y_t - a, 2); // Момент инерции растянутой арматуры
            double I_s_prime = As_prime * Math.Pow(h0_prime - y_t, 2); // Момент инерции сжатой арматуры
            I_red = I + I_s * alpha + I_s_prime * alpha; // Приведенный момент инерции
            W_red = I_red / y_t; // Приведенный момент сопротивления
            W_pl = 1.3 * W_red; // Пластический момент сопротивления
            M_crc = Rbt_ser * W_pl; // Момент образования трещин

            // Проверка на образование трещин
            if (M > M_crc)
            {
                // Трещины образуются, расчет ширины раскрытия
                Eb_red = Rb_ser / epsilon_b1_red; // Приведенный модуль упругости бетона
                alpha_s1 = Es / Eb_red; // Отношение модулей для трещин
                double alpha_s2 = alpha_s1; // В данном случае alpha_s2 = alpha_s1
                double mu_s = As / (b * h0); // Коэффициент армирования растянутой зоны
                double mu_s_prime = As_prime / (b * h0); // Коэффициент армирования сжатой зоны
                x_m = h0 * (Math.Sqrt(Math.Pow(mu_s * alpha_s2 + mu_s_prime * alpha_s1, 2) +
                                      2 * (mu_s * alpha_s2 + mu_s_prime * alpha_s1 * a_prime / h0)) -
                            (mu_s * alpha_s2 + mu_s_prime * alpha_s1)); // Высота сжатой зоны
                y_c = x_m; // Положение нейтральной оси
                I_s = As * Math.Pow(h0 - y_c, 2); // Момент инерции растянутой арматуры
                I_s_prime = As_prime * Math.Pow(y_c - a_prime, 2); // Момент инерции сжатой арматуры
                double I_b = (b * Math.Pow(y_c, 3)) / 12 + y_c * b * Math.Pow(y_c / 2, 2); // Момент инерции бетона
                I_red_cracked = I_b + I_s * alpha_s2 + I_s_prime * alpha_s1; // Приведенный момент инерции с трещинами
                x_t = h - x_m; // Высота растянутой зоны
                if (x_t >= 0.5 * h) x_t = 0.5 * h; // Ограничение высоты растянутой зоны
                double A_t = b * x_t; // Площадь растянутой зоны
                double l_s = 0.5 * (A_t / As) * ds; // Длина зоны растяжения
                if (l_s >= 40 * ds) l_s = 40 * ds; // Ограничение по диаметру
                if (l_s >= 40) l_s = 40; // Ограничение длины

                // Расчет для длительного момента Ml
                sigma_s = (Ml * (h0 - y_c) / I_red_cracked) * alpha_s1; // Напряжение в арматуре
                psi_s = 1 - 0.8 * (M_crc / Ml); // Коэффициент учета трещин
                double phi1 = 1.4; // Коэффициент для длительных нагрузок
                a_crc1 = phi1 * phi2 * phi3 * psi_s * (sigma_s / Es) * l_s; // Ширина раскрытия (длительная)

                // Расчет для непродолжительного действия (Ml)
                phi1 = 1.0; // Коэффициент для непродолжительных нагрузок
                a_crc3 = phi1 * phi2 * phi3 * psi_s * (sigma_s / Es) * l_s; // Ширина раскрытия (непродолжительная)

                // Расчет для полного момента M
                sigma_s = (M * (h0 - y_c) / I_red_cracked) * alpha_s1; // Напряжение в арматуре
                psi_s = 1 - 0.8 * (M_crc / M); // Коэффициент учета трещин
                a_crc2 = phi1 * phi2 * phi3 * psi_s * (sigma_s / Es) * l_s; // Ширина раскрытия (полная)

                // Проверка условий прочности
                double a_crc = a_crc1 + a_crc2 - a_crc3; // Суммарная ширина раскрытия
                Result = a_crc1 <= a_crc_ult_l && a_crc <= a_crc_ult; // True - прочность обеспечена
            }
            else
            {
                // Трещины не образуются, Прочность обеспечена
                Result = true;
            }

            // Параметры, не используемые в данном примере // TODO
            e_x = 0; // Эксцентриситет не учитывается
            x_M = x_m; // Высота сжатой зоны для M совпадает с x_m
            sigma_s_crc = 0; // Не используется отдельно в данном расчете
        }
    }
}
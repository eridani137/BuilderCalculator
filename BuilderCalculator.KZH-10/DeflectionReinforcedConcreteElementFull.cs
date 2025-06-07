using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_10
{
    public class DeflectionReinforcedConcreteElementFull : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; }

        public DeflectionReinforcedConcreteElementFull()
        {
            CalculateResult = new CalculateResult(this);
        }

        public DeflectionReinforcedConcreteElementFull(double m, double ml, double fult, int humidity, int scheme, double l, int sectionType, double b, double bf, double h, double hf, double a, double @as, double aPrime, double asPrime, ConcreteClass concreteClass, ReinforcementClass reinforcementClass, bool calculateReducedModulus)
        {
            CalculateResult = new CalculateResult(this);
            M = m;
            Ml = ml;
            this.fult = fult;
            Humidity = humidity;
            Scheme = scheme;
            this.l = l;
            SectionType = sectionType;
            this.b = b;
            this.bf = bf;
            this.h = h;
            this.hf = hf;
            this.a = a;
            As = @as;
            a_prime = aPrime;
            As_prime = asPrime;
            ConcreteClass = concreteClass;
            ReinforcementClass = reinforcementClass;
            CalculateReducedModulus = calculateReducedModulus;
        }

        [InputParameter("Изгибающий момент от полной нагрузки, кг·см")]
        public double M { get; set; } = 1400000.0;

        [InputParameter("Изгибающий момент от длительной части нагрузки, кг·см")]
        public double Ml { get; set; } = 1100000.0;

        [InputParameter("Предельный прогиб, см")]
        public double fult { get; set; } = 4.0;

        [InputParameter("Влажность окружающей среды (1: >75%, 2: 40%-75%, 3: <40%)")]
        public int Humidity { get; set; } = 2;

        [InputParameter("Схема нагрузки (1-4)")]
        public int Scheme { get; set; } = 1;

        [InputParameter("Расчетный пролет балки, см")]
        public double l { get; set; } = 500.0;

        [InputParameter("Форма поперечного сечения (1: Прямоугольное, 2: Тавровое)")]
        public int SectionType { get; set; } = 2;

        [InputParameter("Ширина сечения, см")] public double b { get; set; } = 15.0;

        [InputParameter("Ширина полки тавра, см (только для Таврового сечения)")]
        public double bf { get; set; } = 30.0;

        [InputParameter("Высота сечения, см")] public double h { get; set; } = 45.0;

        [InputParameter("Высота полки тавра, см (только для Таврового сечения)")]
        public double hf { get; set; } = 15.0;

        [InputParameter("Расстояние от грани бетона до ц.т. арматуры в нижней зоне, см")]
        public double a { get; set; } = 4.5;

        [InputParameter("Площадь растянутой арматуры, см²")]
        public double As { get; set; } = 12.32;

        [InputParameter("Расстояние от грани бетона до ц.т. арматуры в верхней зоне, см")]
        public double a_prime { get; set; } = 3.5;

        [InputParameter("Площадь сжатой арматуры, см²")]
        public double As_prime { get; set; } = 5.5;

        [InputParameter("Класс бетона")] public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B25;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A500;

        [InputParameter("Вычислять приведенный модуль упругости")]
        public bool CalculateReducedModulus { get; set; } = true;

        public override BaseCalculateResult Calculate()
        {
            double Rb_ser = ConcreteClass.GetRbser();
            double Rbt_ser = ConcreteClass.GetRbtser();
            double Eb = ConcreteClass.GetEb();
            double phi_b_cr = ConcreteClass.GetPhibcr();
            double Es = ReinforcementClass.GetEs();

            // 2. Вычисление alpha
            double alpha = Es / Eb;

            // 3. Вычисление площади сечения A
            double A;
            if (SectionType == 1) // Прямоугольное
            {
                A = b * h;
            }
            else // Тавровое
            {
                A = b * (h - hf) + bf * hf;
            }

            // 4. Вычисление приведенной площади A_red
            double A_red = A + As * alpha + As_prime * alpha;

            // 5. Вычисление статического момента S_t_red
            double S_t_red, h0_prime = h - a_prime;
            if (SectionType == 1) // Прямоугольное
            {
                S_t_red = A * h / 2 + As * a * alpha + As_prime * h0_prime * alpha;
            }
            else // Тавровое
            {
                double S_t = b * Math.Pow(h - hf, 2) / 2 + bf * hf * (h - hf / 2);
                S_t_red = S_t + As * a * alpha + As_prime * h0_prime * alpha;
            }

            // 6. Вычисление положения центра тяжести y_t
            double y_t = S_t_red / A_red;

            // 7. Вычисление момента инерции I
            if (SectionType == 1) // Прямоугольное
            {
                CalculateResult.I = b * Math.Pow(h, 3) / 12 + A * Math.Pow(h / 2 - y_t, 2);
            }
            else // Тавровое
            {
                CalculateResult.I = b * Math.Pow(h - hf, 3) / 12 + b * (h - hf) * Math.Pow(y_t - (h - hf) / 2, 2) +
                                    bf * Math.Pow(hf, 3) / 12 + bf * hf * Math.Pow(h - y_t - hf / 2, 2);
            }

            // 8. Вычисление моментов инерции арматуры
            CalculateResult.Is = As * Math.Pow(y_t - a, 2);
            CalculateResult.Is_prime = As_prime * Math.Pow(h0_prime - y_t, 2);

            // 9. Вычисление приведенного момента инерции I_red
            CalculateResult.Ired = CalculateResult.I + CalculateResult.Is * alpha + CalculateResult.Is_prime * alpha;

            // 10. Вычисление момента сопротивления
            CalculateResult.Wred = CalculateResult.Ired / y_t;
            double W_pl = 1.3 * CalculateResult.Wred;

            // 11. Вычисление момента образования трещин
            CalculateResult.Mcrc = Rbt_ser * W_pl;

            // 12. Проверка условия образования трещин
            if (M > CalculateResult.Mcrc)
            {
                // Расчет для сечения с трещинами
                // 13. Установка epsilon_b1_red для полной нагрузки
                CalculateResult.epsilon_b1_red = 0.0015;

                // 14. Вычисление psi_s для полной нагрузки
                CalculateResult.psi_s = 1 - 0.8 * CalculateResult.Mcrc / M;

                // 15. Вычисление приведенных модулей упругости
                CalculateResult.Eb_red = Rb_ser / CalculateResult.epsilon_b1_red;
                CalculateResult.Es_red = Es / CalculateResult.psi_s;

                // 16. Установка Eb1
                CalculateResult.Eb1 = CalculateResult.Eb_red;

                // 17. Вычисление коэффициентов alpha_s1 и alpha_s2
                CalculateResult.alpha_s1 = Es / CalculateResult.Eb_red;
                CalculateResult.alpha_s2 = CalculateResult.Es_red / CalculateResult.Eb_red;

                // 18. Вычисление относительных площадей арматуры
                double h0 = h - a;
                CalculateResult.mu_s = As / (b * h0);
                CalculateResult.mu_s_prime = As_prime / (b * h0);

                // 19. Вычисление высоты сжатой зоны xm для полной нагрузки
                double z_full;
                if (SectionType == 1) // Прямоугольное
                {
                    z_full = Math.Sqrt(
                        Math.Pow(
                            CalculateResult.mu_s * CalculateResult.alpha_s2 +
                            CalculateResult.mu_s_prime * CalculateResult.alpha_s1, 2) +
                        2 * (CalculateResult.mu_s * CalculateResult.alpha_s2 +
                             CalculateResult.mu_s_prime * CalculateResult.alpha_s1 * a_prime / h0));
                    CalculateResult.xm = h0 * (z_full - (CalculateResult.mu_s * CalculateResult.alpha_s2 +
                                                         CalculateResult.mu_s_prime * CalculateResult.alpha_s1));
                }
                else // Тавровое
                {
                    double mu_f_prime = (bf - b) * hf / (b * h0);
                    z_full = CalculateResult.mu_s * CalculateResult.alpha_s2 +
                             CalculateResult.mu_s_prime * CalculateResult.alpha_s1 + mu_f_prime;
                    CalculateResult.xm = h0 * (Math.Sqrt(z_full * z_full + 2 *
                        (CalculateResult.mu_s * CalculateResult.alpha_s2 +
                         CalculateResult.mu_s_prime * CalculateResult.alpha_s1 * a_prime / h0 +
                         mu_f_prime * hf / 2 / h0)) - z_full);
                }

                // 20. Вычисление Ib для полной нагрузки
                if (SectionType == 1 || CalculateResult.xm <= hf) // Прямоугольное или Тавровое с xm <= hf
                {
                    CalculateResult.Ib = b * Math.Pow(CalculateResult.xm, 3) / 3;
                }
                else // Тавровое с xm > hf
                {
                    CalculateResult.Ib = bf * Math.Pow(hf, 3) / 12 +
                                         bf * hf * Math.Pow(CalculateResult.xm - 0.5 * hf, 2) +
                                         b * Math.Pow(CalculateResult.xm - hf, 3) / 3;
                }

                // 21. Вычисление Is и Is_prime для полной нагрузки
                CalculateResult.Is = As * Math.Pow(h0 - CalculateResult.xm, 2);
                CalculateResult.Is_prime = As_prime * Math.Pow(CalculateResult.xm - a_prime, 2);

                // 22. Вычисление Ired для полной нагрузки
                CalculateResult.Ired = CalculateResult.Ib + CalculateResult.Is * CalculateResult.alpha_s2 +
                                       CalculateResult.Is_prime * CalculateResult.alpha_s1;

                // 23. Вычисление D1 и (1/r)1
                CalculateResult.D1 = CalculateResult.Eb1 * CalculateResult.Ired;
                CalculateResult.one_over_r1 = M / CalculateResult.D1;

                // 24. Вычисление psi_s для длительной нагрузки
                CalculateResult.psi_s = 1 - 0.8 * CalculateResult.Mcrc / Ml;

                // 25. Вычисление Es_red для длительной нагрузки
                CalculateResult.Es_red = Es / CalculateResult.psi_s;

                // 26. Вычисление alpha_s2 для длительной нагрузки
                CalculateResult.alpha_s2 = CalculateResult.Es_red / CalculateResult.Eb_red;

                // 27. Вычисление xm для длительной нагрузки
                double z_long;
                if (SectionType == 1) // Прямоугольное
                {
                    z_long = Math.Sqrt(
                        Math.Pow(
                            CalculateResult.mu_s * CalculateResult.alpha_s2 +
                            CalculateResult.mu_s_prime * CalculateResult.alpha_s1, 2) +
                        2 * (CalculateResult.mu_s * CalculateResult.alpha_s2 +
                             CalculateResult.mu_s_prime * CalculateResult.alpha_s1 * a_prime / h0));
                    CalculateResult.xm = h0 * (z_long - (CalculateResult.mu_s * CalculateResult.alpha_s2 +
                                                         CalculateResult.mu_s_prime * CalculateResult.alpha_s1));
                }
                else // Тавровое
                {
                    double mu_f_prime = (bf - b) * hf / (b * h0);
                    z_long = CalculateResult.mu_s * CalculateResult.alpha_s2 +
                             CalculateResult.mu_s_prime * CalculateResult.alpha_s1 + mu_f_prime;
                    CalculateResult.xm = h0 * (Math.Sqrt(z_long * z_long + 2 *
                        (CalculateResult.mu_s * CalculateResult.alpha_s2 +
                         CalculateResult.mu_s_prime * CalculateResult.alpha_s1 * a_prime / h0 +
                         mu_f_prime * hf / 2 / h0)) - z_long);
                }

                // 28. Вычисление Ib для длительной нагрузки
                if (SectionType == 1 || CalculateResult.xm <= hf) // Прямоугольное или Тавровое с xm <= hf
                {
                    CalculateResult.Ib = b * Math.Pow(CalculateResult.xm, 3) / 3;
                }
                else // Тавровое с xm > hf
                {
                    CalculateResult.Ib = bf * Math.Pow(hf, 3) / 12 +
                                         bf * hf * Math.Pow(CalculateResult.xm - 0.5 * hf, 2) +
                                         b * Math.Pow(CalculateResult.xm - hf, 3) / 3;
                }

                // 29. Вычисление Is и Is_prime для длительной нагрузки
                CalculateResult.Is = As * Math.Pow(h0 - CalculateResult.xm, 2);
                CalculateResult.Is_prime = As_prime * Math.Pow(CalculateResult.xm - a_prime, 2);

                // 30. Вычисление Ired для длительной нагрузки
                CalculateResult.Ired = CalculateResult.Ib + CalculateResult.Is * CalculateResult.alpha_s2 +
                                       CalculateResult.Is_prime * CalculateResult.alpha_s1;

                // 31. Вычисление D2 и (1/r)2
                CalculateResult.D2 = CalculateResult.Eb1 * CalculateResult.Ired;
                CalculateResult.one_over_r2 = Ml / CalculateResult.D2;

                // 32. Установка epsilon_b1_red для ползучести
                CalculateResult.epsilon_b1_red = 0.0028;

                // 33. Вычисление Eb_red для ползучести
                CalculateResult.Eb_red = Rb_ser / CalculateResult.epsilon_b1_red;

                // 34. Установка Eb1 для ползучести
                CalculateResult.Eb1 = CalculateResult.Eb_red;

                // 35. Вычисление alpha_s1 и alpha_s2 для ползучести
                CalculateResult.alpha_s1 = Es / CalculateResult.Eb_red;
                CalculateResult.alpha_s2 = CalculateResult.Es_red / CalculateResult.Eb_red;

                // 36. Вычисление xm для ползучести
                double z_creep;
                if (SectionType == 1) // Прямоугольное
                {
                    z_creep = Math.Sqrt(
                        Math.Pow(
                            CalculateResult.mu_s * CalculateResult.alpha_s2 +
                            CalculateResult.mu_s_prime * CalculateResult.alpha_s1, 2) +
                        2 * (CalculateResult.mu_s * CalculateResult.alpha_s2 +
                             CalculateResult.mu_s_prime * CalculateResult.alpha_s1 * a_prime / h0));
                    CalculateResult.xm = h0 * (z_creep - (CalculateResult.mu_s * CalculateResult.alpha_s2 +
                                                          CalculateResult.mu_s_prime * CalculateResult.alpha_s1));
                }
                else // Тавровое
                {
                    double mu_f_prime = (bf - b) * hf / (b * h0);
                    z_creep = CalculateResult.mu_s * CalculateResult.alpha_s2 +
                              CalculateResult.mu_s_prime * CalculateResult.alpha_s1 + mu_f_prime;
                    CalculateResult.xm = h0 * (Math.Sqrt(z_creep * z_creep +
                                                         2 * (CalculateResult.mu_s * CalculateResult.alpha_s2 +
                                                              CalculateResult.mu_s_prime * CalculateResult.alpha_s1 *
                                                              a_prime / h0 +
                                                              mu_f_prime * hf / 2 / h0)) - z_creep);
                }

                // 37. Вычисление Ib для ползучести
                if (SectionType == 1 || CalculateResult.xm <= hf) // Прямоугольное или Тавровое с xm <= hf
                {
                    CalculateResult.Ib = b * Math.Pow(CalculateResult.xm, 3) / 3;
                }
                else // Тавровое с xm > hf
                {
                    CalculateResult.Ib = bf * Math.Pow(hf, 3) / 12 +
                                         bf * hf * Math.Pow(CalculateResult.xm - 0.5 * hf, 2) +
                                         b * Math.Pow(CalculateResult.xm - hf, 3) / 3;
                }

                // 38. Вычисление Is и Is_prime для ползучести
                CalculateResult.Is = As * Math.Pow(h0 - CalculateResult.xm, 2);
                CalculateResult.Is_prime = As_prime * Math.Pow(CalculateResult.xm - a_prime, 2);

                // 39. Вычисление Ired для ползучести
                CalculateResult.Ired = CalculateResult.Ib + CalculateResult.Is * CalculateResult.alpha_s2 +
                                       CalculateResult.Is_prime * CalculateResult.alpha_s1;

                // 40. Вычисление D3 и (1/r)3
                CalculateResult.D3 = CalculateResult.Eb1 * CalculateResult.Ired;
                CalculateResult.one_over_r3 = Ml / CalculateResult.D3;

                // 41. Вычисление полной кривизны
                CalculateResult.one_over_r = CalculateResult.one_over_r1 - CalculateResult.one_over_r2 +
                                             CalculateResult.one_over_r3;

                // 42. Вычисление прогиба f
                double s;
                switch (Scheme)
                {
                    case 1:
                        s = 5.0 / 48.0;
                        break;
                    case 2:
                        s = 1.0 / 8.0;
                        break;
                    case 3:
                        s = 1.0 / 12.0;
                        break;
                    case 4:
                        s = 1.0 / 3.0;
                        break;
                    default:
                        s = 5.0 / 48.0;
                        break;
                }

                CalculateResult.f = s * Math.Pow(l, 2) * CalculateResult.one_over_r;

                // 43. Проверка условия прочности
                CalculateResult.Result = CalculateResult.f <= fult;
            }

            return CalculateResult;
        }
    }
}
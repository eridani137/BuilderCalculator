using System;
using Calculators.Shared;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace Calculators.KZH_04
{
    public class Calculator : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; set; }
        
        public Calculator()
        {
            CalculateResult = new CalculateResult(this);
        }

        public Calculator(
            double M,
            double Ml,
            double N,
            double Nl,
            bool ConsiderAxialForce,
            double b,
            double h,
            double a,
            double a_prime,
            double h0,
            double h0_prime,
            double As,
            double As_prime,
            double ds,
            ConcreteClass ConcreteClass,
            ReinforcementClass ReinforcementClass,
            double acrc_ult,
            double acrc_ult_l)
        {
            M = M;
            Ml = Ml;
            N = N;
            Nl = Nl;
            ConsiderAxialForce = ConsiderAxialForce;
            b = b;
            h = h;
            a = a;
            a_prime = a_prime;
            h0 = h0;
            h0_prime = h0_prime;
            As = As;
            As_prime = As_prime;
            ds = ds;
            ConcreteClass = ConcreteClass;
            ReinforcementClass = ReinforcementClass;
            acrc_ult = acrc_ult;
            acrc_ult_l = acrc_ult_l;
        }

        [InputParameter("Момент от полной нагрузки, кг·см")]
        private double M { get; set; } = 27.70e5;

        [InputParameter("Момент от постоянной и длительной нагрузки, кг·см")]
        private double Ml { get; set; } = 25.00e5;
        
        [InputParameter("Учет продольной силы")] 
        private bool ConsiderAxialForce { get; set; } = true;

        [InputParameter("Продольная сила от полной нагрузки, кг (если учитывается)")]
        private double N { get; set; } = 20.00e3;

        [InputParameter("Продольная сила от постоянной и длительной нагрузки, кг (если учитывается)")]
        private double Nl { get; set; } = 18.00e3;

        [InputParameter("Ширина сечения, см")] 
        private double b { get; set; } = 150.0;

        [InputParameter("Высота сечения, см")] 
        private double h { get; set; } = 60.0;

        [InputParameter("Защитный слой бетона растянутой зоны, см")]
        private double a { get; set; } = 5.0;

        [InputParameter("Защитный слой бетона сжатой зоны, см")]
        private double a_prime { get; set; } = 5.0;

        [InputParameter("Площадь растянутой арматуры, см2")]
        private double As { get; set; } = 11.31;

        [InputParameter("Площадь сжатой арматуры, см2")]
        private double As_prime { get; set; } = 2.50;

        [InputParameter("Диаметр арматуры, см")] 
        private double ds { get; set; } = 1.2;
        
        [InputParameter("Класс бетона")] 
        private ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B15;

        [InputParameter("Класс арматуры")]
        private ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A500;
        
        [InputParameter("Предельно допустимая ширина продолжительного раскрытия трещин, см")]
        public double acrc_ult_l { get; set; } = 0.0300;
        
        [InputParameter("Предельно допустимая ширина непродолжительного раскрытия трещин, см")]
        public double acrc_ult { get; set; } = 0.0400;
        
        
        [InputParameter("Рабочая высота сечения, см")]
        private double h0 { get; set; } = 55.0;

        [InputParameter("Расстояние до центра сжатой арматуры от сжатой грани, см")]
        private double h0_prime { get; set; } = 55.0;

        private double phi2 { get; set; } = 0.5;

        private double phi3 { get; set; } = 1.0;

        private double epsilon_b1_red { get; set; } = 0.0015;


        public override BaseCalculateResult Calculate()
        {
            try
            {
                CalculateCrackingMoment();

                if (M > CalculateResult.Mcrc)
                {
                    CalculateCrackWidth();
                    CheckConditions();
                }
                else
                {
                    CalculateResult.Result = true; // Трещины не образуются
                }

                return CalculateResult;
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
                return null;
            }
        }

        /// <summary>
        /// Расчет момента образования трещин
        /// </summary>
        private void CalculateCrackingMoment()
        {
            double Es = ReinforcementClass.GetElasticityModule();

            double alpha = Es / ConcreteClass.GetElasticityModule();
            double A = b * h;

            double A_red_temp = A + As * alpha + As_prime * alpha;
            double St_red_temp = A * h / 2 + As * a * alpha + As_prime * h0_prime * alpha;
            double yt = St_red_temp / A_red_temp;

            double I_concrete = b * Math.Pow(h, 3) / 12 + A * Math.Pow(h / 2 - yt, 2);
            double Is = As * Math.Pow(yt - a, 2);
            double Is_prime = As_prime * Math.Pow(h0_prime - yt, 2);

            CalculateResult.I_red = I_concrete + Is * alpha + Is_prime * alpha;
            CalculateResult.W_red = CalculateResult.I_red / yt;
            CalculateResult.W_pl = 1.3 * CalculateResult.W_red;

            if (ConsiderAxialForce)
            {
                CalculateResult.ex = CalculateResult.W_red / A_red_temp;
                CalculateResult.Mcrc =
                    ConcreteClass.GetStretchResistance() * CalculateResult.W_pl + N * CalculateResult.ex;
            }
            else
            {
                CalculateResult.Mcrc = ConcreteClass.GetStretchResistance() * CalculateResult.W_pl;
            }
        }

        /// <summary>
        /// Расчет ширины раскрытия трещин
        /// </summary>
        private void CalculateCrackWidth()
        {
            double Es = ReinforcementClass.GetElasticityModule();

            CalculateResult.Eb_red = ConcreteClass.GetCompressionResistance() / epsilon_b1_red;
            CalculateResult.alpha_s1 = Es / CalculateResult.Eb_red;
            double alpha_s2 = CalculateResult.alpha_s1;

            double mu_s = As / (b * h0);
            double mu_s_prime = As_prime / (b * h0);

            // Расчет высоты сжатой зоны - ВСЕГДА вычисляем xM
            CalculateResult.xM = h0 * (Math.Sqrt(Math.Pow(mu_s * alpha_s2 + mu_s_prime * CalculateResult.alpha_s1, 2) +
                                                 2 * (mu_s * alpha_s2 + mu_s_prime * CalculateResult.alpha_s1 *
                                                     a_prime / h0)) -
                                       (mu_s * alpha_s2 + mu_s_prime * CalculateResult.alpha_s1));

            if (!ConsiderAxialForce)
            {
                // Для изгибаемого элемента
                CalculateResult.yc = CalculateResult.xM;
                CalculateResult.xm = CalculateResult.xM;
            }
            else
            {
                // Для сжато-изгибаемого элемента - более сложный расчет
                CalculateCompressedFlexuralParameters();
            }

            // Расчет геометрических характеристик сечения с трещинами
            CalculateCrackedSectionProperties();

            // Расчет напряжений и ширины раскрытия трещин
            CalculateStressesAndCrackWidths();
        }

        /// <summary>
        /// Расчет параметров для сжато-изгибаемого элемента
        /// </summary>
        private void CalculateCompressedFlexuralParameters()
        {
            // Проверяем, что Ml не равно нулю, чтобы избежать деления на ноль
            if (Math.Abs(Ml) < 1e-10)
            {
                CalculateResult.xm = CalculateResult.xM;
                CalculateResult.yc = CalculateResult.xM;
                return;
            }

            // Итеративный расчет для сжато-изгибаемого элемента
            double xm_prev = CalculateResult.xM;
            double tolerance = 0.001;
            int maxIterations = 100;
            int iteration = 0;

            do
            {
                double Ab = xm_prev * b;
                double A_red_temp = Ab + CalculateResult.alpha_s1 * As_prime + CalculateResult.alpha_s1 * As;
                double St_red_temp = Ab * (h - 0.5 * xm_prev) + As_prime * h0_prime * CalculateResult.alpha_s1 +
                                     As * a * CalculateResult.alpha_s1;
                double yc_temp = h - St_red_temp / A_red_temp;

                // Вычисляем момент инерции для текущего приближения
                double Is = As * Math.Pow(h0 - yc_temp, 2);
                double Is_prime = As_prime * Math.Pow(yc_temp - a_prime, 2);
                double Ib = b * Math.Pow(yc_temp, 3) / 3;
                double I_red_temp = Ib + Is * CalculateResult.alpha_s1 + Is_prime * CalculateResult.alpha_s1;

                // Новое значение xm
                double xm_new = CalculateResult.xM + (I_red_temp * Nl) / (A_red_temp * Ml);

                if (Math.Abs(xm_new - xm_prev) < tolerance)
                {
                    CalculateResult.xm = xm_new;
                    CalculateResult.A_red = A_red_temp;
                    CalculateResult.St_red = St_red_temp;
                    CalculateResult.yc = yc_temp;
                    break;
                }

                xm_prev = xm_new;
                iteration++;
            } while (iteration < maxIterations);

            // Если итерации не сошлись, используем последнее значение
            if (iteration >= maxIterations)
            {
                CalculateResult.xm = xm_prev;
                double Ab = CalculateResult.xm * b;
                CalculateResult.A_red = Ab + CalculateResult.alpha_s1 * As_prime + CalculateResult.alpha_s1 * As;
                CalculateResult.St_red = Ab * (h - 0.5 * CalculateResult.xm) +
                                         As_prime * h0_prime * CalculateResult.alpha_s1 +
                                         As * a * CalculateResult.alpha_s1;
                CalculateResult.yc = h - CalculateResult.St_red / CalculateResult.A_red;
            }
        }

        /// <summary>
        /// Расчет характеристик сечения с трещинами
        /// </summary>
        private void CalculateCrackedSectionProperties()
        {
            double Is = As * Math.Pow(h0 - CalculateResult.yc, 2);
            double Is_prime = As_prime * Math.Pow(CalculateResult.yc - a_prime, 2);
            double Ib = b * Math.Pow(CalculateResult.yc, 3) / 3;

            if (!ConsiderAxialForce)
            {
                // Для изгибаемого элемента
                Ib = b * Math.Pow(CalculateResult.yc, 3) / 12 +
                     CalculateResult.yc * b * Math.Pow(CalculateResult.yc / 2, 2);
            }

            CalculateResult.I_red = Ib + Is * CalculateResult.alpha_s1 + Is_prime * CalculateResult.alpha_s1;

            CalculateResult.xt = h - CalculateResult.xm;
            if (CalculateResult.xt >= 0.5 * h)
            {
                CalculateResult.xt = 0.5 * h;
            }
        }

        /// <summary>
        /// Расчет напряжений и ширины раскрытия трещин
        /// </summary>
        private void CalculateStressesAndCrackWidths()
        {
            double At = b * CalculateResult.xt;
            double ls = 0.5 * (At / As) * ds;

            // Ограничения на ls
            if (ls >= 40 * ds)
            {
                ls = 40 * ds;
            }

            if (ls >= 40)
            {
                ls = 40;
            }

            // Расчет напряжений
            if (ConsiderAxialForce)
            {
                CalculateResult.sigma_s =
                    (Ml * (h0 - CalculateResult.yc) / CalculateResult.I_red - Nl / CalculateResult.A_red) *
                    CalculateResult.alpha_s1;
                CalculateResult.sigma_s_crc =
                    (CalculateResult.Mcrc * (h0 - CalculateResult.yc) / CalculateResult.I_red -
                     Nl / CalculateResult.A_red) * CalculateResult.alpha_s1;
            }
            else
            {
                CalculateResult.sigma_s =
                    (Ml * (h0 - CalculateResult.yc) / CalculateResult.I_red) * CalculateResult.alpha_s1;
                CalculateResult.sigma_s_crc =
                    (CalculateResult.Mcrc * (h0 - CalculateResult.yc) / CalculateResult.I_red) *
                    CalculateResult.alpha_s1;
            }

            // Коэффициент psi_s
            if (!ConsiderAxialForce)
            {
                CalculateResult.psi_s = 1 - 0.8 * CalculateResult.Mcrc / Ml;
            }
            else
            {
                CalculateResult.psi_s = 1 - 0.8 * CalculateResult.sigma_s_crc / CalculateResult.sigma_s;
            }

            double Es = ReinforcementClass.GetElasticityModule();

            // Ширина раскрытия трещин от длительных нагрузок
            CalculateResult.acrc1 = 1.4 * phi2 * phi3 * CalculateResult.psi_s * CalculateResult.sigma_s / Es * ls;

            // Ширина раскрытия трещин от кратковременных длительных нагрузок
            CalculateResult.acrc3 = 1.0 * phi2 * phi3 * CalculateResult.psi_s * CalculateResult.sigma_s / Es * ls;

            // Расчет для полных нагрузок
            double sigma_s_full;
            double psi_s_full;

            if (ConsiderAxialForce)
            {
                sigma_s_full = (M * (h0 - CalculateResult.yc) / CalculateResult.I_red - N / CalculateResult.A_red) *
                               CalculateResult.alpha_s1;
                psi_s_full = 1 - 0.8 * CalculateResult.sigma_s_crc / sigma_s_full;
            }
            else
            {
                sigma_s_full = (M * (h0 - CalculateResult.yc) / CalculateResult.I_red) * CalculateResult.alpha_s1;
                psi_s_full = 1 - 0.8 * CalculateResult.Mcrc / M;
            }

            CalculateResult.acrc2 = 1.0 * phi2 * phi3 * psi_s_full * sigma_s_full / Es * ls;
        }

        /// <summary>
        /// Проверка условий
        /// </summary>
        private void CheckConditions()
        {
            bool condition1 = CalculateResult.acrc1 <= acrc_ult_l;
            double acrc_total = CalculateResult.acrc1 + CalculateResult.acrc2 - CalculateResult.acrc3;
            bool condition2 = acrc_total <= acrc_ult;

            CalculateResult.Result = condition1 && condition2;
        }

        /// <summary>
        /// Проверка образования трещин
        /// </summary>
        public bool IsCrackingOccurred()
        {
            return M > CalculateResult.Mcrc;
        }
    }
}
using System;
using Calculators.Shared;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
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

        public Calculator(double m, double ml, bool considerAxialForce, double n,
            double nl, double b, double h, double a, double aPrime, double @as, double asPrime, double ds,
            ConcreteClass concreteClass, ReinforcementClass reinforcementClass, double acrcUltL, double acrcUlt,
            double h0, double h0Prime, double phi2, double phi3, double epsilonB1Red)
        {
            CalculateResult = new CalculateResult(this);
            M = m;
            Ml = ml;
            ConsiderAxialForce = considerAxialForce;
            N = n;
            Nl = nl;
            this.b = b;
            this.h = h;
            this.a = a;
            a_prime = aPrime;
            As = @as;
            As_prime = asPrime;
            this.ds = ds;
            ConcreteClass = concreteClass;
            ReinforcementClass = reinforcementClass;
            acrc_ult_l = acrcUltL;
            acrc_ult = acrcUlt;
            this.h0 = h0;
            h0_prime = h0Prime;
            this.phi2 = phi2;
            this.phi3 = phi3;
            epsilon_b1_red = epsilonB1Red;
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

        /// <summary>
        /// Расчет момента образования трещин
        /// </summary>
        private void CalculateCrackingMoment()
        {
            double Es = ReinforcementClass.GetElasticityModule();
            double Eb = ConcreteClass.GetElasticityModule();
            double alpha = Es / Eb;
            double A = b * h;
            double A_red_temp = A + As * alpha + As_prime * alpha;
            double St_red_temp = A * h / 2 + As * a * alpha + As_prime * h0_prime * alpha;
            double yt = St_red_temp / A_red_temp;
            double I_concrete = b * Math.Pow(h, 3) / 12 + A * Math.Pow(h / 2 - yt, 2);
            double Is = As * Math.Pow(yt - a, 2);
            double Is_prime = As_prime * Math.Pow(h0_prime - yt, 2);
            double I_red = I_concrete + Is * alpha + Is_prime * alpha;
            double W_red = I_red / yt;
            double W_pl = 1.3 * W_red;

            CalculateResult.A_red_elastic = A_red_temp;
            CalculateResult.I_red_elastic = I_red;

            if (ConsiderAxialForce)
            {
                double ex = W_red / A_red_temp;
                CalculateResult.Mcrc = ConcreteClass.GetRbt_ser() * W_pl + N * ex;
            }
            else
            {
                CalculateResult.Mcrc = ConcreteClass.GetRbt_ser() * W_pl;
            }
        }

        /// <summary>
        /// Расчет ширины раскрытия трещин
        /// </summary>
        private void CalculateCrackWidth()
        {
            double Es = ReinforcementClass.GetElasticityModule();
            CalculateResult.Eb_red = ConcreteClass.GetRb_ser() / epsilon_b1_red;
            CalculateResult.alpha_s1 = Es / CalculateResult.Eb_red;
            double alpha_s2 = CalculateResult.alpha_s1;
            double mu_s = As / (b * h0);
            double mu_s_prime = As_prime / (b * h0);

            // Проверка условия (8.127) СП 63.13330.2012
            bool considerCompressedReinforcement = 
                (CalculateResult.alpha_s1 * As_prime) >= 0.001 * b * h0;

            if (considerCompressedReinforcement)
            {
                CalculateResult.xM = h0 * (Math.Sqrt(
                    Math.Pow(mu_s * alpha_s2 + mu_s_prime * CalculateResult.alpha_s1, 2) +
                    2 * (mu_s * alpha_s2 + mu_s_prime * CalculateResult.alpha_s1 * a_prime / h0)) -
                    (mu_s * alpha_s2 + mu_s_prime * CalculateResult.alpha_s1));
            }
            else
            {
                // Формула без учета сжатой арматуры
                double temp = (alpha_s2 * As) / b;
                double insideSqrt = 1 + (2 * b * h0) / (alpha_s2 * As);
                CalculateResult.xM = temp * (-1 + Math.Sqrt(insideSqrt));
            }

            if (!ConsiderAxialForce)
            {
                CalculateResult.yc = CalculateResult.xM;
                CalculateResult.xm = CalculateResult.xM;
            }
            else
            {
                CalculateCompressedFlexuralParameters();
            }
            CalculateCrackedSectionProperties();
            CalculateStressesAndCrackWidths();
        }

        /// <summary>
        /// Расчет параметров для сжато-изгибаемого элемента
        /// </summary>
        private void CalculateCompressedFlexuralParameters()
        {
            if (Math.Abs(Ml) < 1e-10)
            {
                CalculateResult.xm = CalculateResult.xM;
                CalculateResult.yc = CalculateResult.xM;
                return;
            }

            CalculateResult.xm = CalculateResult.xM + 
                (CalculateResult.I_red_elastic * Nl) / 
                (CalculateResult.A_red_elastic * Ml);

            double Ab = CalculateResult.xm * b;
            CalculateResult.A_red = Ab + 
                CalculateResult.alpha_s1 * As_prime + 
                CalculateResult.alpha_s1 * As;
            CalculateResult.St_red = Ab * (h - 0.5 * CalculateResult.xm) + 
                As_prime * h0_prime * CalculateResult.alpha_s1 + 
                As * a * CalculateResult.alpha_s1;
            CalculateResult.yc = h - CalculateResult.St_red / CalculateResult.A_red;
        }

        /// <summary>
        /// Расчет характеристик сечения с трещинами
        /// </summary>
        private void CalculateCrackedSectionProperties()
        {
            double Is = As * Math.Pow(h0 - CalculateResult.yc, 2);
            double Is_prime = As_prime * Math.Pow(CalculateResult.yc - a_prime, 2);
            double Ib = b * Math.Pow(CalculateResult.yc, 3) / 3; // Всегда используем /3
            CalculateResult.I_red = Ib + 
                Is * CalculateResult.alpha_s1 + 
                Is_prime * CalculateResult.alpha_s1;
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
            if (ls >= 40 * ds) ls = 40 * ds;
            if (ls >= 40) ls = 40;

            if (ConsiderAxialForce)
            {
                CalculateResult.sigma_s =
                    (Ml * (h0 - CalculateResult.yc) / CalculateResult.I_red - 
                     Nl / CalculateResult.A_red) * CalculateResult.alpha_s1;
                CalculateResult.sigma_s_crc =
                    (CalculateResult.Mcrc * (h0 - CalculateResult.yc) / CalculateResult.I_red - 
                     Nl / CalculateResult.A_red) * CalculateResult.alpha_s1;
            }
            else
            {
                CalculateResult.sigma_s =
                    (Ml * (h0 - CalculateResult.yc) / CalculateResult.I_red) * 
                    CalculateResult.alpha_s1;
                CalculateResult.sigma_s_crc =
                    (CalculateResult.Mcrc * (h0 - CalculateResult.yc) / CalculateResult.I_red) * 
                    CalculateResult.alpha_s1;
            }

            if (!ConsiderAxialForce)
            {
                CalculateResult.psi_s = 1 - 0.8 * CalculateResult.Mcrc / Ml;
            }
            else
            {
                CalculateResult.psi_s = 
                    1 - 0.8 * CalculateResult.sigma_s_crc / CalculateResult.sigma_s;
            }

            double Es = ReinforcementClass.GetElasticityModule();
            CalculateResult.acrc1 = 1.4 * phi2 * phi3 * 
                CalculateResult.psi_s * (CalculateResult.sigma_s / Es) * ls;
            CalculateResult.acrc3 = 1.0 * phi2 * phi3 * 
                CalculateResult.psi_s * (CalculateResult.sigma_s / Es) * ls;

            double sigma_s_full, psi_s_full;
            if (ConsiderAxialForce)
            {
                sigma_s_full = 
                    (M * (h0 - CalculateResult.yc) / CalculateResult.I_red - 
                     N / CalculateResult.A_red) * CalculateResult.alpha_s1;
                psi_s_full = 
                    1 - 0.8 * CalculateResult.sigma_s_crc / sigma_s_full;
            }
            else
            {
                sigma_s_full = 
                    (M * (h0 - CalculateResult.yc) / CalculateResult.I_red) * 
                    CalculateResult.alpha_s1;
                psi_s_full = 1 - 0.8 * CalculateResult.Mcrc / M;
            }

            CalculateResult.acrc2 = 1.0 * phi2 * phi3 * 
                psi_s_full * (sigma_s_full / Es) * ls;
        }

        /// <summary>
        /// Проверка условий
        /// </summary>
        private void CheckConditions()
        {
            bool condition1 = CalculateResult.acrc1 <= acrc_ult_l;
            double acrc_total = 
                CalculateResult.acrc1 + 
                CalculateResult.acrc2 - 
                CalculateResult.acrc3;
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
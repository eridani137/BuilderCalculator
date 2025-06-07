using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_09
{
    public class Calculator : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; }

        public Calculator()
        {
            CalculateResult = new CalculateResult(this);
        }

        [InputParameter("Изгибающий момент от полной нагрузки, кг·см")]
        public double M { get; set; } = 1000000.0;

        [InputParameter("Изгибающий момент от длительной части нагрузки, кг·см")]
        public double Ml { get; set; } = 800000.0;

        [InputParameter("Предельный прогиб, см")]
        public double Fult { get; set; } = 4.0;

        [InputParameter("Схема нагружения (1, 2, 3, 4)")]
        public int Scheme { get; set; } = 1;

        [InputParameter("Расчетный пролет балки, см")]
        public double L { get; set; } = 600.0;

        [InputParameter("Ширина сечения, см")] public double B { get; set; } = 30.0;

        [InputParameter("Высота сечения, см")] public double H { get; set; } = 45.0;

        [InputParameter("Защитный слой бетона растянутой зоны, см")]
        public double A { get; set; } = 5.0;

        [InputParameter("Площадь растянутой арматуры, см^2")]
        public double As { get; set; } = 16.0;

        [InputParameter("Класс бетона")] public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B25;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A500;


        public override BaseCalculateResult Calculate()
        {
            const double epsilonB1RedShort = 0.0015;
            const double epsilonB1RedLong = 0.0028;

            double R_b_ser = ConcreteClass.GetRbser();
            double E_s = ReinforcementClass.GetEs();

            // Определение s в зависимости от схемы нагружения
            double s;
            switch (Scheme)
            {
                case 1:
                    s = 1.0 / 12.0;
                    break;
                case 2:
                    s = 5.0 / 48.0;
                    break;
                case 3:
                    s = 1.0 / 3.0;
                    break;
                case 4:
                    s = 1.0 / 4.0;
                    break;
                default:
                    throw new ArgumentException("Неверная схема нагружения");
            }

            // Вычисление h_0
            double h_0 = H - A;

            // Расчет для кратковременного действия нагрузок
            double E_b_red_short = R_b_ser / epsilonB1RedShort;
            double mu_s_alpha_s2_short = (As / (B * h_0)) * (E_s / E_b_red_short);
            double xm_short = h_0 * (Math.Sqrt(mu_s_alpha_s2_short * mu_s_alpha_s2_short + 2 * mu_s_alpha_s2_short) -
                                     mu_s_alpha_s2_short);
            double z_short = h_0 - xm_short / 3;
            double D_short = E_s * As * z_short * (h_0 - xm_short);
            double oneOverR1 = M / D_short;
            double oneOverR2 = Ml / D_short;

            // Расчет для длительного действия нагрузок
            double E_b_red_long = R_b_ser / epsilonB1RedLong;
            double mu_s_alpha_s2_long = (As / (B * h_0)) * (E_s / E_b_red_long);
            double xm_long = h_0 * (Math.Sqrt(mu_s_alpha_s2_long * mu_s_alpha_s2_long + 2 * mu_s_alpha_s2_long) -
                                    mu_s_alpha_s2_long);
            double z_long = h_0 - xm_long / 3;
            double D_long = E_s * As * z_long * (h_0 - xm_long);
            double oneOverR3 = Ml / D_long;

            // Полная кривизна
            double oneOverR = oneOverR1 - oneOverR2 + oneOverR3;

            // Прогиб
            double f = s * L * L * oneOverR;

            // Результат
            bool result = f <= Fult;

            // Сохранение результатов
            CalculateResult.EbRed = E_b_red_short;
            CalculateResult.MuSAlphaS2 = mu_s_alpha_s2_short;
            CalculateResult.Xm = xm_short;
            CalculateResult.Z = z_short;
            CalculateResult.D = D_short;
            CalculateResult.OneOverR1 = oneOverR1;
            CalculateResult.OneOverR2 = oneOverR2;
            CalculateResult.OneOverR3 = oneOverR3;
            CalculateResult.OneOverR = oneOverR;
            CalculateResult.F = f;
            CalculateResult.Result = result;

            return CalculateResult;
        }
    }
}
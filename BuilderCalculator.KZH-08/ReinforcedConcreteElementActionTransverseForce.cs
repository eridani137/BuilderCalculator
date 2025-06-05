using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_08
{
    public class ReinforcedConcreteElementActionTransverseForce : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; set; }

        public ReinforcedConcreteElementActionTransverseForce()
        {
            CalculateResult = new CalculateResult(this);
        }

        public ReinforcedConcreteElementActionTransverseForce(double m, double Q, double n, double q, double b,
            double h, double a, double h0, double asw, double sw, double @as, double asComp,
            ConcreteClass concreteClass, double gammaB, ReinforcementClass longReinfClass,
            ReinforcementClass transReinfClass)
        {
            CalculateResult = new CalculateResult(this);
            M = m;
            this.Q = Q;
            N = n;
            this.q = q;
            this.b = b;
            this.h = h;
            this.a = a;
            this.h0 = h0;
            Asw = asw;
            this.sw = sw;
            As = @as;
            As_comp = asComp;
            ConcreteClass = concreteClass;
            gamma_b = gammaB;
            LongReinfClass = longReinfClass;
            TransReinfClass = transReinfClass;
        }

        [InputParameter("Изгибающий момент M (кг*см)")]
        public double M { get; set; } = 5.5e5;

        [InputParameter("Поперечная сила Q (кг)")]
        public double Q { get; set; } = 2e4;

        [InputParameter("Продольная сила N (кг)")]
        public double N { get; set; } = -3e3;

        [InputParameter("Распределенная нагрузка q (кг/см)")]
        public double q { get; set; } = 10;

        [InputParameter("Ширина сечения b (см)")]
        public double b { get; set; } = 30;

        [InputParameter("Высота сечения h (см)")]
        public double h { get; set; } = 60;

        [InputParameter("Толщина защитного слоя a (см)")]
        public double a { get; set; } = 4;

        [InputParameter("Рабочая высота сечения h0 (см)")]
        public double h0 { get; set; } = 56;

        [InputParameter("Площадь поперечной арматуры Asw (см²)")]
        public double Asw { get; set; } = 2.1;

        [InputParameter("Шаг поперечной арматуры sw (см)")]
        public double sw { get; set; } = 20;

        [InputParameter("Площадь растянутой арматуры As (см²)")]
        public double As { get; set; } = 10;

        [InputParameter("Площадь сжатой арматуры A's (см²)")]
        public double As_comp { get; set; } = 3;

        [InputParameter("Класс бетона")] public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B35;

        [InputParameter("Коэффициент условий работы бетона γb")]
        public double gamma_b { get; set; } = 1.0;

        [InputParameter("Класс продольной арматуры")]
        public ReinforcementClass LongReinfClass { get; set; } = ReinforcementClass.A400;

        [InputParameter("Класс поперечной арматуры")]
        public ReinforcementClass TransReinfClass { get; set; } = ReinforcementClass.A400;


        public override BaseCalculateResult Calculate()
        {
            // Автоматический расчет h0 если не задан
            if (h0 <= 0.001)
            {
                h0 = h - a;
            }

            // 1. Определение характеристик бетона
            double Eb = ConcreteClass.GetEb();
            double Rb = ConcreteClass.GetRb() * gamma_b;
            double Rbt = ConcreteClass.GetRbt() * gamma_b;

            // 2. Определение характеристик арматуры
            double Es = 2.04e6;
            double Rs = LongReinfClass.GetRs();
            double Rsw = TransReinfClass.GetRsw();

            // 3. Расчет коэффициентов
            double alpha = Es / Eb;
            double Ab = b * h - As - As_comp;
            CalculateResult.sigma_cp = Math.Abs(N) / (Ab + alpha * (As + As_comp));
            CalculateResult.phi_n = 1 + CalculateResult.sigma_cp / Rb;

            // 4. Проверка прочности между трещинами
            double phi_b1 = 0.3;
            double Q_check1 = CalculateResult.phi_n * phi_b1 * Rb * b * h0;
            bool check1 = Q <= Q_check1;

            // 5. Проверка прочности на поперечную силу
            double phi_b2 = 1.5;
            double phi_sw = 0.75;
            double C = 3 * h0; // Максимальная проекция
            double C0 = Math.Min(C, 2 * h0);

            // Расчет Qb с ограничениями
            double Qb_base = CalculateResult.phi_n * phi_b2 * Rbt * b * Math.Pow(h0, 2) / C;
            double Qb_min = 0.5 * Rbt * b * h0;
            double Qb_max = 2.5 * Rbt * b * h0;
            CalculateResult.Qb = Qb_base.Clamp(Qb_min, Qb_max);

            CalculateResult.qsw = Rsw * Asw / sw;
            double Qsw_trans = phi_sw * CalculateResult.qsw * C0;
            double qC = q * C;
            bool check2 = Q <= CalculateResult.Qb + Qsw_trans + qC;

            // 6. Проверка прочности на изгибающий момент
            double C_moment = h0; // Минимальная проекция
            double C0_moment = Math.Min(C_moment, 2 * h0);
            CalculateResult.zs = 0.9 * h0;
            CalculateResult.Ns = Rs * As;
            CalculateResult.Ms = CalculateResult.Ns * CalculateResult.zs;
            CalculateResult.Qsw = CalculateResult.qsw * C0_moment;
            CalculateResult.Msw = 0.5 * CalculateResult.Qsw * C0_moment;
            bool check3 = M <= CalculateResult.Ms + CalculateResult.Msw;

            CalculateResult.Result = check1 && check2 && check3;

            return CalculateResult;
        }
    }
}
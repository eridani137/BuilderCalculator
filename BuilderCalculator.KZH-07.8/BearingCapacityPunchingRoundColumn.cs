using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_07._8
{
    public class BearingCapacityPunchingRoundColumn : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; set; }

        public BearingCapacityPunchingRoundColumn()
        {
            CalculateResult = new CalculateResult(this);
        }

        [InputParameter("Сосредоточенная сила, кг")]
        public double F { get; set; } = 20000;

        [InputParameter("Изгибающий момент вдоль оси X, кг·см")]
        public double Mx { get; set; } = 100_000;

        [InputParameter("Изгибающий момент вдоль оси Y, кг·см")]
        public double My { get; set; } = 120_000;

        [InputParameter("Учитывать изгибающие моменты")]
        public bool ConsiderBendingMoments { get; set; } = true;

        [InputParameter("Делить моменты пополам")]
        public bool DivideMomentsByHalf { get; set; } = true;

        [InputParameter("Наружный диаметр колонны, см")]
        public double D { get; set; } = 60;

        [InputParameter("Высота сечения, см")] 
        public double h { get; set; } = 20;

        [InputParameter("Защитный слой бетона растянутой зоны, см")]
        public double a { get; set; } = 5;

        [InputParameter("Класс бетона")]
        public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B20;

        [InputParameter("Коэффициент условий работы бетона")]
        public double GammaBi { get; set; } = 0.9;

        [InputParameter("Учитывать поперечную арматуру")]
        public bool ConsiderShearReinforcement { get; set; } = true;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A400;

        [InputParameter("Площадь поперечной арматуры, см2")]
        public double Asw { get; set; } = 1.06;

        [InputParameter("Шаг поперечной арматуры, см")]
        public double Sw { get; set; } = 10;

        public override BaseCalculateResult Calculate()
        {
            double h0 = h - a;
            double u = Math.PI * (D + h0);
            double Ab = u * h0;

            // Получение характеристик материалов с проверкой
            double Rbt = ConcreteClass.GetRbt(GammaBi);
            double Rsw = ReinforcementClass.GetRsw();

            // Расчет параметров бетона
            CalculateResult.Fb_ult = Rbt * Ab;
            CalculateResult.Wb = Math.PI * Math.Pow(D + h0, 2) / 4;
            CalculateResult.Mb_ult = Rbt * CalculateResult.Wb * h0;

            // Инициализация параметров арматуры
            CalculateResult.qsw = 0;
            CalculateResult.Fsw_ult = 0;
            CalculateResult.Msw_ult = 0;
            CalculateResult.Wsw = CalculateResult.Wb;

            if (ConsiderShearReinforcement && Asw > 0 && Sw > 0)
            {
                CalculateResult.qsw = Rsw * Asw / Sw;
                CalculateResult.Fsw_ult = 0.8 * CalculateResult.qsw * u;

                // Корректировка Fsw_ult согласно п.8.1.48 СП
                CalculateResult.Fsw_ult = Math.Max(CalculateResult.Fsw_ult, 0.25 * CalculateResult.Fb_ult);
                CalculateResult.Fsw_ult = Math.Min(CalculateResult.Fsw_ult, CalculateResult.Fb_ult);

                // Расчет момента для арматуры
                CalculateResult.Msw_ult = 0.8 * CalculateResult.qsw * CalculateResult.Wsw;
                
                // Корректировка Msw_ult согласно п.8.1.50 СП
                if (CalculateResult.Msw_ult > CalculateResult.Mb_ult)
                {
                    CalculateResult.Msw_ult = CalculateResult.Mb_ult;
                }
            }

            // Расчет результирующих усилий
            CalculateResult.F_ult = CalculateResult.Fb_ult + 
                                  (ConsiderShearReinforcement ? CalculateResult.Fsw_ult : 0);
            
            CalculateResult.M_ult = CalculateResult.Mb_ult + 
                                  (ConsiderShearReinforcement ? CalculateResult.Msw_ult : 0);

            // Расчет результирующего момента
            CalculateResult.M = 0;
            if (ConsiderBendingMoments && (Math.Abs(Mx) > 0.001 || Math.Abs(My) > 0.001))
            {
                double mx = DivideMomentsByHalf ? Mx / 2 : Mx;
                double my = DivideMomentsByHalf ? My / 2 : My;
                CalculateResult.M = Math.Sqrt(Math.Pow(mx, 2) + Math.Pow(my, 2));
            }

            // Проверка условий прочности
            if (ConsiderBendingMoments && CalculateResult.M > 0.001)
            {
                // Условия согласно п.8.1.49 СП
                double leftPart = CalculateResult.M / CalculateResult.M_ult;
                double rightPart = F / (2 * CalculateResult.F_ult);
                double combined = F / CalculateResult.F_ult + CalculateResult.M / CalculateResult.M_ult;

                CalculateResult.Result = (leftPart <= rightPart) && (combined <= 1);
            }
            else
            {
                // Проверка без учета моментов
                CalculateResult.Result = F <= CalculateResult.F_ult;
            }

            return CalculateResult;
        }
    }
}
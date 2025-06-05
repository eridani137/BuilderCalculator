using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_07._8
{
    public class BearingCapacityPunchingRoundColumn : BaseBuilderCalculator
    {
        private readonly CalculateResult _calculateResult;

        public BearingCapacityPunchingRoundColumn()
        {
            _calculateResult = new CalculateResult(this);
        }

        [InputParameter("Сосредоточенная сила, кг")]
        public double F { get; set; } = 2e4;

        [InputParameter("Изгибающий момент вдоль оси X, кг·см")]
        public double Mx { get; set; } = 1e5;

        [InputParameter("Изгибающий момент вдоль оси Y, кг·см")]
        public double My { get; set; } = 1.2e5;

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
            _calculateResult.Fb_ult = Rbt * Ab;
            _calculateResult.Wb = Math.PI * Math.Pow(D + h0, 2) / 4;
            _calculateResult.Mb_ult = Rbt * _calculateResult.Wb * h0;

            // Инициализация параметров арматуры
            _calculateResult.qsw = 0;
            _calculateResult.Fsw_ult = 0;
            _calculateResult.Msw_ult = 0;
            _calculateResult.Wsw = _calculateResult.Wb;

            if (ConsiderShearReinforcement && Asw > 0 && Sw > 0)
            {
                _calculateResult.qsw = Rsw * Asw / Sw;
                _calculateResult.Fsw_ult = 0.8 * _calculateResult.qsw * u;

                // Корректировка Fsw_ult согласно п.8.1.48 СП
                _calculateResult.Fsw_ult = Math.Max(_calculateResult.Fsw_ult, 0.25 * _calculateResult.Fb_ult);
                _calculateResult.Fsw_ult = Math.Min(_calculateResult.Fsw_ult, _calculateResult.Fb_ult);

                // Расчет момента для арматуры
                _calculateResult.Msw_ult = 0.8 * _calculateResult.qsw * _calculateResult.Wsw;
                
                // Корректировка Msw_ult согласно п.8.1.50 СП
                if (_calculateResult.Msw_ult > _calculateResult.Mb_ult)
                {
                    _calculateResult.Msw_ult = _calculateResult.Mb_ult;
                }
            }

            // Расчет результирующих усилий
            _calculateResult.F_ult = _calculateResult.Fb_ult + 
                                  (ConsiderShearReinforcement ? _calculateResult.Fsw_ult : 0);
            
            _calculateResult.M_ult = _calculateResult.Mb_ult + 
                                  (ConsiderShearReinforcement ? _calculateResult.Msw_ult : 0);

            // Расчет результирующего момента
            _calculateResult.M = 0;
            if (ConsiderBendingMoments && (Math.Abs(Mx) > 0.001 || Math.Abs(My) > 0.001))
            {
                double mx = DivideMomentsByHalf ? Mx / 2 : Mx;
                double my = DivideMomentsByHalf ? My / 2 : My;
                _calculateResult.M = Math.Sqrt(Math.Pow(mx, 2) + Math.Pow(my, 2));
            }

            // Проверка условий прочности
            if (ConsiderBendingMoments && _calculateResult.M > 0.001)
            {
                // Условия согласно п.8.1.49 СП
                double leftPart = _calculateResult.M / _calculateResult.M_ult;
                double rightPart = F / (2 * _calculateResult.F_ult);
                double combined = F / _calculateResult.F_ult + _calculateResult.M / _calculateResult.M_ult;

                _calculateResult.Result = (leftPart <= rightPart) && (combined <= 1);
            }
            else
            {
                // Проверка без учета моментов
                _calculateResult.Result = F <= _calculateResult.F_ult;
            }

            return _calculateResult;
        }
    }
}
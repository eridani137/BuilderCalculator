using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;

namespace BuilderCalculator.KZH_06
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }
        
        [OutputParameter("Расчетное сопротивление сцеплению, кН/см²")]
        public double Rbond { get; set; }

        [OutputParameter("Площадь сечения арматуры, см²")]
        public double As { get; set; }

        [OutputParameter("Периметр арматуры, см")]
        public double Us { get; set; }

        [OutputParameter("Базовая длина анкеровки, см")]
        public double L0an { get; set; }

        [OutputParameter("Расчетная длина анкеровки/нахлестки, см")]
        public double Lan { get; set; }

        [OutputParameter("Относительная длина (lan/ds)")]
        public double Lan_ds { get; set; }

        [OutputParameter("Условие: lan >= 0.3*l0an")]
        public bool Result1 { get; set; }

        [OutputParameter("Условие: lan >= 15*ds")]
        public bool Result2 { get; set; }

        [OutputParameter("Условие: lan >= 20 см")]
        public bool Result3 { get; set; }

    }
}
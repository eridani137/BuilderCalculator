using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;

namespace BuilderCalculator.KZH_02
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }
        
        [OutputParameter("Коэффициент ξR")]
        public double XiR { get; set; }

        [OutputParameter("Коэффициент αR")]
        public double AlphaR { get; set; }

        [OutputParameter("Коэффициент αm")]
        public double AlphaM { get; set; }

        [OutputParameter("Площадь арматуры, см^2")]
        public double As { get; set; }
    }
}
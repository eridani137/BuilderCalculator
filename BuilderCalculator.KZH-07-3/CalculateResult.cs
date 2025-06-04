using Calculators.Shared;

namespace BuilderCalculator.KZH_07_3
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(Calculator calculator) : base(calculator)
        {
            Calculator = calculator;
        }
        
        public override void PrintSummary()
        {
            throw new System.NotImplementedException();
        }
    }
}
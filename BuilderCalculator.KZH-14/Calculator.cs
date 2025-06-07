using Calculators.Shared.Abstractions;

namespace BuilderCalculator.KZH_14
{
    public class Calculator : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; }

        public Calculator()
        {
            CalculateResult = new CalculateResult(this);
        }

        
        public override BaseCalculateResult Calculate()
        {
            
            
            return CalculateResult;
        }
    }
}
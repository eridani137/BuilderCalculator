using Calculators.Shared.Abstractions;

namespace BuilderCalculator.KZH_12
{
    public class Calculator : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; set; }

        public Calculator()
        {
            CalculateResult = new CalculateResult(this);
        }
        
        
        
        public override BaseCalculateResult Calculate()
        {
            throw new System.NotImplementedException();
        }
    }
}
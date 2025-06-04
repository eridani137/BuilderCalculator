using Calculators.Shared;

namespace BuilderCalculator.KZH_07_3
{
    public class Calculator : BaseBuilderCalculator
    {
        CalculateResult CalculateResult { get; set; }
        
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
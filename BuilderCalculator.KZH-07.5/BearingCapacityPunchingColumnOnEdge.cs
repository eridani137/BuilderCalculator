using Calculators.Shared.Abstractions;

namespace BuilderCalculator.KZH_07._5
{
    public class BearingCapacityPunchingColumnOnEdge : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; }

        public BearingCapacityPunchingColumnOnEdge()
        {
            CalculateResult = new CalculateResult(this);
        }

        
        public override BaseCalculateResult Calculate()
        {
            
            
            return CalculateResult;
        }
    }
}
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;

namespace BuilderCalculator.KZH_14
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }
        
        [OutputParameter("e0")]
        public double E0 { get; set; }

        [OutputParameter("δe")]
        public double DeltaE { get; set; }

        [OutputParameter("M1")]
        public double M1 { get; set; }

        [OutputParameter("Ml1")]
        public double Ml1 { get; set; }

        [OutputParameter("φl")]
        public double PhiL { get; set; }

        [OutputParameter("kb")]
        public double Kb { get; set; }

        [OutputParameter("I")]
        public double I { get; set; }

        [OutputParameter("Is")]
        public double Is { get; set; }

        [OutputParameter("D")]
        public double D { get; set; }

        [OutputParameter("Ncr")]
        public double Ncr { get; set; }

        [OutputParameter("η")]
        public double Eta { get; set; }

        [OutputParameter("M")]
        public double MCalculated { get; set; }

        [OutputParameter("A")]
        public double Area { get; set; }

        [OutputParameter("ξcir")]
        public double XiCir { get; set; }

        [OutputParameter("φ")]
        public double Phi { get; set; }

        [OutputParameter("Mult")]
        public double Mult { get; set; }

        [OutputParameter("Result")]
        public bool Result { get; set; }
    }
}
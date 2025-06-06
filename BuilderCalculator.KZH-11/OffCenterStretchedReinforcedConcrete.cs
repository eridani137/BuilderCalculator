using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_11
{
    public class OffCenterStretchedReinforcedConcrete : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; set; }

        public OffCenterStretchedReinforcedConcrete()
        {
            CalculateResult = new CalculateResult(this);
        }

        public OffCenterStretchedReinforcedConcrete(double m, double n, double b, double h, double a, double aPrime, double @as, double asPrime, ConcreteClass concreteClass, ReinforcementClass reinforcementClass, double gammaBi)
        {
            CalculateResult = new CalculateResult(this);
            M = m;
            N = n;
            this.b = b;
            this.h = h;
            this.a = a;
            this.aPrime = aPrime;
            As = @as;
            AsPrime = asPrime;
            ConcreteClass = concreteClass;
            ReinforcementClass = reinforcementClass;
            this.gammaBi = gammaBi;
        }

        [InputParameter("Изгибающий момент, кг·см")]
        public double M { get; set; } = 17e5;

        [InputParameter("Продольная растягивающая сила, кг")]
        public double N { get; set; } = 10e3;

        [InputParameter("Ширина сечения, см")]
        public double b { get; set; } = 40.0;

        [InputParameter("Высота сечения, см")]
        public double h { get; set; } = 50.0;

        [InputParameter("Защитный слой бетона растянутой зоны, см")]
        public double a { get; set; } = 5.0;

        [InputParameter("Защитный слой бетона сжатой зоны, см")]
        public double aPrime { get; set; } = 5.0;

        [InputParameter("Площадь растянутой арматуры, см²")]
        public double As { get; set; } = 16.09;

        [InputParameter("Площадь сжатой арматуры, см²")]
        public double AsPrime { get; set; } = 16.09;

        [InputParameter("Класс бетона")]
        public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B25;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A400;

        [InputParameter("Коэффициент условий работы бетона")]
        public double gammaBi { get; set; } = 0.9;
        
        public override BaseCalculateResult Calculate()
        {
            double Rb = ConcreteClass.GetRb() * gammaBi;
            double Rs = ReinforcementClass.GetRs();
            double Rsc = ReinforcementClass.GetRsc(); // Для x, но не для моментов

            double h0 = h - a;
            CalculateResult.e0 = M / N;
            CalculateResult.e_prime = (h0 - aPrime) / 2 + CalculateResult.e0;
            CalculateResult.e = CalculateResult.e_prime - h0 + aPrime;
            CalculateResult.x = (Rs * As - Rsc * AsPrime - N) / (Rb * b);

            // Исправленные моменты
            CalculateResult.Mult = Rs * AsPrime * (h0 - aPrime); // M_ult = Rs * A_s' * (h0 - a')
            CalculateResult.MultPrime = Rs * As * (h0 - aPrime); // M_ult' = Rs * A_s * (h0 - a')

            CalculateResult.Ne = N * CalculateResult.e;
            CalculateResult.Ne_prime = N * CalculateResult.e_prime;

            // Проверка условий
            CalculateResult.Result = (CalculateResult.Ne <= CalculateResult.Mult) && (CalculateResult.Ne_prime <= CalculateResult.MultPrime);

            return CalculateResult;
        }
    }
}
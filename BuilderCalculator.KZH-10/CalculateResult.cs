using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;

namespace BuilderCalculator.KZH_10
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }

        [OutputParameter("Момент инерции бетона, см^4")]
        public double I { get; set; }

        [OutputParameter("Момент инерции растянутой арматуры, см^4")]
        public double Is { get; set; }

        [OutputParameter("Момент инерции сжатой арматуры, см^4")]
        public double Is_prime { get; set; }

        [OutputParameter("Момент инерции сжатой зоны бетона, см^4")]
        public double Ib { get; set; }

        [OutputParameter("Приведенный момент инерции, см^4")]
        public double Ired { get; set; }

        [OutputParameter("Момент сопротивления, см^3")]
        public double Wred { get; set; }

        [OutputParameter("Момент образования трещин, кг·см")]
        public double Mcrc { get; set; }

        [OutputParameter("Относительная деформация бетона")]
        public double epsilon_b1_red { get; set; }

        [OutputParameter("Коэффициент ψ_s")] public double psi_s { get; set; }

        [OutputParameter("Приведенный модуль упругости бетона, кг/см^2")]
        public double Eb_red { get; set; }

        [OutputParameter("Приведенный модуль упругости арматуры, кг/см^2")]
        public double Es_red { get; set; }

        [OutputParameter("Модуль упругости бетона для расчета, кг/см^2")]
        public double Eb1 { get; set; }

        [OutputParameter("Коэффициент α_s1")] public double alpha_s1 { get; set; }

        [OutputParameter("Коэффициент α_s2")] public double alpha_s2 { get; set; }

        [OutputParameter("Относительная площадь растянутой арматуры")]
        public double mu_s { get; set; }

        [OutputParameter("Относительная площадь сжатой арматуры")]
        public double mu_s_prime { get; set; }

        [OutputParameter("Высота сжатой зоны, см")]
        public double xm { get; set; }

        [OutputParameter("Изгибная жесткость D1")]
        public double D1 { get; set; }

        [OutputParameter("Кривизна (1/r)1")] 
        public double one_over_r1 { get; set; }

        [OutputParameter("Изгибная жесткость D2")]
        public double D2 { get; set; }

        [OutputParameter("Кривизна (1/r)2")] 
        public double one_over_r2 { get; set; }

        [OutputParameter("Изгибная жесткость D3")]
        public double D3 { get; set; }

        [OutputParameter("Кривизна (1/r)3")] public double one_over_r3 { get; set; }

        [OutputParameter("Полная кривизна (1/r)")]
        public double one_over_r { get; set; }

        [OutputParameter("Прогиб, см")] public double f { get; set; }

        [OutputParameter("Результат проверки прочности")]
        public bool Result { get; set; }
    }
}
using System.Text;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Exceptions;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace BuilderCalculator.KZH_12
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }
        
        [OutputParameter("Площадь локального сжатия (см2)")]
        public double Ab_loc { get; set; }

        [OutputParameter("Максимальная расчетная площадь (см2)")]
        public double Ab_max { get; set; }

        [OutputParameter("Коэффициент φ_b")]
        public double phi_b { get; set; }

        [OutputParameter("Расчетное сопротивление бетона (кг/см2)")]
        public double Rb_loc { get; set; }

        [OutputParameter("Эффективная площадь (см2)")]
        public double Ab_loc_ef { get; set; }

        [OutputParameter("Коэффициент φ_sxy")]
        public double phi_sxy { get; set; }

        [OutputParameter("Коэффициент армирования μ_s,xy")]
        public double mu_sxy { get; set; }

        [OutputParameter("Расчетное сопротивление с учетом армирования (кг/см2)")]
        public double Rbs_loc { get; set; }

        [OutputParameter("Коэффициент ψ")]
        public double Psi { get; set; } = 1.0;

        [OutputParameter("Расчетное усилие (кг)")]
        public double DesignForce { get; set; }

        [OutputParameter("Результат")]
        public bool Result { get; set; }
    }
}
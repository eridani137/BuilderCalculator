using System.Text;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace BuilderCalculator.KZH_11
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }

        [OutputParameter("Эксцентриситет, см")]
        public double e0 { get; set; }

        [OutputParameter("Расчетный эксцентриситет, см")]
        public double e_prime { get; set; }

        [OutputParameter("Расстояние до точки приложения силы, см")]
        public double e { get; set; }

        [OutputParameter("Высота сжатой зоны, см")]
        public double x { get; set; }

        [OutputParameter("Момент для растянутой арматуры, кг·см")]
        public double Mult { get; set; }

        [OutputParameter("Момент для сжатой арматуры, кг·см")]
        public double MultPrime { get; set; }

        [OutputParameter("Произведение N·e, кг·см")]
        public double Ne { get; set; }

        [OutputParameter("Произведение N·e', кг·см")]
        public double Ne_prime { get; set; }

        [OutputParameter("Результат")]
        public bool Result { get; set; }
    }
}
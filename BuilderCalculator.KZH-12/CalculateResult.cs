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

        public bool Result { get; set; }

        public override void PrintSummary()
        {
            if (!(Calculator is BearingCapacityPunchingColumnAtCorner calculator))
            {
                throw new BadCalculatorException();
            }

            var sb = new StringBuilder();
            
            sb.AppendLine("=== РЕЗУЛЬТАТЫ РАСЧЕТА НА МЕСТНОЕ СЖАТИЕ ===");
            sb.AppendLine($"• Схема нагружения: {calculator.CaseType}");
            sb.AppendLine($"• Площадь локального сжатия (Ab_loc): {Ab_loc:F1} см²");
            sb.AppendLine($"• Максимальная площадь (Ab_max): {Ab_max:F1} см²");
            sb.AppendLine($"• Коэффициент φ_b: {phi_b:F3}");
            sb.AppendLine($"• Расчетное сопротивление бетона (Rb_loc): {Rb_loc:F1} кг/см²");
            
            if (calculator.IncludeIndirectReinforcement)
            {
                sb.AppendLine($"• Эффективная площадь (Ab_loc_ef): {Ab_loc_ef:F1} см²");
                sb.AppendLine($"• Коэффициент φ_sxy: {phi_sxy:F3}");
                sb.AppendLine($"• Коэффициент армирования μ_s,xy: {mu_sxy:F3}");
                sb.AppendLine($"• Сопротивление с армированием (Rbs_loc): {Rbs_loc:F1} кг/см²");
            }
            
            sb.AppendLine($"• Коэффициент ψ: {Psi:F1}");
            sb.AppendLine($"• Расчетное усилие: {DesignForce:F0} кг");
            sb.AppendLine($"• Приложенное усилие: {calculator.N:F0} кг");
            sb.AppendLine($"ВЫВОД: Прочность {(Result ? "ОБЕСПЕЧЕНА".MarkupSecondaryColor() : "НЕ ОБЕСПЕЧЕНА".MarkupErrorColor())}");
            
            AnsiConsole.MarkupLine(sb.ToString());
        }
    }
}
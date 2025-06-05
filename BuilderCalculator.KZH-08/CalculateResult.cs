using System.Text;
using Calculators.Shared;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Exceptions;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace BuilderCalculator.KZH_08
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }
        
        [OutputParameter("Напряжение σ_cp (кг/см²)")]
        public double sigma_cp { get; set; }
    
        [OutputParameter("Коэффициент φ_n")]
        public double phi_n { get; set; }
    
        [OutputParameter("Поперечное усилие Qb (кг)")]
        public double Qb { get; set; }
    
        [OutputParameter("Интенсивность арматуры q_sw (кг/см)")]
        public double qsw { get; set; }
    
        [OutputParameter("Усилие в арматуре Q_sw (кг)")]
        public double Qsw { get; set; }
    
        [OutputParameter("Плечо внутренней пары z_s (см)")]
        public double zs { get; set; }
    
        [OutputParameter("Усилие в арматуре N_s (кг)")]
        public double Ns { get; set; }
    
        [OutputParameter("Момент от арматуры M_s (кг*см)")]
        public double Ms { get; set; }
    
        [OutputParameter("Момент от поперечной арматуры M_sw (кг*см)")]
        public double Msw { get; set; }
    
        public bool Result { get; set; }

        public override void PrintSummary()
        {
            if (!(Calculator is ReinforcedConcreteElementActionTransverseForce calculator))
            {
                throw new BadCalculatorException();
            }

            var sb = new StringBuilder();
            
            sb.AppendLine("РЕЗУЛЬТАТЫ РАСЧЕТА");
            sb.AppendLine($"σ_cp = {sigma_cp:F3} кг/см²");
            sb.AppendLine($"φ_n = {phi_n:F3}");
            sb.AppendLine($"Qb = {Qb:F0} кг");
            sb.AppendLine($"q_sw = {qsw:F2} кг/см");
            sb.AppendLine($"Q_sw = {Qsw:F0} кг");
            sb.AppendLine($"z_s = {zs:F1} см");
            sb.AppendLine($"N_s = {Ns:F0} кг");
            sb.AppendLine($"M_s = {Ms:E} кг*см");
            sb.AppendLine($"M_sw = {Msw:E} кг*см");
            sb.AppendLine($"Прочность {(Result ? "обеспечена".MarkupSecondaryColor() : "не обеспечена".MarkupErrorColor())}");
        
            sb.AppendLine("ДОПОЛНИТЕЛЬНЫЕ ПРОВЕРКИ:");
            sb.AppendLine($"Шаг хомутов: {calculator.sw} см ≤ 0.5h0 = {0.5 * calculator.h0} см -> {calculator.sw <= 0.5 * calculator.h0}");
            sb.AppendLine($"Qb_min = {0.5 * calculator.ConcreteClass.GetRbt() * calculator.b * calculator.h0:F0} кг");
            
            AnsiConsole.MarkupLine(sb.ToString());
        }
    }
}
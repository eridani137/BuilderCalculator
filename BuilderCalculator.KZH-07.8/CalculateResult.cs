using System;
using System.Text;
using Calculators.Shared;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace BuilderCalculator.KZH_07._8
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }
        
        [OutputParameter("Результирующий момент (кг·см)")]
        public double M { get; set; }

        [OutputParameter("Предельное усилие бетона (кг)")]
        public double Fb_ult { get; set; }

        [OutputParameter("Момент сопротивления бетона (см2)")]
        public double Wb { get; set; }

        [OutputParameter("Предельный момент бетона (кг·см)")]
        public double Mb_ult { get; set; }

        [OutputParameter("Интенсивность армирования (кг/см)")]
        public double qsw { get; set; }

        [OutputParameter("Предельное усилие арматуры (кг)")]
        public double Fsw_ult { get; set; }

        [OutputParameter("Общее предельное усилие (кг)")]
        public double F_ult { get; set; }

        [OutputParameter("Момент сопротивления арматуры (см2)")]
        public double Wsw { get; set; }

        [OutputParameter("Предельный момент арматуры (кг·см)")]
        public double Msw_ult { get; set; }

        [OutputParameter("Общий предельный момент (кг·см)")]
        public double M_ult { get; set; }

        public bool Result { get; set; }

        public override void PrintSummary()
        {
            if (!(Calculator is BearingCapacityPunchingRoundColumn calculator))
            {
                throw new BagCalculatorException();
            }

            var sb = new StringBuilder();

            sb.AppendLine("### РЕЗУЛЬТАТЫ РАСЧЕТА НА ПРОДАВЛИВАНИЕ ###");
            sb.AppendLine($"Предельное усилие бетона: {Fb_ult:F2} кг");
            sb.AppendLine($"Предельное усилие арматуры: {Fsw_ult:F2} кг");
            sb.AppendLine($"Общее предельное усилие: {F_ult:F2} кг");
            
            if (M > 0)
            {
                sb.AppendLine($"Результирующий момент: {M:F2} кг·см");
                sb.AppendLine($"Предельный момент бетона: {Mb_ult:F2} кг·см");
                sb.AppendLine($"Предельный момент арматуры: {Msw_ult:F2} кг·см");
                sb.AppendLine($"Общий предельный момент: {M_ult:F2} кг·см");
            }
            
            sb.AppendLine($"Проверка прочности: {(Result ? "УСПЕШНО".MarkupSecondaryColor() : "НЕ УДАЛОСЬ".MarkupErrorColor())}");
            sb.AppendLine($"Соотношение F/F_ult: {calculator.F / F_ult:F2}");
            
            if (M > 0)
            {
                sb.AppendLine($"Соотношение M/M_ult: {M / M_ult:F2}");
            }
            
            AnsiConsole.MarkupLine(sb.ToString().EscapeMarkup());
        }
    }
}
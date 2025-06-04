using System;
using System.Text;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace BuilderCalculator.KZH_07._7.Two
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }

        [OutputParameter("Приведенный момент инерции относительно оси Y, см4")]
        public double Iby_prime { get; set; }

        [OutputParameter("Приведенный момент инерции относительно оси X, см4")]
        public double Ibx_prime { get; set; }

        [OutputParameter("Статический момент относительно оси X, см3")]
        public double Sx_prime { get; set; }

        [OutputParameter("Статический момент относительно оси Y, см3")]
        public double Sy_prime { get; set; }

        [OutputParameter("Длина контура продавливания по оси X, см")]
        public double Lx { get; set; }

        [OutputParameter("Длина контура продавливания по оси Y, см")]
        public double Ly { get; set; }

        [OutputParameter("Координата центра тяжести контура по X, см")]
        public double xc { get; set; }

        [OutputParameter("Координата центра тяжести контура по Y, см")]
        public double yc { get; set; }

        [OutputParameter("Момент инерции брутто относительно оси X, см4")]
        public double Ibx { get; set; }

        [OutputParameter("Момент инерции брутто относительно оси Y, см4")]
        public double Iby { get; set; }

        [OutputParameter("Момент сопротивления по оси X, см3")]
        public double Wbx { get; set; }

        [OutputParameter("Момент сопротивления по оси Y, см3")]
        public double Wby { get; set; }

        [OutputParameter("Предельное усилие воспринимаемое бетоном, кгс")]
        public double Fb_ult { get; set; }

        [OutputParameter("Предельный момент по оси X воспринимаемый бетоном, кг·см")]
        public double Mbx_ult { get; set; }

        [OutputParameter("Предельный момент по оси Y воспринимаемый бетоном, кг·см")]
        public double Mby_ult { get; set; }

        [OutputParameter("Интенсивность поперечного армирования, кгс/см")]
        public double qsw { get; set; }

        [OutputParameter("Предельное усилие воспринимаемое арматурой, кгс")]
        public double Fsw_ult { get; set; }

        [OutputParameter("Суммарное предельное усилие, кгс")]
        public double F_ult { get; set; }

        [OutputParameter("Момент сопротивления арматуры по оси X, см3")]
        public double Wsw_x { get; set; }

        [OutputParameter("Момент сопротивления арматуры по оси Y, см3")]
        public double Wsw_y { get; set; }

        [OutputParameter("Предельный момент арматуры по оси X, кг·см")]
        public double Msw_x_ult { get; set; }

        [OutputParameter("Предельный момент арматуры по оси Y, кг·см")]
        public double Msw_y_ult { get; set; }

        [OutputParameter("Суммарный предельный момент по оси X, кг·см")]
        public double Mx_ult { get; set; }

        [OutputParameter("Суммарный предельный момент по оси Y, кг·см")]
        public double My_ult { get; set; }

        public bool Result { get; set; }

        public override void PrintSummary()
        {
            if (!(Calculator is Calculator calculator))
            {
                throw new ApplicationException("Задан неверный тип калькулятора");
            }

            var sb = new StringBuilder();
            sb.AppendLine("=== РЕЗУЛЬТАТЫ РАСЧЕТА НА ПРОДАВЛИВАНИЕ ===");
            sb.AppendLine();

            // Основные геометрические характеристики
            sb.AppendLine("Геометрические характеристики:");
            sb.AppendLine($"  Приведенный момент инерции I'bx: {Ibx_prime:F1} см⁴");
            sb.AppendLine($"  Приведенный момент инерции I'by: {Iby_prime:F1} см⁴");
            sb.AppendLine($"  Длина контура Lx: {Lx:F1} см, Ly: {Ly:F1} см");
            sb.AppendLine($"  Координаты центра тяжести: xc = {xc:F2} см, yc = {yc:F2} см");
            sb.AppendLine();

            // Характеристики прочности
            sb.AppendLine("Предельные усилия:");
            sb.AppendLine($"  Предельное усилие (бетон): {Fb_ult:F0} кгс");
            if (Fsw_ult > 0)
            {
                sb.AppendLine($"  Предельное усилие (арматура): {Fsw_ult:F0} кгс");
            }

            sb.AppendLine($"  Суммарное предельное усилие: {F_ult:F0} кгс");
            sb.AppendLine();

            sb.AppendLine("Предельные моменты:");
            sb.AppendLine($"  По оси X (бетон): {Mbx_ult:F0} кг·см");
            if (Msw_x_ult > 0)
            {
                sb.AppendLine($"  По оси X (арматура): {Msw_x_ult:F0} кг·см");
            }

            sb.AppendLine($"  Суммарный по оси X: {Mx_ult:F0} кг·см");

            sb.AppendLine($"  По оси Y (бетон): {Mby_ult:F0} кг·см");
            if (Msw_y_ult > 0)
            {
                sb.AppendLine($"  По оси Y (арматура): {Msw_y_ult:F0} кг·см");
            }

            sb.AppendLine($"  Суммарный по оси Y: {My_ult:F0} кг·см");
            sb.AppendLine();

            // Итоговый результат
            sb.AppendLine("=== ИТОГОВАЯ ПРОВЕРКА ПРОЧНОСТИ ===");
            sb.AppendLine(Result
                ? "Условие прочности выполняется: (F/Fult) + (Mx/Mx_ult) + (My/My_ult) ≤ 1".MarkupSecondaryColor()
                : "Условие прочности не выполняется: (F/Fult) + (Mx/Mx_ult) + (My/My_ult) > 1".MarkupErrorColor());

            sb.AppendLine();
            sb.AppendLine(Result
                ? "РЕЗУЛЬТАТ: ПРОЧНОСТЬ ОБЕСПЕЧЕНА".MarkupSecondaryColor()
                : "РЕЗУЛЬТАТ: ПРОЧНОСТЬ НЕ ОБЕСПЕЧЕНА".MarkupErrorColor());
            
            AnsiConsole.MarkupLine(sb.ToString().EscapeMarkup());
        }
    }
}
using Calculators.Shared;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace Calculators.KZH_04
{
    public class CalculateResult : BaseCalculateResult
    {
        private readonly CheckingCrackAndOpeningWidth _calculator;

        public CalculateResult(CheckingCrackAndOpeningWidth calculator)
        {
            _calculator = calculator;
        }

        // Геометрические характеристики
        public double I_red { get; set; } // Приведенный момент инерции, см⁴
        public double W_red { get; set; } // Приведенный момент сопротивления, см³
        public double W_pl { get; set; } // Пластический момент сопротивления, см³
        public double ex { get; set; } // Эксцентриситет, см
        public double Mcrc { get; set; } // Момент трещинообразования, кг·см

        // Деформационные характеристики
        public double Eb_red { get; set; } // Приведенный модуль упругости бетона, кг/см²
        public double alpha_s1 { get; set; } // Отношение модулей упругости
        public double xM { get; set; } // Высота сжатой зоны, см
        public double xm { get; set; } // Высота сжатой зоны с учетом нагрузок, см
        public double A_red { get; set; } // Приведенная площадь, см²
        public double St_red { get; set; } // Статический момент, см³
        public double yc { get; set; } // Расстояние до центра тяжести, см
        public double xt { get; set; } // Высота растянутой зоны, см

        // Напряжения
        public double sigma_s { get; set; } // Напряжение в арматуре, кг/см²
        public double sigma_s_crc { get; set; } // Напряжение в арматуре при трещинообразовании, кг/см²
        public double psi_s { get; set; } // Коэффициент

        // Ширины раскрытия трещин
        public double acrc1 { get; set; } // Ширина раскрытия от длительных нагрузок, см
        public double acrc2 { get; set; } // Ширина раскрытия от полных нагрузок, см
        public double acrc3 { get; set; } // Ширина раскрытия от кратковременных длительных нагрузок, см

        // Результат проверки
        public bool Result { get; set; } // true - прочность обеспечена, false - не обеспечена

        public override void PrintSummary()
        {
            var summary = $@"
                           === РЕЗУЛЬТАТЫ РАСЧЕТА ТРЕЩИНООБРАЗОВАНИЯ ===
                           Момент трещинообразования: {Mcrc:F2} кг·см
                           Трещины образуются: {(_calculator.IsCrackingOccurred() ? "ДА".MarkupErrorColor() : "НЕТ".MarkupSecondaryColor())}

                           Ширина раскрытия трещин:
                           - От длительных нагрузок (acrc1): {acrc1:F4} см ≤ {_calculator.acrc_ult_l:F4} см
                           - От полных нагрузок (acrc2): {acrc2:F4} см
                           - От кратковременных длительных (acrc3): {acrc3:F4} см
                           - Общая ширина: {(acrc1 + acrc2 - acrc3):F4} см ≤ {_calculator.acrc_ult:F4} см

                           РЕЗУЛЬТАТ: {(Result ? "ПРОЧНОСТЬ ОБЕСПЕЧЕНА".MarkupSecondaryColor() : "ПРОЧНОСТЬ НЕ ОБЕСПЕЧЕНА".MarkupErrorColor())}
                           ";

            AnsiConsole.MarkupLine(summary);
        }
    }
}
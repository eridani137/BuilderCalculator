using System.Reflection;
using Calculators.Shared;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace Calculators.KZH_04
{
    public class CalculateResult : BaseCalculateResult
    {
        private readonly Calculator _calculator;

        public CalculateResult(Calculator calculator)
        {
            _calculator = calculator;
        }

        // Геометрические характеристики
        [Parameter("Приведенный момент инерции, см⁴")] public double I_red { get; set; }
        [Parameter("Приведенный момент сопротивления, см³")] public double W_red { get; set; }
        [Parameter("Пластический момент сопротивления, см³")] public double W_pl { get; set; }
        [Parameter("Эксцентриситет, см")] public double ex { get; set; }
        [Parameter("Момент трещинообразования, кг·см")] public double Mcrc { get; set; }

        // Деформационные характеристики
        [Parameter("Приведенный модуль упругости бетона, кг/см²")] public double Eb_red { get; set; }
        [Parameter("Отношение модулей упругости")] public double alpha_s1 { get; set; }
        [Parameter("Высота сжатой зоны, см")] public double xM { get; set; }
        [Parameter("Высота сжатой зоны с учетом нагрузок, см")] public double xm { get; set; }
        [Parameter("Приведенная площадь, см²")] public double A_red { get; set; }
        [Parameter("Статический момент, см³")] public double St_red { get; set; }
        [Parameter("Расстояние до центра тяжести, см")] public double yc { get; set; }
        [Parameter("Высота растянутой зоны, см")] public double xt { get; set; }

        // Напряжения
        [Parameter("Напряжение в арматуре, кг/см²")] public double sigma_s { get; set; }
        [Parameter("Напряжение в арматуре при трещинообразовании, кг/см²")] public double sigma_s_crc { get; set; }
        [Parameter("Коэффициент")] public double psi_s { get; set; }

        // Ширины раскрытия трещин
        [Parameter("Ширина раскрытия от длительных нагрузок, см")] public double acrc1 { get; set; }
        [Parameter("Ширина раскрытия от полных нагрузок, см")] public double acrc2 { get; set; }
        [Parameter("Ширина раскрытия от кратковременных длительных нагрузок, см")] public double acrc3 { get; set; }

        // Результат проверки
        public bool Result { get; set; }
        
        public override void PrintParameters()
        {
            var type = GetType();
            var properties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            var table = new Table();
            
            table.AddColumn("Параметр");
            table.AddColumn("Обозначение");
            table.AddColumn("Значение");

            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttribute<ParameterAttribute>();
                if (attr == null || !prop.CanRead) continue;
                var value = prop.GetValue(this);
                
                table.AddRow(attr.Name, prop.Name, value.ToString());
            }

            AnsiConsole.Write(table);
        }

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
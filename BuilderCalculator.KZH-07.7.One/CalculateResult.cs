using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace BuilderCalculator.KZH_07._7.One
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

        [OutputParameter("Статический момент относительно оси Y, см3")]
        public double Sy_prime { get; set; }

        [OutputParameter("Статический момент относительно оси X, см3")]
        public double Sx_prime { get; set; }

        [OutputParameter("Размер контура продавливания по оси X, см")]
        public double Lx { get; set; }

        [OutputParameter("Размер контура продавливания по оси Y, см")]
        public double Ly { get; set; }

        [OutputParameter("Координата центра тяжести по X, см")]
        public double xc { get; set; }

        [OutputParameter("Координата центра тяжести по Y, см")]
        public double yc { get; set; }

        [OutputParameter("Момент инерции относительно оси X, см4")]
        public double Ibx { get; set; }

        [OutputParameter("Момент инерции относительно оси Y, см4")]
        public double Iby { get; set; }

        [OutputParameter("Момент сопротивления по оси X, см3")]
        public double Wbx { get; set; }

        [OutputParameter("Момент сопротивления по оси Y, см3")]
        public double Wby { get; set; }

        [OutputParameter("Предельная несущая способность бетона, кгс")]
        public double Fb_ult { get; set; }

        [OutputParameter("Предельный момент по оси X, кгс·см")]
        public double Mbx_ult { get; set; }

        [OutputParameter("Предельный момент по оси Y, кгс·см")]
        public double Mby_ult { get; set; }

        [OutputParameter("Интенсивность армирования, кгс/см")]
        public double q_sw { get; set; }

        [OutputParameter("Предельная несущая способность арматуры, кгс")]
        public double Fsw_ult { get; set; }

        [OutputParameter("Суммарная несущая способность, кгс")]
        public double F_ult { get; set; }

        [OutputParameter("Момент сопротивления арматуры по оси X, см3")]
        public double Wsw_x { get; set; }

        [OutputParameter("Момент сопротивления арматуры по оси Y, см3")]
        public double Wsw_y { get; set; }

        [OutputParameter("Предельный момент арматуры по оси X, кгс·см")]
        public double Msw_x_ult { get; set; }

        [OutputParameter("Предельный момент арматуры по оси Y, кгс·см")]
        public double Msw_y_ult { get; set; }

        [OutputParameter("Суммарный предельный момент по оси X, кгс·см")]
        public double Mx_ult { get; set; }

        [OutputParameter("Суммарный предельный момент по оси Y, кгс·см")]
        public double My_ult { get; set; }

        public bool Result { get; set; }

        public override void PrintSummary()
        {
            if (!(Calculator is Calculator calculator))
            {
                throw new ApplicationException("Задан неверный тип калькулятора");
            }

            var summary = $@"
===== РЕЗУЛЬТАТЫ РАСЧЕТА НА ПРОДАВЛИВАНИЕ =====
Предельная несущая способность: {F_ult:F2} кгс
Предельный момент по оси X: {Mx_ult:F2} кгс·см
Предельный момент по оси Y: {My_ult:F2} кгс·см
Статус проверки: {(Result ? "ПРОЧНОСТЬ ОБЕСПЕЧЕНА".MarkupSecondaryColor() : "ПРОЧНОСТЬ НЕ ОБЕСПЕЧЕНА".MarkupErrorColor())}
";
            AnsiConsole.MarkupLine(summary);
        }
    }
}
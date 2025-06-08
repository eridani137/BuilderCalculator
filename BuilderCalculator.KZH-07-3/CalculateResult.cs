using System;
using Calculators.Shared;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Exceptions;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace BuilderCalculator.KZH_07_3
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }
        
        [OutputParameter("Рабочая высота сечения, см")]
        public double H0 { get; set; }

        [OutputParameter("Предельная продавливающая сила от бетона, кг")]
        public double Fb_ult { get; set; }

        [OutputParameter("Момент инерции контура продавливания относительно оси x, см4")]
        public double Ibx { get; set; }

        [OutputParameter("Момент инерции контура продавливания относительно оси y, см4")]
        public double Iby { get; set; }

        [OutputParameter("Момент сопротивления контура продавливания относительно оси x, см3")]
        public double Wbx { get; set; }

        [OutputParameter("Момент сопротивления контура продавливания относительно оси y, см3")]
        public double Wby { get; set; }

        [OutputParameter("Предельный изгибающий момент от бетона относительно оси x, кг·см")]
        public double Mbx_ult { get; set; }

        [OutputParameter("Предельный изгибающий момент от бетона относительно оси y, кг·см")]
        public double Mby_ult { get; set; }

        [OutputParameter("Интенсивность поперечной арматуры, кг/см")]
        public double qsw { get; set; }

        [OutputParameter("Предельная продавливающая сила от поперечной арматуры, кг")]
        public double Fsw_ult { get; set; }

        [OutputParameter("Общая предельная продавливающая сила, кг")]
        public double F_ult { get; set; }

        [OutputParameter("Предельный изгибающий момент от поперечной арматуры относительно оси x, кг·см")]
        public double Msw_x_ult { get; set; }

        [OutputParameter("Предельный изгибающий момент от поперечной арматуры относительно оси y, кг·см")]
        public double Msw_y_ult { get; set; }

        [OutputParameter("Общий предельный изгибающий момент относительно оси x, кг·см")]
        public double Mx_ult { get; set; }

        [OutputParameter("Общий предельный изгибающий момент относительно оси y, кг·см")]
        public double My_ult { get; set; }

        [OutputParameter("Результат")]
        public bool Result { get; set; }
    }
}
using System;
using System.Text;
using Calculators.Shared;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Exceptions;
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

        [OutputParameter("Результат")]
        public bool Result { get; set; }
    }
}
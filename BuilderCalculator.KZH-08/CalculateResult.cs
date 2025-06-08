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
    
        [OutputParameter("Результат")]
        public bool Result { get; set; }
    }
}
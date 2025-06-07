using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;

namespace BuilderCalculator.KZH_09
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }
        
        [OutputParameter("Модуль упругости бетона, кг/см^2")]
        public double EbRed { get; set; }

        [OutputParameter("μs αs2")]
        public double MuSAlphaS2 { get; set; }

        [OutputParameter("Высота сжатой зоны, см")]
        public double Xm { get; set; }

        [OutputParameter("Расстояние от центра тяжести арматуры до нейтральной оси, см")]
        public double Z { get; set; }

        [OutputParameter("Жесткость, кг·см²")]
        public double D { get; set; }

        [OutputParameter("(1/r)1, 1/см")]
        public double OneOverR1 { get; set; }

        [OutputParameter("(1/r)2, 1/см")]
        public double OneOverR2 { get; set; }

        [OutputParameter("(1/r)3, 1/см")]
        public double OneOverR3 { get; set; }

        [OutputParameter("Полная кривизна, 1/см")]
        public double OneOverR { get; set; }

        [OutputParameter("Прогиб, см")]
        public double F { get; set; }

        [OutputParameter("Результат (true, если прогиб ≤ предельный прогиб)")]
        public bool Result { get; set; }
    }
}
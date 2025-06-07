using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;

namespace BuilderCalculator.KZH_07._5
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }

        [OutputParameter("Площадь контура продавливания, см^2")]
        public double Ab { get; set; }

        [OutputParameter("Момент инерции по оси x, см^3")]
        public double Ibx { get; set; }

        [OutputParameter("Момент инерции по оси y, см^3")]
        public double Iby { get; set; }

        [OutputParameter("Момент сопротивления по оси x, см^2")]
        public double Wbx { get; set; }

        [OutputParameter("Момент сопротивления по оси y, см^2")]
        public double Wby { get; set; }

        [OutputParameter("Статический момент по оси x или y, см^2")]
        public double Sx { get; set; }

        [OutputParameter("Координата центра тяжести по оси x или y, см")]
        public double Xc { get; set; }

        [OutputParameter("Результирующий момент по оси x, кг·см")]
        public double MxResult { get; set; }

        [OutputParameter("Результирующий момент по оси y, кг·см")]
        public double MyResult { get; set; }

        [OutputParameter("Несущая способность бетона, кг")]
        public double FbUlt { get; set; }

        [OutputParameter("Несущая способность бетона по моменту x, кг·см")]
        public double MbxUlt { get; set; }

        [OutputParameter("Несущая способность бетона по моменту y, кг·см")]
        public double MbyUlt { get; set; }

        [OutputParameter("Усилие в поперечной арматуре на единицу длины, кг/см")]
        public double Qsw { get; set; }

        [OutputParameter("Несущая способность арматуры, кг")]
        public double FswUlt { get; set; }

        [OutputParameter("Полная несущая способность, кг")]
        public double Fult { get; set; }

        [OutputParameter("Несущая способность арматуры по моменту x, кг·см")]
        public double MswxUlt { get; set; }

        [OutputParameter("Несущая способность арматуры по моменту y, кг·см")]
        public double MswyUlt { get; set; }

        [OutputParameter("Полная несущая способность по моменту x, кг·см")]
        public double MxUlt { get; set; }

        [OutputParameter("Полная несущая способность по моменту y, кг·см")]
        public double MyUlt { get; set; }

        [OutputParameter("Результат проверки прочности: true - прочность обеспечена")]
        public bool Result { get; set; }
    }
}
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;

namespace BuilderCalculator.KZH_07._6
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }

        [OutputParameter("Площадь первого момента по оси X, см²")]
        public double Sx { get; set; }

        [OutputParameter("Площадь первого момента по оси Y, см²")]
        public double Sy { get; set; }

        [OutputParameter("Координата центра тяжести по оси X, см")]
        public double Xc { get; set; }

        [OutputParameter("Координата центра тяжести по оси Y, см")]
        public double Yc { get; set; }

        [OutputParameter("Момент инерции относительно оси X, см³")]
        public double Ibx { get; set; }

        [OutputParameter("Момент инерции относительно оси Y, см³")]
        public double Iby { get; set; }

        [OutputParameter("Момент сопротивления по оси X, см²")]
        public double Wbx { get; set; }

        [OutputParameter("Момент сопротивления по оси Y, см²")]
        public double Wby { get; set; }

        [OutputParameter("Расчетный момент по оси X, кг·см")]
        public double MxCalc { get; set; }

        [OutputParameter("Расчетный момент по оси Y, кг·см")]
        public double MyCalc { get; set; }

        [OutputParameter("Предельная сила бетона, кг")]
        public double FbUlt { get; set; }

        [OutputParameter("Предельный момент бетона по оси X, кг·см")]
        public double MbxUlt { get; set; }

        [OutputParameter("Предельный момент бетона по оси Y, кг·см")]
        public double MbyUlt { get; set; }

        [OutputParameter("Усилие в поперечной арматуре на единицу длины, кг/см")]
        public double Qsw { get; set; }

        [OutputParameter("Предельная сила поперечной арматуры, кг")]
        public double FswUlt { get; set; }

        [OutputParameter("Общая предельная сила, кг")]
        public double Fult { get; set; }

        [OutputParameter("Предельный момент арматуры по оси X, кг·см")]
        public double MswXUlt { get; set; }

        [OutputParameter("Предельный момент арматуры по оси Y, кг·см")]
        public double MswYUlt { get; set; }

        [OutputParameter("Общий предельный момент по оси X, кг·см")]
        public double MxUlt { get; set; }

        [OutputParameter("Общий предельный момент по оси Y, кг·см")]
        public double MyUlt { get; set; }

        [OutputParameter("Результат проверки прочности (true - обеспечена, false - не обеспечена)")]
        public bool Result { get; set; }
    }
}
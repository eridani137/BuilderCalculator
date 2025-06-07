using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;

namespace BuilderCalculator.KZH_05._1
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }

        [OutputParameter("Рабочая высота сечения (ось X), см")]
        public double h0x { get; set; }

        [OutputParameter("Расчетная высота сжатой зоны (ось X), см")]
        public double h0px { get; set; }

        [OutputParameter("Рабочая высота сечения (ось Y), см")]
        public double h0y { get; set; }

        [OutputParameter("Расчетная высота сжатой зоны (ось Y), см")]
        public double h0py { get; set; }

        [OutputParameter("Относительная высота сжатой зоны (ось X)")]
        public double alpha_nx { get; set; }

        [OutputParameter("Коэффициент армирования (ось X)")]
        public double alpha_sx { get; set; }

        [OutputParameter("Граничное значение относительной высоты сжатой зоны")]
        public double xi_R { get; set; }

        [OutputParameter("Относительная высота сжатой зоны (ось X)")]
        public double xi_x { get; set; }

        [OutputParameter("Относительная высота сжатой зоны (ось Y)")]
        public double alpha_ny { get; set; }

        [OutputParameter("Коэффициент армирования (ось Y)")]
        public double alpha_sy { get; set; }

        [OutputParameter("Относительная высота сжатой зоны (ось Y)")]
        public double xi_y { get; set; }

        [OutputParameter("Предельный момент (ось X), кг·см")]
        public double M0x { get; set; }

        [OutputParameter("Предельный момент (ось Y), кг·см")]
        public double M0y { get; set; }

        [OutputParameter("Суммарная площадь арматуры, см^2")]
        public double Astot { get; set; }

        [OutputParameter("Общий коэффициент армирования")]
        public double alpha_s { get; set; }

        [OutputParameter("Базовый коэффициент")]
        public double k0 { get; set; }

        [OutputParameter("Относительная высота сжатой зоны")]
        public double alpha_n { get; set; }

        [OutputParameter("Расчетный коэффициент")]
        public double k { get; set; }

        [OutputParameter("Результат проверки прочности")]
        public bool Result { get; set; }
    }
}
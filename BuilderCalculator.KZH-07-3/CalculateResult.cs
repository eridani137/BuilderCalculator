using Calculators.Shared;

namespace BuilderCalculator.KZH_07_3
{
    public class CalculateResult : BaseCalculateResult
    {
        public double H0 { get; set; }
        public double Fb_ult { get; set; }
        public double Ibx { get; set; }
        public double Iby { get; set; }
        public double Wbx { get; set; }
        public double Wby { get; set; }
        public double Mbx_ult { get; set; }
        public double Mby_ult { get; set; }
        public double qsw { get; set; }
        public double Fsw_ult { get; set; }
        public double F_ult { get; set; }
        public double Msw_x_ult { get; set; }
        public double Msw_y_ult { get; set; }
        public double Mx_ult { get; set; }
        public double My_ult { get; set; }
        
        public bool Result { get; set; }
        
        public CalculateResult(Calculator calculator) : base(calculator)
        {
            Calculator = calculator;
        }
        
        public override void PrintSummary()
        {
            throw new System.NotImplementedException();
        }
    }
}
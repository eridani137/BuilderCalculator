using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_07_3
{
    public class BearingCapacityPunching : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; set; }

        public BearingCapacityPunching()
        {
            CalculateResult = new CalculateResult(this);
        }

        public BearingCapacityPunching(double f, bool considerBendingMoments, double mx, double my,
            bool divideMomentsByTwo, bool considerSoilReaction, double p, double sizeX, double sizeY, double h,
            double a, ConcreteClass concreteClass, double gammaB, bool considerShearReinforcement,
            ReinforcementClass reinforcementClass, double asw, double sw)
        {
            CalculateResult = new CalculateResult(this);
            F = f;
            ConsiderBendingMoments = considerBendingMoments;
            Mx = mx;
            My = my;
            DivideMomentsByTwo = divideMomentsByTwo;
            ConsiderSoilReaction = considerSoilReaction;
            this.p = p;
            SizeX = sizeX;
            SizeY = sizeY;
            this.h = h;
            this.a = a;
            ConcreteClass = concreteClass;
            GammaB = gammaB;
            ConsiderShearReinforcement = considerShearReinforcement;
            ReinforcementClass = reinforcementClass;
            Asw = asw;
            Sw = sw;
        }

        [InputParameter("Сосредоточенная сила от внешней нагрузки, кг")]
        public double F { get; set; } = 20000;

        [InputParameter("Учесть изгибающие моменты")]
        public bool ConsiderBendingMoments { get; set; } = true;

        [InputParameter("Изгибающий момент вдоль оси x, кг·см")]
        public double Mx { get; set; } = 100000;

        [InputParameter("Изгибающий момент вдоль оси y, кг·см")]
        public double My { get; set; } = 120000;

        [InputParameter("Делить изгибающие моменты пополам")]
        public bool DivideMomentsByTwo { get; set; } = true;

        [InputParameter("Учесть отпор грунта под плитой")]
        public bool ConsiderSoilReaction { get; set; } = true;

        [InputParameter("Отпор грунта под плитой, кгс/см2")]
        public double p { get; set; } = 1.0;

        [InputParameter("Ширина зоны приложения нагрузки (b_cx), см")]
        public double SizeX { get; set; } = 50.0;

        [InputParameter("Длина зоны приложения нагрузки (a_cy), см")]
        public double SizeY { get; set; } = 40.0;

        [InputParameter("Высота сечения, см")] public double h { get; set; } = 20.0;

        [InputParameter("Защитный слой бетона растянутой зоны, см")]
        public double a { get; set; } = 5.0;

        [InputParameter("Класс бетона")]
        public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B50;

        [InputParameter("Коэффициент условий работы бетона")]
        public double GammaB { get; set; } = 0.9;

        [InputParameter("Учесть поперечную арматуру")]
        public bool ConsiderShearReinforcement { get; set; } = true;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.B500;

        [InputParameter("Площадь поперечной арматуры с шагом, см2")]
        public double Asw { get; set; } = 1.06;

        [InputParameter("Шаг поперечной арматуры, см")]
        public double Sw { get; set; } = 10.0;

        public override BaseCalculateResult Calculate()
        {
            ValidateInputParameters();
            CalculateResult.H0 = Math.Max(0, h - a);

            double originalF = F;
            double currentF = F;
            double currentMx = Math.Abs(Mx);
            double currentMy = Math.Abs(My);

            if (ConsiderBendingMoments && DivideMomentsByTwo)
            {
                currentMx /= 2.0;
                currentMy /= 2.0;
            }

            if (ConsiderSoilReaction)
            {
                double Fp = p * (SizeY + 2 * CalculateResult.H0) * (SizeX + 2 * CalculateResult.H0);
                Fp = Math.Min(Fp, currentF);
                currentF -= Fp;
            }

            double Rbt = ConcreteClass.GetRbt(GammaB);
            double Rsw = ConsiderShearReinforcement ? ReinforcementClass.GetRsw() : 0;

            double u = 2 * (SizeX + SizeY + 2 * CalculateResult.H0);
            double Ab = u * CalculateResult.H0;
            CalculateResult.Fb_ult = Rbt * Ab;

            if (ConsiderBendingMoments)
            {
                double Lx = SizeX + CalculateResult.H0;
                double Ly = SizeY + CalculateResult.H0;

                double Ibx1 = Math.Pow(Lx, 3) / 6.0;
                double Iby1 = Math.Pow(Ly, 3) / 6.0;
                double Ibx2 = 0.5 * Ly * Math.Pow(Lx, 2);
                double Iby2 = 0.5 * Lx * Math.Pow(Ly, 2);

                CalculateResult.Ibx = Ibx1 + Ibx2;
                CalculateResult.Iby = Iby1 + Iby2;

                CalculateResult.Wbx = CalculateResult.Ibx / (Lx / 2);
                CalculateResult.Wby = CalculateResult.Iby / (Ly / 2);

                CalculateResult.Mbx_ult = Rbt * CalculateResult.Wbx * CalculateResult.H0;
                CalculateResult.Mby_ult = Rbt * CalculateResult.Wby * CalculateResult.H0;
            }

            CalculateResult.qsw = 0;
            CalculateResult.Fsw_ult = 0;
            CalculateResult.Msw_x_ult = 0;
            CalculateResult.Msw_y_ult = 0;

            if (ConsiderShearReinforcement)
            {
                CalculateResult.qsw = Rsw * Asw / Sw;
                CalculateResult.Fsw_ult = 0.8 * CalculateResult.qsw * u;

                if (CalculateResult.Fsw_ult < 0.25 * CalculateResult.Fb_ult)
                {
                    CalculateResult.Fsw_ult = 0;
                }
                else
                {
                    CalculateResult.Fsw_ult = Math.Min(CalculateResult.Fsw_ult, CalculateResult.Fb_ult);
                }

                if (ConsiderBendingMoments)
                {
                    CalculateResult.Msw_x_ult = 0.8 * CalculateResult.qsw * CalculateResult.Wbx;
                    CalculateResult.Msw_y_ult = 0.8 * CalculateResult.qsw * CalculateResult.Wby;

                    CalculateResult.Msw_x_ult = Math.Min(CalculateResult.Msw_x_ult, CalculateResult.Mbx_ult);
                    CalculateResult.Msw_y_ult = Math.Min(CalculateResult.Msw_y_ult, CalculateResult.Mby_ult);
                }
            }

            CalculateResult.F_ult = CalculateResult.Fb_ult + CalculateResult.Fsw_ult;

            if (ConsiderBendingMoments)
            {
                CalculateResult.Mx_ult = CalculateResult.Mbx_ult + CalculateResult.Msw_x_ult;
                CalculateResult.My_ult = CalculateResult.Mby_ult + CalculateResult.Msw_y_ult;
            }

            if (!ConsiderBendingMoments)
            {
                CalculateResult.Result = (currentF <= CalculateResult.F_ult);
            }
            else
            {
                double denominator = ConsiderShearReinforcement ? CalculateResult.F_ult : CalculateResult.Fb_ult;
                double term1 = (currentMx / CalculateResult.Mx_ult) + (currentMy / CalculateResult.My_ult);
                double term2 = currentF / (2 * denominator);
                double sum = (currentF / denominator) + term1;

                CalculateResult.Result = (term1 <= term2) && (sum <= 1.0);
            }

            F = originalF;

            return CalculateResult;
        }

        private void ValidateInputParameters()
        {
            if (F <= 0) throw new ArgumentException("Сила F должна быть положительной");
            if (a >= h) throw new ArgumentException("Защитный слой 'a' не может быть больше высоты сечения 'h'");
            if (SizeX <= 0 || SizeY <= 0)
            {
                throw new ArgumentException("Размеры зоны приложения нагрузки должны быть положительными");
            }

            if (h <= 0) throw new ArgumentException("Высота сечения должна быть положительной");
            if (GammaB <= 0) throw new ArgumentException("Коэффициент условий работы бетона должен быть положительным");
            if (ConsiderShearReinforcement && (Asw <= 0 || Sw <= 0))
            {
                throw new ArgumentException("Параметры арматуры должны быть положительными");
            }
        }
    }
}
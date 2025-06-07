using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_07._5
{
    public class BearingCapacityPunchingColumnOnEdge : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; }

        public BearingCapacityPunchingColumnOnEdge()
        {
            CalculateResult = new CalculateResult(this);
        }

        public BearingCapacityPunchingColumnOnEdge(double xaYa, double f, int fDirection, double mx, double my, bool divideMoments, bool divideEccentricityMoments, double ac, double bc, double h, double a, ConcreteClass concreteClass, double gammaBi, ReinforcementClass reinforcementClass, double asw, double sw, int orientation, bool calculatePoints)
        {
            CalculateResult = new CalculateResult(this);
            XaYa = xaYa;
            F = f;
            FDirection = fDirection;
            Mx = mx;
            My = my;
            DivideMoments = divideMoments;
            DivideEccentricityMoments = divideEccentricityMoments;
            Ac = ac;
            Bc = bc;
            H = h;
            A = a;
            ConcreteClass = concreteClass;
            GammaBi = gammaBi;
            ReinforcementClass = reinforcementClass;
            Asw = asw;
            Sw = sw;
            Orientation = orientation;
            CalculatePoints = calculatePoints;
        }

        [InputParameter("Расстояние от центра колонны до края плиты, см")]
        public double XaYa { get; set; } = 40.0;

        [InputParameter("Сосредоточенная сила, кг")]
        public double F { get; set; } = 20000;

        [InputParameter("Направление усилия F: 0 - снизу вверх, 1 - сверху вниз")]
        public int FDirection { get; set; } = 1;

        [InputParameter("Изгибающий момент вдоль оси x, кг·см")]
        public double Mx { get; set; } = 100000;

        [InputParameter("Изгибающий момент вдоль оси y, кг·см")]
        public double My { get; set; } = 120000;

        [InputParameter("Делить изгибающие моменты пополам")]
        public bool DivideMoments { get; set; } = false;

        [InputParameter("Делить дополнительные моменты от эксцентриситета пополам")]
        public bool DivideEccentricityMoments { get; set; } = false;

        [InputParameter("Высота зоны приложения нагрузки, см")]
        public double Ac { get; set; } = 40.0;

        [InputParameter("Ширина зоны приложения нагрузки, см")]
        public double Bc { get; set; } = 50.0;

        [InputParameter("Толщина плиты, см")] public double H { get; set; } = 20.0;

        [InputParameter("Защитный слой бетона растянутой зоны, см")]
        public double A { get; set; } = 5.0;

        [InputParameter("Класс бетона")] public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B25;

        [InputParameter("Коэффициент условий работы бетона")]
        public double GammaBi { get; set; } = 0.9;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A500;

        [InputParameter("Площадь поперечной арматуры с шагом sw, см²")]
        public double Asw { get; set; } = 1.06;

        [InputParameter("Шаг поперечной арматуры, см")]
        public double Sw { get; set; } = 10.0;

        [InputParameter("Ориентация площадки относительно края: 1, 2, 3 или 4")]
        public int Orientation { get; set; } = 1;

        [InputParameter("Выполнять расчет для отдельных точек контура с учетом знаков моментов")]
        public bool CalculatePoints { get; set; } = false;

        public override BaseCalculateResult Calculate()
        {
            double mx = DivideMoments ? Mx / 2 : Mx;
            double my = DivideMoments ? My / 2 : My;
            double f = FDirection == 1 ? -F : F;
            double H0 = H - A;
            double Rbt = ConcreteClass.GetRbt();
            double Rsw = ReinforcementClass.GetRsw();

            double lx, ly, u, sxSy, xcYc, ibx, iby, wbx, wby;

            if (Orientation == 1 || Orientation == 2)
            {
                lx = XaYa + Bc / 2 + H0 / 2;
                ly = Ac + H0;
                u = 2 * lx + ly;
                CalculateResult.Ab = u * H0;
                sxSy = 2 * lx * (lx / 2 - XaYa) + ly * (Bc + H0) / 2;
                xcYc = sxSy / u;
                ibx = (lx * lx * lx) / 6 + 2 * lx * Math.Pow(XaYa + xcYc - lx / 2, 2) +
                      ly * Math.Pow(lx - XaYa - xcYc, 2);
                iby = (ly * ly * ly) / 12 + (lx * ly * ly) / 2;
                wbx = ibx / (lx - XaYa);
                wby = iby / (ly / 2);
            }
            else
            {
                ly = XaYa + Ac / 2 + H0 / 2;
                lx = Bc + H0;
                u = 2 * ly + lx;
                CalculateResult.Ab = u * H0;
                sxSy = 2 * ly * (XaYa - ly / 2) - lx * (Ac + H0) / 2;
                xcYc = sxSy / u;
                iby = (ly * ly * ly) / 6 + 2 * ly * Math.Pow(XaYa - xcYc - ly / 2, 2) +
                      lx * Math.Pow(ly - XaYa + xcYc, 2);
                ibx = (lx * lx * lx) / 12 + (ly * lx * lx) / 2;
                wbx = ibx / (lx / 2);
                wby = iby / (ly - XaYa);
            }

            CalculateResult.Sx = sxSy;
            CalculateResult.Xc = xcYc;
            CalculateResult.Ibx = ibx;
            CalculateResult.Iby = iby;
            CalculateResult.Wbx = wbx;
            CalculateResult.Wby = wby;

            double eccentricityX = DivideEccentricityMoments ? CalculateResult.Xc / 2 : CalculateResult.Xc;
            double eccentricityY = DivideEccentricityMoments ? CalculateResult.Xc / 2 : CalculateResult.Xc;
            CalculateResult.MxResult = Math.Abs(mx + f * eccentricityX);
            CalculateResult.MyResult = Math.Abs(my + f * eccentricityY);

            CalculateResult.FbUlt = Rbt * CalculateResult.Ab;
            CalculateResult.MbxUlt = Rbt * CalculateResult.Wbx * H0;
            CalculateResult.MbyUlt = Rbt * CalculateResult.Wby * H0;
            CalculateResult.Qsw = Rsw * Asw / Sw;
            CalculateResult.FswUlt = 0.8 * CalculateResult.Qsw * u;

            if (CalculateResult.FswUlt > CalculateResult.FbUlt)
                CalculateResult.FswUlt = CalculateResult.FbUlt;

            CalculateResult.Fult = CalculateResult.FbUlt + CalculateResult.FswUlt;
            CalculateResult.MswxUlt = 0.8 * CalculateResult.Qsw * CalculateResult.Wbx;
            CalculateResult.MswyUlt = 0.8 * CalculateResult.Qsw * CalculateResult.Wby;

            if (CalculateResult.MswxUlt > CalculateResult.MbxUlt)
                CalculateResult.MswxUlt = CalculateResult.MbxUlt;

            if (CalculateResult.MswyUlt > CalculateResult.MbyUlt)
                CalculateResult.MswyUlt = CalculateResult.MbyUlt;

            CalculateResult.MxUlt = CalculateResult.MbxUlt + CalculateResult.MswxUlt;
            CalculateResult.MyUlt = CalculateResult.MbyUlt + CalculateResult.MswyUlt;

            double check1 = CalculateResult.MxResult / CalculateResult.MxUlt + CalculateResult.MyResult / CalculateResult.MyUlt;
            double check2 = Math.Abs(F) / (2 * CalculateResult.Fult);
            double finalCheck = Math.Abs(F) / CalculateResult.Fult + check1;

            CalculateResult.Result = finalCheck <= 1;

            return CalculateResult;
        }
    }
}
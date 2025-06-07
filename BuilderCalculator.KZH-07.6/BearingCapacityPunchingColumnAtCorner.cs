using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_07._6
{
    public class BearingCapacityPunchingColumnAtCorner : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; }

        public BearingCapacityPunchingColumnAtCorner()
        {
            CalculateResult = new CalculateResult(this);
        }

        public BearingCapacityPunchingColumnAtCorner(int orientPl, double xa, double ya, double f, int directionF, double mx, double my, bool divideMoments, bool divideEccentricityMoments, double ac, double bc, double h, double a, ConcreteClass concreteClass, double gammaBi, ReinforcementClass reinforcementClass, double asw, double sw, bool checkIndividualPoints)
        {
            CalculateResult = new CalculateResult(this);
            OrientPl = orientPl;
            Xa = xa;
            Ya = ya;
            F = f;
            DirectionF = directionF;
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
            CheckIndividualPoints = checkIndividualPoints;
        }

        [InputParameter("Ориентация площадки относительно угла (1, 2, 3, 4)")]
        public int OrientPl { get; set; } = 1;

        [InputParameter("Расстояние от центра колонны до края плиты по оси X, см")]
        public double Xa { get; set; } = 40.0;

        [InputParameter("Расстояние от центра колонны до края плиты по оси Y, см")]
        public double Ya { get; set; } = 40.0;

        [InputParameter("Сосредоточенная сила, кг")]
        public double F { get; set; } = 20000;

        [InputParameter("Направление усилия F (0 - снизу вверх, 1 - сверху вниз)")]
        public int DirectionF { get; set; } = 1;

        [InputParameter("Изгибающий момент вдоль оси X, кг·см")]
        public double Mx { get; set; } = 100000;

        [InputParameter("Изгибающий момент вдоль оси Y, кг·см")]
        public double My { get; set; } = 120000;

        [InputParameter("Делить изгибающие моменты пополам")]
        public bool DivideMoments { get; set; } = true;

        [InputParameter("Делить дополнительные моменты пополам (от эксцентриситета)")]
        public bool DivideEccentricityMoments { get; set; } = true;

        [InputParameter("Высота зоны приложения нагрузки, см (вдоль оси Y)")]
        public double Ac { get; set; } = 40.0;

        [InputParameter("Ширина зоны приложения нагрузки, см (вдоль оси X)")]
        public double Bc { get; set; } = 50.0;

        [InputParameter("Толщина плиты, см")] public double H { get; set; } = 20.0;

        [InputParameter("Защитный слой бетона растянутой зоны, см")]
        public double A { get; set; } = 5.0;

        [InputParameter("Класс бетона")] public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B25;

        [InputParameter("Коэффициент условий работы бетона")]
        public double GammaBi { get; set; } = 0.9;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A500;

        [InputParameter("Площадь поперечной арматуры, см²")]
        public double Asw { get; set; } = 1.06;

        [InputParameter("Шаг поперечной арматуры, см")]
        public double Sw { get; set; } = 10.0;

        [InputParameter("Выполнять расчет для отдельных точек контура")]
        public bool CheckIndividualPoints { get; set; } = true;

        public override BaseCalculateResult Calculate()
        {
            // Рабочая высота сечения
            double h0 = H - A;

            // 1. Геометрические характеристики контура продавливания
            double Lx = Xa + Bc / 2 + h0 / 2;
            double Ly = Ya + Ac / 2 + h0 / 2;
            double u = Lx + Ly;
            double Ab = u * h0;

            // Расчет Sx и Sy в зависимости от ориентации
            switch (OrientPl)
            {
                case 1:
                    CalculateResult.Sx = Lx * (Lx / 2 - Xa) + Ly * (Bc + h0) / 2;
                    CalculateResult.Sy = Ly * (Ya - Ly / 2) - Lx * (Ac + h0) / 2;
                    break;
                case 2:
                    CalculateResult.Sx = Lx * (Xa - Lx / 2) - Ly * (Bc + h0) / 2;
                    CalculateResult.Sy = Ly * (Ya - Ly / 2) - Lx * (Ac + h0) / 2;
                    break;
                case 3:
                    CalculateResult.Sx = Lx * (Xa - Lx / 2) - Ly * (Bc + h0) / 2;
                    CalculateResult.Sy = Ly * (Ly / 2 - Ya) + Lx * (Ac + h0) / 2;
                    break;
                case 4:
                    CalculateResult.Sx = Lx * (Lx / 2 - Xa) + Ly * (Bc + h0) / 2;
                    CalculateResult.Sy = Ly * (Ly / 2 - Ya) + Lx * (Ac + h0) / 2;
                    break;
                default:
                    throw new ArgumentException("Недопустимое значение OrientPl");
            }

            CalculateResult.Xc = CalculateResult.Sx / u;
            CalculateResult.Yc = CalculateResult.Sy / u;

            // Моменты инерции
            CalculateResult.Ibx = (Math.Pow(Lx, 3) / 12) + Lx * Math.Pow(Xa + CalculateResult.Xc - Lx / 2, 2) + Ly * Math.Pow(Lx - Xa - CalculateResult.Xc, 2);
            CalculateResult.Iby = (Math.Pow(Ly, 3) / 12) + Ly * Math.Pow(Ya - CalculateResult.Yc - Ly / 2, 2) + Lx * Math.Pow(Ly - Ya + CalculateResult.Yc, 2);

            // 2. Учет эксцентриситета
            double mxInput = DivideMoments ? Mx / 2 : Mx;
            double myInput = DivideMoments ? My / 2 : My;
            double eccentricityX = F * CalculateResult.Xc * (DirectionF == 0 ? 1 : -1);
            double eccentricityY = F * CalculateResult.Yc * (DirectionF == 0 ? 1 : -1);
            CalculateResult.MxCalc = mxInput + (DivideEccentricityMoments ? eccentricityX / 2 : eccentricityX);
            CalculateResult.MyCalc = myInput + (DivideEccentricityMoments ? eccentricityY / 2 : eccentricityY);

            // 3. Расчет напряжений в точках (если выбрано)
            double xMax = Math.Abs(CalculateResult.Xc) + Lx / 2;
            double yMax = Math.Abs(CalculateResult.Yc) + Ly / 2;
            if (CheckIndividualPoints)
            {
                double[] xPoints = { -xMax, xMax, xMax, -xMax };
                double[] yPoints = { -yMax, -yMax, yMax, yMax };
                double maxTau = double.MinValue;
                int maxPointIndex = -1;

                for (int i = 0; i < 4; i++)
                {
                    double tau = (F / (u * h0)) - (CalculateResult.MxCalc * xPoints[i] / (CalculateResult.Ibx * h0)) -
                                 (CalculateResult.MyCalc * yPoints[i] / (CalculateResult.Iby * h0));
                    if (tau > maxTau && (i == 1 || i == 2 || i == 3)) // Точка 5 (i=0) вне контура
                    {
                        maxTau = tau;
                        maxPointIndex = i;
                    }
                }

                CalculateResult.Wbx = -CalculateResult.Ibx / xPoints[maxPointIndex];
                CalculateResult.Wby = -CalculateResult.Iby / yPoints[maxPointIndex];
            }
            else
            {
                CalculateResult.Wbx = CalculateResult.Ibx / xMax;
                CalculateResult.Wby = CalculateResult.Iby / yMax;
            }

            // 4. Компоненты предельной несущей способности
            double Rbt = ConcreteClass.GetRbt() * GammaBi;
            double Rsw = ReinforcementClass.GetRsw();

            CalculateResult.FbUlt = Rbt * Ab;
            CalculateResult.MbxUlt = Rbt * CalculateResult.Wbx * h0;
            CalculateResult.MbyUlt = Rbt * CalculateResult.Wby * h0;

            CalculateResult.Qsw = Rsw * Asw / Sw;
            CalculateResult.FswUlt = 0.8 * CalculateResult.Qsw * u;
            if (CalculateResult.FswUlt < 0.25 * CalculateResult.FbUlt) CalculateResult.FswUlt = 0.25 * CalculateResult.FbUlt;
            if (CalculateResult.FswUlt > CalculateResult.FbUlt) CalculateResult.FswUlt = CalculateResult.FbUlt;

            CalculateResult.Fult = CalculateResult.FbUlt + CalculateResult.FswUlt;

            CalculateResult.MswXUlt = 0.8 * CalculateResult.Qsw * CalculateResult.Wbx;
            CalculateResult.MswYUlt = 0.8 * CalculateResult.Qsw * CalculateResult.Wby;
            if (Math.Abs(CalculateResult.MswXUlt) > Math.Abs(CalculateResult.MbxUlt)) CalculateResult.MswXUlt = CalculateResult.MbxUlt;
            if (Math.Abs(CalculateResult.MswYUlt) > Math.Abs(CalculateResult.MbyUlt)) CalculateResult.MswYUlt = CalculateResult.MbyUlt;

            CalculateResult.MxUlt = CalculateResult.MbxUlt + CalculateResult.MswXUlt;
            CalculateResult.MyUlt = CalculateResult.MbyUlt + CalculateResult.MswYUlt;

            // 5. Проверка прочности
            double momentRatio = Math.Abs(CalculateResult.MxCalc / CalculateResult.MxUlt) + Math.Abs(CalculateResult.MyCalc / CalculateResult.MyUlt);
            double forceLimit = Math.Abs(F) / (2 * CalculateResult.Fult);
            double totalRatio = Math.Abs(F) / CalculateResult.Fult + Math.Abs(CalculateResult.MxCalc / CalculateResult.MxUlt) + Math.Abs(CalculateResult.MyCalc / CalculateResult.MyUlt);

            CalculateResult.Result = momentRatio <= forceLimit && totalRatio <= 1;

            return CalculateResult;
        }
    }
}
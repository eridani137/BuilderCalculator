using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_07._7.One
{
    public class Calculator : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; set; }

        public Calculator()
        {
            CalculateResult = new CalculateResult(this);
        }

        public Calculator(double f, double mx, double my, double h, double h0, double aCy, double bCx, double uPrime,
            double ibyPrime, double ibxPrime, double syPrime, double sxPrime, int forceDirection, bool divideMoments,
            bool divideAdditionalMoments, ConcreteClass concreteClass, double gammaBi, bool considerReinforcement,
            ReinforcementClass reinforcementClass, double aSw, double sSw)
        {
            CalculateResult = new CalculateResult(this);
            F = f;
            Mx = mx;
            My = my;
            this.h = h;
            this.h0 = h0;
            a_cy = aCy;
            b_cx = bCx;
            u_prime = uPrime;
            Iby_prime = ibyPrime;
            Ibx_prime = ibxPrime;
            Sy_prime = syPrime;
            Sx_prime = sxPrime;
            ForceDirection = forceDirection;
            DivideMoments = divideMoments;
            DivideAdditionalMoments = divideAdditionalMoments;
            ConcreteClass = concreteClass;
            Gamma_bi = gammaBi;
            ConsiderReinforcement = considerReinforcement;
            ReinforcementClass = reinforcementClass;
            A_sw = aSw;
            s_sw = sSw;
        }

        [InputParameter("Сосредоточенная сила, кгс")]
        public double F { get; set; } = 20000;

        [InputParameter("Изгибающий момент Mx, кгс·см")]
        public double Mx { get; set; } = 100000;

        [InputParameter("Изгибающий момент My, кгс·см")]
        public double My { get; set; } = 120000;

        [InputParameter("Толщина плиты, см")] public double h { get; set; } = 20;

        [InputParameter("Рабочая высота сечения, см")]
        public double h0 { get; set; } = 15;

        [InputParameter("Размер зоны передачи усилия по оси Y, см")]
        public double a_cy { get; set; } = 40;

        [InputParameter("Размер зоны передачи усилия по оси X, см")]
        public double b_cx { get; set; } = 50;

        [InputParameter("Периметр отверстия, см")]
        public double u_prime { get; set; } = 24.9;

        [InputParameter("Момент инерции отверстия относительно оси Y, см⁴")]
        public double Iby_prime { get; set; } = 18443;

        [InputParameter("Момент инерции отверстия относительно оси X, см⁴")]
        public double Ibx_prime { get; set; } = 15022;

        [InputParameter("Статический момент отверстия относительно оси Y, см³")]
        public double Sy_prime { get; set; } = 677.4;

        [InputParameter("Статический момент отверстия относительно оси X, см³")]
        public double Sx_prime { get; set; } = 588.0;

        [InputParameter("Направление усилия F (0 - снизу вверх, 1 - сверху вниз)")]
        public int ForceDirection { get; set; } = 1;

        [InputParameter("Делить изгибающие моменты пополам")]
        public bool DivideMoments { get; set; } = true;

        [InputParameter("Делить дополнительные моменты пополам")]
        public bool DivideAdditionalMoments { get; set; } = true;

        [InputParameter("Класс бетона")] public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B20;

        [InputParameter("Коэффициент условий работы бетона")]
        public double Gamma_bi { get; set; } = 0.90;

        [InputParameter("Учесть поперечную арматуру")]
        public bool ConsiderReinforcement { get; set; } = true;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A400;

        [InputParameter("Площадь поперечной арматуры, см²")]
        public double A_sw { get; set; } = 1.06;

        [InputParameter("Шаг поперечной арматуры, см")]
        public double s_sw { get; set; } = 10.0;

        public override BaseCalculateResult Calculate()
        {
            // Шаг 1: Подготовка исходных данных
            double F_calc = ForceDirection == 0 ? F : -F;
            double Mx_calc = DivideMoments ? Mx / 2.0 : Mx;
            double My_calc = DivideMoments ? My / 2.0 : My;

            // Шаг 2: Расчет геометрических характеристик
            double Lx = b_cx + h0;
            double Ly = a_cy + h0;
            double u = 2 * (Lx + Ly) - u_prime;
            double Ab = u * h0;
            double xc = -Sx_prime / u;
            double yc = -Sy_prime / u;

            // Расчет моментов инерции
            double Ibx = Math.Pow(Lx, 3) / 6.0 + Ly * Math.Pow(Lx, 2) / 2.0 - Ibx_prime - u * Math.Pow(xc, 2);
            double Iby = Math.Pow(Ly, 3) / 6.0 + Lx * Math.Pow(Ly, 2) / 2.0 - Iby_prime - u * Math.Pow(yc, 2);

            // Расчет максимальных расстояний
            double x_max = Math.Max(Math.Abs(-Lx / 2 - xc), Math.Abs(Lx / 2 - xc));
            double y_max = Math.Max(Math.Abs(-Ly / 2 - yc), Math.Abs(Ly / 2 - yc));

            // Моменты сопротивления
            double Wbx = Ibx / x_max;
            double Wby = Iby / y_max;

            // Шаг 3: Учет эксцентриситета
            double divisor = DivideAdditionalMoments ? 2.0 : 1.0;
            double Mx_eff = Math.Abs(Mx_calc + F_calc * xc / divisor);
            double My_eff = Math.Abs(My_calc + F_calc * yc / divisor);
            double F_abs = Math.Abs(F);

            // Шаг 4: Расчет характеристик бетона
            double Rbt = ConcreteClass.GetRbt(Gamma_bi);

            // Шаг 5: Расчет несущей способности
            double Fb_ult = Rbt * Ab;
            double Mbx_ult = Rbt * Wbx * h0;
            double Mby_ult = Rbt * Wby * h0;

            // Шаг 6: Расчет арматуры (если требуется)
            double q_sw = 0.0, Fsw_ult = 0.0, F_ult = Fb_ult;
            double Wsw_x = Wbx, Wsw_y = Wby;
            double Msw_x_ult = 0.0, Msw_y_ult = 0.0;
            double Mx_ult = Mbx_ult, My_ult = Mby_ult;

            if (ConsiderReinforcement)
            {
                q_sw = ReinforcementClass.GetRsw() * A_sw / s_sw;
                Fsw_ult = 0.8 * q_sw * u;

                // Проверка ограничений для Fsw_ult
                if (Fsw_ult < 0.25 * Fb_ult) Fsw_ult = 0.25 * Fb_ult;
                if (Fsw_ult > Fb_ult) Fsw_ult = Fb_ult;

                F_ult = Fb_ult + Fsw_ult;

                // Расчет моментов от арматуры
                Msw_x_ult = 0.8 * q_sw * Wsw_x;
                Msw_y_ult = 0.8 * q_sw * Wsw_y;

                // Проверка ограничений для моментов
                if (Msw_x_ult > Mbx_ult) Msw_x_ult = Mbx_ult;
                if (Msw_y_ult > Mby_ult) Msw_y_ult = Mby_ult;

                Mx_ult = Mbx_ult + Msw_x_ult;
                My_ult = Mby_ult + Msw_y_ult;
            }

            // Шаг 7: Проверка условий прочности
            double condition1 = Mx_eff / Mx_ult + My_eff / My_ult;
            double condition2 = F_abs / F_ult + Mx_eff / Mx_ult + My_eff / My_ult;
            double limit = F_abs / (2 * F_ult);

            bool result = (condition1 <= limit) && (condition2 <= 1.0);
            
            // Заполнение результатов
            CalculateResult.Iby_prime = Iby_prime;
            CalculateResult.Ibx_prime = Ibx_prime;
            CalculateResult.Sy_prime = Sy_prime;
            CalculateResult.Sx_prime = Sx_prime;
            CalculateResult.Lx = Lx;
            CalculateResult.Ly = Ly;
            CalculateResult.xc = xc;
            CalculateResult.yc = yc;
            CalculateResult.Ibx = Ibx;
            CalculateResult.Iby = Iby;
            CalculateResult.Wbx = Wbx;
            CalculateResult.Wby = Wby;
            CalculateResult.Fb_ult = Fb_ult;
            CalculateResult.Mbx_ult = Mbx_ult;
            CalculateResult.Mby_ult = Mby_ult;
            CalculateResult.q_sw = q_sw;
            CalculateResult.Fsw_ult = Fsw_ult;
            CalculateResult.F_ult = F_ult;
            CalculateResult.Wsw_x = Wsw_x;
            CalculateResult.Wsw_y = Wsw_y;
            CalculateResult.Msw_x_ult = Msw_x_ult;
            CalculateResult.Msw_y_ult = Msw_y_ult;
            CalculateResult.Mx_ult = Mx_ult;
            CalculateResult.My_ult = My_ult;
            CalculateResult.Result = result;
            
            return CalculateResult;
        }
    }
}
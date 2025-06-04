using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;

namespace BuilderCalculator.KZH_07._7.Two
{
    public class Calculator : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; set; }

        public Calculator()
        {
            CalculateResult = new CalculateResult(this);
        }

        [InputParameter("Сосредоточенная сила, кгс")]
        public double F { get; set; } = 20000;

        [InputParameter("Направление усилия F (0 - снизу вверх, 1 - сверху вниз)")]
        public int FDirection { get; set; } = 0;

        [InputParameter("Изгибающий момент вдоль оси X, кг·см")]
        public double Mx { get; set; } = 100000;

        [InputParameter("Изгибающий момент вдоль оси Y, кг·см")]
        public double My { get; set; } = 120000;

        [InputParameter("Делить изгибающие моменты пополам")]
        public bool DivideM { get; set; } = false;

        [InputParameter("Делить дополнительные моменты пополам")]
        public bool DivideFe { get; set; } = false;

        [InputParameter("Высота зоны приложения нагрузки, см")]
        public double Acy { get; set; } = 40.0;

        [InputParameter("Ширина зоны приложения нагрузки, см")]
        public double Bcx { get; set; } = 50.0;

        [InputParameter("Толщина плиты, см")] 
        public double H { get; set; } = 20.0;

        [InputParameter("Защитный слой бетона, см")]
        public double A { get; set; } = 5.0;

        [InputParameter("Высота 1-го отверстия, см")]
        public double C1 { get; set; } = 20.0;

        [InputParameter("Ширина 1-го отверстия, см")]
        public double D1 { get; set; } = 30.0;

        [InputParameter("Привязка 1-го отверстия по X, см")]
        public double Dx1 { get; set; } = -35.0;

        [InputParameter("Привязка 1-го отверстия по Y, см")]
        public double Dy1 { get; set; } = -50.0;

        [InputParameter("Высота 2-го отверстия, см")]
        public double C2 { get; set; } = 20.0;

        [InputParameter("Ширина 2-го отверстия, см")]
        public double D2 { get; set; } = 50.0;

        [InputParameter("Привязка 2-го отверстия по X, см")]
        public double Dx2 { get; set; } = 45.0;

        [InputParameter("Привязка 2-го отверстия по Y, см")]
        public double Dy2 { get; set; } = -30.0;

        [InputParameter("Класс бетона")] 
        public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B40;

        [InputParameter("Коэффициент условий работы бетона")]
        public double GammaBi { get; set; } = 0.9;

        [InputParameter("Учесть поперечную арматуру")]
        public bool ConsiderShearReinforcement { get; set; } = false;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A400;

        [InputParameter("Площадь поперечной арматуры, см²")]
        public double Asw { get; set; } = 1.06;

        [InputParameter("Шаг поперечной арматуры, см")]
        public double Sw { get; set; } = 10.0;

        public override BaseCalculateResult Calculate()
        {
            // 1. Вычисление геометрических характеристик
            double ho = H - A;
            double A1 = C1 * D1;
            double A2 = C2 * D2;
        
            // Статические моменты отверстий
            double Sy_prime = A1 * Dy1 + A2 * Dy2;
            double Sx_prime = A1 * Dx1 + A2 * Dx2;
        
            // Моменты инерции отверстий
            double Iby_prime = (C1 * Math.Pow(D1, 3)) / 12 + A1 * Math.Pow(Dy1, 2) 
                            + (C2 * Math.Pow(D2, 3)) / 12 + A2 * Math.Pow(Dy2, 2);
        
            double Ibx_prime = (D1 * Math.Pow(C1, 3)) / 12 + A1 * Math.Pow(Dx1, 2) 
                            + (D2 * Math.Pow(C2, 3)) / 12 + A2 * Math.Pow(Dx2, 2);
        
            // Периметр отверстий (сумма сторон)
            double u_prime = 2 * (C1 + D1) + 2 * (C2 + D2);
        
            // Характеристики контура продавливания
            double Lx = Bcx + ho;
            double Ly = Acy + ho;
            double u = 2 * (Lx + Ly) - u_prime;
            double Ab = u * ho;
        
            // Координаты центра тяжести
            double xc = -Sx_prime / u;
            double yc = -Sy_prime / u;
        
            // Моменты инерции брутто
            double Ibx = Math.Pow(Lx, 3) / 6 + (Ly * Math.Pow(Lx, 2)) / 2 - Ibx_prime - u * Math.Pow(xc, 2);
            double Iby = Math.Pow(Ly, 3) / 6 + (Lx * Math.Pow(Ly, 2)) / 2 - Iby_prime - u * Math.Pow(yc, 2);
        
            // Максимальные расстояния до края
            double xmax = Lx / 2 + Math.Abs(xc);
            double ymax = Ly / 2 + Math.Abs(yc);
        
            // Моменты сопротивления
            double Wbx = Ibx / xmax;
            double Wby = Iby / ymax;
            
            // 2. Расчет характеристик материалов
            double Rbt = ConcreteClass.GetRbt(GammaBi);
            double Rsw = ReinforcementClass.GetRsw();
            
            // 3. Эффективные усилия
            double F_abs = Math.Abs(F);
            double effectiveF = (FDirection == 0) ? F : -F;
            
            double Mx_eff = CalculateEffectiveMoment(Mx, effectiveF, xc, DivideM, DivideFe);
            double My_eff = CalculateEffectiveMoment(My, effectiveF, yc, DivideM, DivideFe);
            
            // 4. Предельная несущая способность
            double Fb_ult = Rbt * Ab;
            double Mbx_ult = Rbt * Wbx * ho;
            double Mby_ult = Rbt * Wby * ho;
        
            // Параметры арматуры (если учтена)
            double qsw = 0;
            double Fsw_ult = 0;
            double Fsw_ult_corr = 0;
            double Wsw_x = 0;
            double Wsw_y = 0;
            double Msw_x_ult = 0;
            double Msw_y_ult = 0;
            double Msw_x_ult_corr = 0;
            double Msw_y_ult_corr = 0;
            
            if (ConsiderShearReinforcement)
            {
                qsw = Rsw * Asw / Sw;
                Fsw_ult = 0.8 * qsw * u;
                
                // Корректировка по СП
                if (Fsw_ult < 0.25 * Fb_ult)
                {
                    Fsw_ult_corr = 0;
                }
                else
                {
                    Fsw_ult_corr = Math.Min(Fsw_ult, Fb_ult);
                }
                
                Wsw_x = Wbx;
                Wsw_y = Wby;
                
                Msw_x_ult = 0.8 * qsw * Wsw_x;
                Msw_y_ult = 0.8 * qsw * Wsw_y;
                
                Msw_x_ult_corr = Math.Min(Msw_x_ult, Mbx_ult);
                Msw_y_ult_corr = Math.Min(Msw_y_ult, Mby_ult);
            }
            
            double F_ult = Fb_ult + Fsw_ult_corr;
            double Mx_ult = Mbx_ult + Msw_x_ult_corr;
            double My_ult = Mby_ult + Msw_y_ult_corr;
            
            // 5. Проверка прочности
            double condition = (F_abs / F_ult) + (Mx_eff / Mx_ult) + (My_eff / My_ult);
            bool result = condition <= 1;
            
            // Заполнение результата
            CalculateResult.Iby_prime = Iby_prime;
            CalculateResult.Ibx_prime = Ibx_prime;
            CalculateResult.Sx_prime = Sx_prime;
            CalculateResult.Sy_prime = Sy_prime;
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
            CalculateResult.qsw = qsw;
            CalculateResult.Fsw_ult = Fsw_ult_corr;
            CalculateResult.F_ult = F_ult;
            CalculateResult.Wsw_x = Wsw_x;
            CalculateResult.Wsw_y = Wsw_y;
            CalculateResult.Msw_x_ult = Msw_x_ult_corr;
            CalculateResult.Msw_y_ult = Msw_y_ult_corr;
            CalculateResult.Mx_ult = Mx_ult;
            CalculateResult.My_ult = My_ult;
            CalculateResult.Result = result;
            
            return CalculateResult;
        }
        
        private double CalculateEffectiveMoment(
            double M, double F_eff, 
            double offset, bool divideM, bool divideFe)
        {
            double effectiveM = divideM ? M / 2 : M;
            double additionalMoment = F_eff * offset;
            
            if (divideFe)
            {
                additionalMoment /= 2;
            }
            
            return Math.Abs(effectiveM + additionalMoment);
        }
    }
}
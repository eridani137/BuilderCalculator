using System;
using Calculators.Shared;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;

namespace BuilderCalculator.KZH_07_3
{
    public class Calculator : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; set; }
        
        public Calculator()
        {
            CalculateResult = new CalculateResult(this);
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

        [InputParameter("Ширина зоны приложения нагрузки, см")] 
        public double acy { get; set; } = 40.0;
        
        [InputParameter("Высота зоны приложения нагрузки, см")] 
        public double bcx { get; set; } = 50.0;
        
        [InputParameter("Высота сечения, см")]
        public double h { get; set; } = 20.0;

        [InputParameter("Защитный слой бетона растянутой зоны, см")]
        public double a { get; set; } = 5.0;
        
        [InputParameter("Класс бетона на сжатие")] 
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
            // 1. Расчет рабочей высоты сечения
            CalculateResult.H0 = h - a;
        
            // 2. Обработка моментов
            if (ConsiderBendingMoments)
            {
                // Приведение моментов к абсолютным значениям
                Mx = Math.Abs(Mx);
                My = Math.Abs(My);
            
                if (DivideMomentsByTwo)
                {
                    Mx /= 2.0;
                    My /= 2.0;
                }
            }
            
            // 3. Учет отпора грунта
            if (ConsiderSoilReaction)
            {
                double Fp = p * (acy + 2 * CalculateResult.H0) * (bcx + 2 * CalculateResult.H0);
                if (Fp < F)
                {
                    F -= Fp;
                }
            }
            
            // 4. Определение характеристик материалов
            double Rbt = ConcreteClass.GetRbt() * GammaB;
            double Rsw = ConsiderShearReinforcement ? ReinforcementClass.GetRsw() : 0;
            
            // 5. Расчет геометрических характеристик
            double Lx = bcx + CalculateResult.H0;
            double Ly = acy + CalculateResult.H0;
            double u = 2 * (Lx + Ly);
            double Ab = u * CalculateResult.H0;
            CalculateResult.Fb_ult = Rbt * Ab;
            
            // 6. Расчет характеристик для моментов (если нужно)
            if (ConsiderBendingMoments)
            {
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
            
            // 7. Учет поперечной арматуры
            CalculateResult.qsw = 0;
            CalculateResult.Fsw_ult = 0;
            CalculateResult.Msw_x_ult = 0;
            CalculateResult.Msw_y_ult = 0;
        
            if (ConsiderShearReinforcement)
            {
                CalculateResult.qsw = Rsw * Asw / Sw;
                CalculateResult.Fsw_ult = 0.8 * CalculateResult.qsw * u;
            
                // Проверка условий СП 63.13330.2012
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
            
            // 8. Определение общих предельных усилий
            CalculateResult.F_ult = CalculateResult.Fb_ult + CalculateResult.Fsw_ult;
        
            if (ConsiderBendingMoments)
            {
                CalculateResult.Mx_ult = CalculateResult.Mbx_ult + CalculateResult.Msw_x_ult;
                CalculateResult.My_ult = CalculateResult.Mby_ult + CalculateResult.Msw_y_ult;
            }
            
            // 9. Проверка условий прочности
            if (!ConsiderBendingMoments)
            {
                CalculateResult.Result = (F <= CalculateResult.F_ult);
            }
            else
            {
                double term1 = (Mx / CalculateResult.Mx_ult) + (My / CalculateResult.My_ult);
                double term2 = F / (2 * CalculateResult.F_ult);
                double sum = (F / CalculateResult.F_ult) + term1;
            
                CalculateResult.Result = (term1 <= term2) && (sum <= 1.0);
            }
            
            return CalculateResult;
        }
    }
}
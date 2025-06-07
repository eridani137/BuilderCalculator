using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_05._1
{
    public class Calculator : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; }

        public Calculator()
        {
            CalculateResult = new CalculateResult(this);
        }

        [InputParameter("Изгибающий момент Mx, кг·см")]
        public double Mx { get; set; } = 1500000.0; // 15.0·10^5
    
        [InputParameter("Изгибающий момент My, кг·см")]
        public double My { get; set; } = 1000000.0; // 10.0·10^5
    
        [InputParameter("Нормальная сила, кг")]
        public double N { get; set; } = 260000.0;
    
        [InputParameter("Ширина сечения, см")]
        public double b { get; set; } = 40.0;
    
        [InputParameter("Высота сечения, см")]
        public double h { get; set; } = 50.0;
    
        [InputParameter("Защитный слой бетона растянутой зоны, см")]
        public double a { get; set; } = 5.0;
    
        [InputParameter("Защитный слой бетона сжатой зоны, см")]
        public double ap { get; set; } = 5.0;
    
        [InputParameter("Площадь растянутой арматуры, см^2")]
        public double As { get; set; } = 16.09;
    
        [InputParameter("Площадь сжатой арматуры, см^2")]
        public double Asp { get; set; } = 16.09;
    
        [InputParameter("Класс бетона")]
        public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B25;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A500;
        
        
        private double Rb;   // Призменная прочность бетона
        private double Rs;   // Сопротивление арматуры растяжению
        private double Rsc;  // Сопротивление арматуры сжатию
        
        public override BaseCalculateResult Calculate()
        {
            GetMaterialProperties();
            CalculateForAxisX();
            CalculateForAxisY();
            CheckStrength();
            
            return CalculateResult;
        }
        
        private void GetMaterialProperties()
        {
            Rb = ConcreteClass.GetRb();
            Rs = ReinforcementClass.GetRs();
            Rsc = ReinforcementClass.GetRsc();
        }
        
        private void CalculateForAxisX()
        {
            CalculateResult.h0x = h - a;
            CalculateResult.h0px = h - ap;
        
            CalculateResult.alpha_nx = N / (Rb * b * CalculateResult.h0x);
            CalculateResult.alpha_sx = (Rs * As) / (Rb * b * CalculateResult.h0x);
        
            // Расчет граничной высоты сжатой зоны (Rs в МПа)
            double Rs_MPa = Rs / 10.0;
            CalculateResult.xi_R = 0.8 / (1.0 + Rs_MPa / 700.0);
        
            if (CalculateResult.alpha_nx <= CalculateResult.xi_R)
            {
                CalculateResult.xi_x = CalculateResult.alpha_nx;
            }
            else
            {
                CalculateResult.xi_x = (CalculateResult.alpha_nx * (1 - CalculateResult.xi_R) + 2 * CalculateResult.alpha_sx * CalculateResult.xi_R) / 
                                       (1 - CalculateResult.xi_R + 2 * CalculateResult.alpha_sx);
            }
        
            double x = CalculateResult.xi_x * CalculateResult.h0x;
            double term1 = Rb * b * x * (CalculateResult.h0x - 0.5 * x);
            double term2 = (Rsc * Asp - N / 2.0) * (CalculateResult.h0x - ap);
            CalculateResult.M0x = term1 + term2;
        }
        
        private void CalculateForAxisY()
        {
            CalculateResult.h0y = b - a;
            CalculateResult.h0py = b - ap;
        
            CalculateResult.alpha_ny = N / (Rb * h * CalculateResult.h0y);
            CalculateResult.alpha_sy = (Rs * As) / (Rb * h * CalculateResult.h0y);
        
            if (CalculateResult.alpha_ny <= CalculateResult.xi_R)
            {
                CalculateResult.xi_y = CalculateResult.alpha_ny;
            }
            else
            {
                CalculateResult.xi_y = (CalculateResult.alpha_ny * (1 - CalculateResult.xi_R) + 2 * CalculateResult.alpha_sy * CalculateResult.xi_R) / 
                                       (1 - CalculateResult.xi_R + 2 * CalculateResult.alpha_sy);
            }
        
            double x = CalculateResult.xi_y * CalculateResult.h0y;
            double term1 = Rb * h * x * (CalculateResult.h0y - 0.5 * x);
            double term2 = (Rsc * Asp - N / 2.0) * (CalculateResult.h0y - ap);
            CalculateResult.M0y = term1 + term2;
        }
        
        private void CheckStrength()
        {
            CalculateResult.Astot = As + Asp;
            CalculateResult.alpha_s = (Rs * CalculateResult.Astot) / (Rb * b * h);
            CalculateResult.k0 = (0.275 + CalculateResult.alpha_s) / (0.16 + CalculateResult.alpha_s);
            CalculateResult.alpha_n = N / (Rb * b * h);
        
            double part1 = Math.Pow(1.7 - CalculateResult.alpha_s, 2) / 4.0 + 0.1775;
            double part2 = Math.Pow(CalculateResult.alpha_n, 2) - 0.16;
            CalculateResult.k = part1 * part2 + CalculateResult.k0;
            CalculateResult.k = Math.Min(CalculateResult.k, 1.6);
        
            double ratioX = Math.Pow(Mx / CalculateResult.M0x, CalculateResult.k);
            double ratioY = Math.Pow(My / CalculateResult.M0y, CalculateResult.k);
            CalculateResult.Result = (ratioX + ratioY) <= 1.0;
        }

    }
}
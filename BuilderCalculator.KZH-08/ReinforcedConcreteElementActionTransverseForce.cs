using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;

namespace BuilderCalculator.KZH_08
{
    public class ReinforcedConcreteElementActionTransverseForce : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; set; }

        public ReinforcedConcreteElementActionTransverseForce()
        {
            CalculateResult = new CalculateResult(this);
        }
        
        [InputParameter("Изгибающий момент M (кг*см)")]
        public double M { get; set; } = 5.5e5;
    
        [InputParameter("Поперечная сила Q (кг)")]
        public double Q { get; set; } = 2e4;
    
        [InputParameter("Продольная сила N (кг)")]
        public double N { get; set; } = -3e3;
    
        [InputParameter("Распределенная нагрузка q (кг/см)")]
        public double q { get; set; } = 10;
    
        [InputParameter("Ширина сечения b (см)")]
        public double b { get; set; } = 30;
    
        [InputParameter("Высота сечения h (см)")]
        public double h { get; set; } = 60;
    
        [InputParameter("Толщина защитного слоя a (см)")]
        public double a { get; set; } = 4;
    
        [InputParameter("Рабочая высота сечения h0 (см)")]
        public double h0 { get; set; } = 56;
    
        [InputParameter("Площадь поперечной арматуры Asw (см²)")]
        public double Asw { get; set; } = 2.1;
    
        [InputParameter("Шаг поперечной арматуры sw (см)")]
        public double sw { get; set; } = 20;
    
        [InputParameter("Площадь продольной арматуры As (см²)")]
        public double As { get; set; } = 10;
    
        [InputParameter("Площадь сжатой арматуры A's (см²)")]
        public double As_comp { get; set; } = 3;
    
        [InputParameter("Класс бетона")]
        public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B35;
    
        [InputParameter("Коэффициент условий работы бетона γb")]
        public double gamma_b { get; set; } = 1.0;
    
        [InputParameter("Класс продольной арматуры")]
        public ReinforcementClass LongReinfClass { get; set; } = ReinforcementClass.A400;
    
        [InputParameter("Класс поперечной арматуры")]
        public ReinforcementClass TransReinfClass { get; set; } = ReinforcementClass.A240;

        
        public override BaseCalculateResult Calculate()
        {
            if (Math.Abs(h0) < 0.001)
            {
                h0 = h - a;
            }
            
            

            return CalculateResult;
        }
    }
}
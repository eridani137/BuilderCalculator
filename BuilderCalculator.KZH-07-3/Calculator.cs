using Calculators.Shared;
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
            
            
            return CalculateResult;
        }
    }
}
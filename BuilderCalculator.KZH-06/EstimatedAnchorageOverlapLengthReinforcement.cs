using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_06
{
    public class EstimatedAnchorageOverlapLengthReinforcement : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; }

        public EstimatedAnchorageOverlapLengthReinforcement()
        {
            CalculateResult = new CalculateResult(this);
        }

        [InputParameter("Нормативное обоснование (0 - без изменений, 1 - изм.1)")]
        public int Edition { get; set; } = 0;

        [InputParameter("Тип стыка (1 - анкеровка, 2 - нахлестка)")]
        public int JointType { get; set; } = 1;

        [InputParameter("Напряженное состояние (1 - растянута, 2 - сжата)")]
        public int StressState { get; set; } = 1;

        [InputParameter("Соотношение площадей арматуры (As,cal/As,ef)")]
        public double AreaRatio { get; set; } = 1.0;

        [InputParameter("Диаметр арматуры, мм")]
        public double Diameter { get; set; } = 12.0;

        [InputParameter("Коэффициент условий работы бетона (γb1×γb5)")]
        public double GammaBi { get; set; } = 1.0;

        [InputParameter("Класс бетона")]
        public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B25;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A500;

        [InputParameter("Стыкуется 100% арматуры (только для нахлестки)")]
        public bool IsFullJoint { get; set; } = false;
        
        public override BaseCalculateResult Calculate()
        {
            ValidateInputs();
            
            double ds = Diameter / 10; // Переводим мм в см
            double rbt = ConcreteClass.GetRbt1();
            double rs = ReinforcementClass.GetRs1();

            // Расчет коэффициентов
            double eta1 = 2.5; // Для периодического профиля
            double eta2 = 1.0; // Без учета поперечного давления
            CalculateResult.Rbond = eta1 * eta2 * rbt;

            // Геометрические характеристики
            CalculateResult.As = Math.PI * Math.Pow(ds, 2) / 4.0;
            CalculateResult.Us = Math.PI * ds;

            // Базовая длина анкеровки
            CalculateResult.L0an = (rs * CalculateResult.As) / (CalculateResult.Rbond * CalculateResult.Us);

            // Коэффициент α
            double alpha = CalculateAlpha();

            // Расчетная длина
            CalculateResult.Lan = alpha * CalculateResult.L0an * AreaRatio;
            CalculateResult.Lan_ds = CalculateResult.Lan / ds;

            // Проверка условий
            CalculateResult.Result1 = CalculateResult.Lan >= 0.3 * CalculateResult.L0an;
            CalculateResult.Result2 = CalculateResult.Lan >= 15 * ds;
            CalculateResult.Result3 = CalculateResult.Lan >= 20.0;
            
            return CalculateResult;
        }
        
        private void ValidateInputs()
        {
            if (Diameter <= 0) 
                throw new ArgumentException("Диаметр арматуры должен быть положительным");
                
            if (AreaRatio <= 0)
                throw new ArgumentException("Соотношение площадей должно быть положительным");
                
            if (GammaBi <= 0)
                throw new ArgumentException("Коэффициент условий работы должен быть положительным");
                
            if (JointType < 1 || JointType > 2)
                throw new ArgumentException("Недопустимый тип стыка (допустимо: 1 или 2)");
                
            if (StressState < 1 || StressState > 2)
                throw new ArgumentException("Недопустимое напряженное состояние (допустимо: 1 или 2)");
        }
        
        private double CalculateAlpha()
        {
            // Анкеровка
            if (JointType == 1) // Anchorage
            {
                return StressState == 2 ? 0.75 : 1.00; // Compression : Tension
            }
            
            // Нахлестка
            if (JointType == 2) // Overlap
            {
                if (StressState == 2) // Compression
                    return 0.90;
                
                // Tension
                return IsFullJoint ? 1.80 : 1.20;
            }

            throw new InvalidOperationException("Недопустимый тип стыка");
        }
    }
}
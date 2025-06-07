using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_02
{
    public class ReinforcementForBendingReinforcedConcreteElement : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; }

        public ReinforcementForBendingReinforcedConcreteElement()
        {
            CalculateResult = new CalculateResult(this);
        }

        public ReinforcementForBendingReinforcedConcreteElement(double m, int loadDuration, int sectionShape, double b, double bf, double h, double hf, double a, double aPrime, ConcreteClass concreteClass, ReinforcementClass reinforcementClass, double gammaBi)
        {
            CalculateResult = new CalculateResult(this);
            M = m;
            LoadDuration = loadDuration;
            SectionShape = sectionShape;
            B = b;
            Bf = bf;
            H = h;
            Hf = hf;
            A = a;
            APrime = aPrime;
            ConcreteClass = concreteClass;
            ReinforcementClass = reinforcementClass;
            GammaBi = gammaBi;
        }

        private const double _es = 2038736; // Модуль упругости арматуры (кгс/см²)
        private const double _epsB2 = 0.0035; // Относительная деформация бетона
        
        [InputParameter("Изгибающий момент, кг·см")]
        public double M { get; set; } = 1400000;

        [InputParameter("Продолжительность нагрузки (0-кратковременная, 1-длительная)")]
        public int LoadDuration { get; set; } = 0;

        [InputParameter("Форма сечения (0-прямоугольное, 1-тавровое)")]
        public int SectionShape { get; set; } = 0;

        [InputParameter("Ширина сечения, см")]
        public double B { get; set; } = 30.0;

        [InputParameter("Ширина полки тавра, см")]
        public double Bf { get; set; } = 90.0;

        [InputParameter("Высота сечения, см")]
        public double H { get; set; } = 40.0;

        [InputParameter("Высота полки тавра, см")]
        public double Hf { get; set; } = 10.0;

        [InputParameter("Расстояние до растянутой арматуры, см")]
        public double A { get; set; } = 5.0;

        [InputParameter("Расстояние до сжатой арматуры, см")]
        public double APrime { get; set; } = 5.0;

        [InputParameter("Класс бетона")]
        public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B25;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A500;

        [InputParameter("Коэффициент условий работы бетона")]
        public double GammaBi { get; set; } = 1.0;

        public override BaseCalculateResult Calculate()
        {
            ValidateInputs();
            double h0 = H - A;
            double rb = ConcreteClass.GetRb();
            double rs = ReinforcementClass.GetRs();

            // Расчет ξR и αR
            double epsS = rs / _es;
            CalculateResult.XiR = 0.8 / (1 + epsS / _epsB2);
            CalculateResult.AlphaR = CalculateResult.XiR * (1 - 0.5 * CalculateResult.XiR);

            // Расчет αM и As в зависимости от формы сечения
            if (SectionShape == 0) 
            {
                CalculateRectangular(rb, rs, h0);
            }
            else 
            {
                CalculateTee(rb, rs, h0);
            }

            return CalculateResult;
        }
        
        private void CalculateRectangular(double rb, double rs, double h0)
        {
            CalculateResult.AlphaM = M / (rb * B * h0 * h0);
            ValidateAlphaM();
            CalculateResult.As = rb * B * h0 * (1 - Math.Sqrt(1 - 2 * CalculateResult.AlphaM)) / rs;
        }

        private void CalculateTee(double rb, double rs, double h0)
        {
            // Проверка положения нейтральной оси
            double mf = rb * Bf * Hf * (h0 - 0.5 * Hf);
        
            if (M <= mf) 
            {
                CalculateResult.AlphaM = M / (rb * Bf * h0 * h0);
                ValidateAlphaM();
                CalculateResult.As = rb * Bf * h0 * (1 - Math.Sqrt(1 - 2 * CalculateResult.AlphaM)) / rs;
            }
            else 
            {
                double aov = (Bf - B) * Hf;
                double m1 = rb * aov * (h0 - 0.5 * Hf);
                double m2 = M - m1;
                CalculateResult.AlphaM = m2 / (rb * B * h0 * h0);
                ValidateAlphaM();
                CalculateResult.As = (rb * B * h0 * (1 - Math.Sqrt(1 - 2 * CalculateResult.AlphaM)) + rb * aov) / rs;
            }
        }
        
        private void ValidateAlphaM()
        {
            if (CalculateResult.AlphaM > CalculateResult.AlphaR)
                throw new InvalidOperationException("Требуется сжатая арматура (не реализовано)");
        }

        private void ValidateInputs()
        {
            if (SectionShape == 1 && (Bf <= B || Hf <= 0))
                throw new ArgumentException("Неверные параметры таврового сечения");
        }
    }
}
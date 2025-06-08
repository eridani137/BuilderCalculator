using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_12
{
    public class BearingCapacityPunchingColumnAtCorner : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; set; }

        public BearingCapacityPunchingColumnAtCorner()
        {
            CalculateResult = new CalculateResult(this);
        }

        public BearingCapacityPunchingColumnAtCorner(int caseType, double n, int loadDistribution, double a1, double a2, double c, bool includeIndirectReinforcement, double lx, double asx, int nx, double ly, double asy, int ny, double s, ConcreteClass concreteClass, double gammaBi, ReinforcementClass reinforcementClass)
        {
            CalculateResult = new CalculateResult(this);
            CaseType = caseType;
            N = n;
            LoadDistribution = loadDistribution;
            this.a1 = a1;
            this.a2 = a2;
            this.c = c;
            IncludeIndirectReinforcement = includeIndirectReinforcement;
            this.lx = lx;
            Asx = asx;
            this.nx = nx;
            this.ly = ly;
            Asy = asy;
            this.ny = ny;
            this.s = s;
            ConcreteClass = concreteClass;
            gamma_bi = gammaBi;
            ReinforcementClass = reinforcementClass;
        }

        [InputParameter("Случай приложения нагрузки (1-8)")]
        public int CaseType { get; set; } = 1;

        [InputParameter("Сосредоточенная сила (кг)")]
        public double N { get; set; } = 8e4;

        [InputParameter("Распределение нагрузки (0-равномерное, 1-неравномерное)")]
        public int LoadDistribution { get; set; } = 0;

        [InputParameter("Ширина зоны приложения нагрузки (см)")]
        public double a1 { get; set; } = 20.0;

        [InputParameter("Высота зоны приложения нагрузки (см)")]
        public double a2 { get; set; } = 22.0;

        [InputParameter("Расстояние от края (см, для схем 4,7,8)")]
        public double c { get; set; } = 5.0;

        [InputParameter("Учитывать косвенное армирование")]
        public bool IncludeIndirectReinforcement { get; set; } = false;

        [InputParameter("Длина стержней в направлении X (см)")]
        public double lx { get; set; } = 30.0;

        [InputParameter("Площадь одного стержня в X (см2)")]
        public double Asx { get; set; } = 0.5;

        [InputParameter("Количество стержней в X (шт)")]
        public int nx { get; set; } = 7;

        [InputParameter("Длина стержней в направлении Y (см)")]
        public double ly { get; set; } = 30.0;

        [InputParameter("Площадь одного стержня в Y (см2)")]
        public double Asy { get; set; } = 0.5;

        [InputParameter("Количество стержней в Y (шт)")]
        public int ny { get; set; } = 7;

        [InputParameter("Шаг сеток (см)")]
        public double s { get; set; } = 5.0;

        [InputParameter("Класс бетона")]
        public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B25;

        [InputParameter("Коэффициент условий работы бетона")]
        public double gamma_bi { get; set; } = 0.9;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A400;
        
        public override BaseCalculateResult Calculate()
        {
            ValidateInputs();
            CalculateBaseParameters();
            CalculateIndirectReinforcement();
            CheckStrength();
            
            return CalculateResult;
        }
        
        private void ValidateInputs()
        {
            // Проверка обязательных параметров для косвенного армирования
            if (IncludeIndirectReinforcement)
            {
                if (lx == 0 || Asx == 0 || nx == 0 || 
                    ly == 0 || Asy == 0 || ny == 0 || s == 0)
                    throw new ArgumentException("Для косвенного армирования все параметры должны быть заданы");
            }

            // Проверка наличия 'c' для схем 4,7,8
            if (CaseType == 4 || CaseType == 7 || CaseType == 8)
            {
                if (c == 0)
                    throw new ArgumentException($"Для схемы {CaseType} требуется параметр 'c'");
            }
        }
        
        private void CalculateBaseParameters()
        {
            // 1. Основные площади
            CalculateResult.Ab_loc = a1 * a2;
            CalculateResult.Ab_max = CalculateAbMax();

            // 2. Коэффициент φ_b
            CalculateResult.phi_b = 0.8 * Math.Sqrt(CalculateResult.Ab_max / CalculateResult.Ab_loc);
            CalculateResult.phi_b = CalculateResult.phi_b.Clamp(1.0, 2.5);

            double Rb = ConcreteClass.GetRb();

            CalculateResult.Rb_loc = CalculateResult.phi_b * Rb * gamma_bi;
            CalculateResult.Psi = LoadDistribution == 0 ? 1.0 : 0.75;
        }
        
        private double CalculateAbMax()
        {
            switch (CaseType)
            {
                case 1:
                    return (a1 + 2 * a2) * a2; // Случай А
                case 2:
                    return (a1 + a2) * a2; // Случай Б
                case 3:
                    return a1 * a2; // Случай В
                case 4:
                    return (a1 + 2 * a2) * (a2 + 2 * c); // Случай Г
                case 5:
                    return (a1 + 2 * a2) * (2 * a1 + a2); // Случай Д
                case 6:
                    return Math.Pow(a1 + 2 * a2, 2); // Случай Е
                case 7:
                    return (a1 + 2 * a2) * (a2 + 2 * c); // Случай Ж
                case 8:
                    return (a1 + 2 * c) * (a2 + 2 * c); // Случай З
                default:
                    throw new ArgumentException("Недопустимый тип схемы (1-8)");
            }
        }
        
        private void CalculateIndirectReinforcement()
        {
            if (!IncludeIndirectReinforcement) return;

            // Эффективная площадь как lx * ly
            double Ab_loc_ef = lx * ly;
            CalculateResult.Ab_loc_ef = Ab_loc_ef; // Для вывода

            // Эффективная площадь для phi_sxy с учетом ограничения Ab_max
            double Ab_for_phi = Math.Min(Ab_loc_ef, CalculateResult.Ab_max);
            CalculateResult.phi_sxy = Math.Sqrt(Ab_for_phi / CalculateResult.Ab_loc);

            // Расчет mu_sxy с использованием Ab_loc_ef = lx * ly
            double numerator = nx * Asx * lx + ny * Asy * ly;
            CalculateResult.mu_sxy = numerator / (s * Ab_loc_ef);

            double Rsxy = ReinforcementClass.GetRs();

            CalculateResult.Rbs_loc = CalculateResult.Rb_loc + 2 * CalculateResult.phi_sxy * Rsxy * CalculateResult.mu_sxy;
            CalculateResult.Rbs_loc = Math.Min(CalculateResult.Rbs_loc, 2 * CalculateResult.Rb_loc);
        }
        
        private void CheckStrength()
        {
            double resistance = IncludeIndirectReinforcement ? CalculateResult.Rbs_loc : CalculateResult.Rb_loc;
            CalculateResult.DesignForce = CalculateResult.Psi * resistance * CalculateResult.Ab_loc;
            CalculateResult.Result = N <= CalculateResult.DesignForce;
        }
    }
}
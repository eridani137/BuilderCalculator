using System;
using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Enums;
using Calculators.Shared.Extensions;

namespace BuilderCalculator.KZH_14
{
    public class ColumnsCircularCrossSection : BaseBuilderCalculator
    {
        private CalculateResult CalculateResult { get; }

        public ColumnsCircularCrossSection()
        {
            CalculateResult = new CalculateResult(this);
        }

        public ColumnsCircularCrossSection(int statichOpred, int formaSechenia, double m, double ml, double n,
            double nl, double l, double mu, double dcir, double dcir1, double a, double asTot,
            ConcreteClass concreteClass, ReinforcementClass reinforcementClass, double gammaBi)
        {
            CalculateResult = new CalculateResult(this);
            StatichOpred = statichOpred;
            FormaSechenia = formaSechenia;
            M = m;
            Ml = ml;
            N = n;
            Nl = nl;
            L = l;
            Mu = mu;
            Dcir = dcir;
            Dcir1 = dcir1;
            A = a;
            AsTot = asTot;
            ConcreteClass = concreteClass;
            ReinforcementClass = reinforcementClass;
            GammaBi = gammaBi;
        }

        [InputParameter("Статическая определимость конструкции (0 - определимая, 1 - неопределимая)")]
        public int StatichOpred { get; set; } = 0;

        [InputParameter("Форма поперечного сечения (0 - круглое, 1 - кольцевое)")]
        public int FormaSechenia { get; set; } = 0;

        [InputParameter("Изгибающий момент от полной нагрузки, кг·см")]
        public double M { get; set; } = 2500000.0;

        [InputParameter("Изгибающий момент от длительной нагрузки, кг·см")]
        public double Ml { get; set; } = 2000000.0;

        [InputParameter("Продольная сила от полной нагрузки, кг")]
        public double N { get; set; } = 110000.0;

        [InputParameter("Продольная сила от длительной нагрузки, кг")]
        public double Nl { get; set; } = 100000.0;

        [InputParameter("Длина элемента, см")] public double L { get; set; } = 500.0;

        [InputParameter("Коэффициент приведения к расчетной длине")]
        public double Mu { get; set; } = 1.0;

        [InputParameter("Наружный диаметр, см")]
        public double Dcir { get; set; } = 60.0;

        [InputParameter("Внутренний диаметр, см (для кольцевого сечения)")]
        public double Dcir1 { get; set; } = 40.0;

        [InputParameter("Защитный слой бетона, см")]
        public double A { get; set; } = 5.0;

        [InputParameter("Площадь продольной арматуры, см²")]
        public double AsTot { get; set; } = 31.4;

        [InputParameter("Класс бетона")] public ConcreteClass ConcreteClass { get; set; } = ConcreteClass.B25;

        [InputParameter("Класс арматуры")]
        public ReinforcementClass ReinforcementClass { get; set; } = ReinforcementClass.A500;

        [InputParameter("Коэффициент условий работы бетона")]
        public double GammaBi { get; set; } = 0.9;

        public override BaseCalculateResult Calculate()
        {
            double ea = Math.Max(Math.Max(L / 600, Dcir / 30), 1);
            double e0;
            if (StatichOpred == 0) // Определимая
            {
                e0 = ea + M / N;
            }
            else // Неопределимая
            {
                e0 = M / N;
                if (e0 <= ea)
                {
                    e0 = ea;
                }
            }

            CalculateResult.E0 = e0;

            double deltaE = e0 / Dcir;
            CalculateResult.DeltaE = deltaE;

            double r = Dcir / 2;
            double rs = r - A;
            double M1 = M + N * rs;
            CalculateResult.M1 = M1;

            double Ml1 = Ml + Nl * rs;
            CalculateResult.Ml1 = Ml1;

            double phiL = 1 + Ml1 / M1;
            CalculateResult.PhiL = phiL;

            double kb = 0.15 / (phiL * (0.3 + deltaE));
            CalculateResult.Kb = kb;

            // Момент инерции
            double I;
            if (FormaSechenia == 0) // Круглое
            {
                I = Math.PI * Math.Pow(Dcir, 4) / 64;
            }
            else // Кольцевое
            {
                double r1 = Dcir1 / 2;
                I = Math.PI * (Math.Pow(r, 4) - Math.Pow(r1, 4)) / 4;
            }

            CalculateResult.I = I;

            double Is = AsTot * Math.Pow(rs, 2) / 2;
            CalculateResult.Is = Is;

            // Жесткость
            double Eb = ConcreteClass.GetEb();
            double Es = 2038736;
            double ks = 0.7;
            double D = kb * Eb * I + ks * Es * Is;
            CalculateResult.D = D;

            // Критическая сила
            double l0 = Mu * L;
            double Ncr = Math.Pow(Math.PI, 2) * D / Math.Pow(l0, 2);
            CalculateResult.Ncr = Ncr;

            if (Ncr < N)
            {
                CalculateResult.Result = false;
                return CalculateResult;
            }

            double eta = 1 / (1 - N / Ncr);
            CalculateResult.Eta = eta;

            double MCalculated = N * e0 * eta;
            CalculateResult.MCalculated = MCalculated;

            // Площадь сечения
            double Area;
            if (FormaSechenia == 0) // Круглое
            {
                Area = Math.PI * Math.Pow(r, 2);
            }
            else // Кольцевое
            {
                double r1 = Dcir1 / 2;
                Area = Math.PI * (Math.Pow(r, 2) - Math.Pow(r1, 2));
            }

            CalculateResult.Area = Area;

            // Характеристики материалов
            double Rs = ReinforcementClass.GetRs();
            double Rsc = ReinforcementClass.GetRsc();
            double Rb = ConcreteClass.GetRb() * GammaBi;

            // Проверка продольной силы
            double N_limit;
            if (FormaSechenia == 0) // Круглое
            {
                N_limit = 0.77 * Rb * Area + 0.645 * Rs * AsTot;
            }
            else // Кольцевое
            {
                N_limit = Rb * Area + Rsc * AsTot;
            }

            if (N > N_limit)
            {
                CalculateResult.Result = false;
                return CalculateResult;
            }

            // Вычисление ξcir
            double XiCir;
            if (FormaSechenia == 0) // Круглое
            {
                XiCir = CalculateXiCirForCircular(N, Rs, Rb, Area, AsTot);
            }
            else // Кольцевое
            {
                XiCir = (N + Rs * AsTot) / (Rb * Area + (Rsc + 1.7 * Rs) * AsTot);
            }

            CalculateResult.XiCir = XiCir;

            // Коэффициент φ (только для круглого сечения)
            double Phi = 0;
            if (FormaSechenia == 0)
            {
                Phi = 1.6 * (1 - 1.55 * XiCir) * XiCir;
                if (Phi > 1.0) Phi = 1.0;
            }

            CalculateResult.Phi = Phi;

            // Несущая способность Mult
            double Mult;
            if (FormaSechenia == 0) // Круглое
            {
                Mult = (2.0 / 3.0) * Rb * Area * r * Math.Pow(Math.Sin(Math.PI * XiCir), 3) / Math.PI +
                       Rs * AsTot * (Math.Sin(Math.PI * XiCir) / Math.PI + Phi) * rs;
            }
            else // Кольцевое
            {
                double r1 = Dcir1 / 2;
                double rm = (r1 + r) / 2;
                Mult = (Rb * Area * rm + Rsc * AsTot * rs) * Math.Sin(Math.PI * XiCir) / Math.PI +
                       Rs * AsTot * rs * (1 - 1.7 * XiCir) * (0.2 + 1.3 * XiCir);
            }

            CalculateResult.Mult = Mult;

            // Проверка прочности
            CalculateResult.Result = MCalculated <= Mult;

            return CalculateResult;
        }

        private double CalculateXiCirForCircular(double N, double Rs, double Rb, double Area, double AsTot)
        {
            // Метод бисекции для решения уравнения (Д.9)
            double a = 0;
            double b = 1;
            double tolerance = 1e-6;
            int maxIterations = 100;
            double XiCir = 0;

            for (int i = 0; i < maxIterations; i++)
            {
                double c = (a + b) / 2;
                double fc = (N + Rs * AsTot + Rb * Area * Math.Sin(2 * Math.PI * c) / (2 * Math.PI)) /
                    (Rb * Area + 2.55 * Rs * AsTot) - c;
                if (Math.Abs(fc) < tolerance)
                {
                    XiCir = c;
                    break;
                }

                double fa = (N + Rs * AsTot + Rb * Area * Math.Sin(2 * Math.PI * a) / (2 * Math.PI)) /
                    (Rb * Area + 2.55 * Rs * AsTot) - a;
                if (fa * fc < 0)
                {
                    b = c;
                }
                else
                {
                    a = c;
                }

                XiCir = (a + b) / 2;
            }

            return XiCir;
        }
    }
}
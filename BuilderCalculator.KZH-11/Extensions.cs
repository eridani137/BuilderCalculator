using System;
using Calculators.Shared.Enums;

namespace BuilderCalculator.KZH_11
{
    public static class Extensions
    {
        public static double GetRb(this ConcreteClass concreteClass)
        {
            switch (concreteClass)
            {
                case ConcreteClass.B10: return 61.2;
                case ConcreteClass.B15: return 86.6;
                case ConcreteClass.B20: return 117.0;
                case ConcreteClass.B25: return 148.0;
                case ConcreteClass.B30: return 173.0;
                case ConcreteClass.B35: return 199.0;
                case ConcreteClass.B40: return 224.0;
                case ConcreteClass.B45: return 255.0;
                case ConcreteClass.B50: return 280.0;
                case ConcreteClass.B55: return 306.0;
                case ConcreteClass.B60: return 336.0;
                default:
                    throw new ArgumentOutOfRangeException(nameof(concreteClass), concreteClass, null);
            }
        }
        
        public static double GetRsc(this ReinforcementClass reinforcementClass)
        {
            switch (reinforcementClass)
            {
                case ReinforcementClass.A240: return 2190;
                case ReinforcementClass.A400: return 3620;
                case ReinforcementClass.A500: return 4080;
                case ReinforcementClass.B500: return 3670;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reinforcementClass), reinforcementClass, null);
            }
        }
        
        public static double GetRs(this ReinforcementClass reinforcementClass)
        {
            switch (reinforcementClass)
            {
                case ReinforcementClass.A240: return 2190;
                case ReinforcementClass.A400: return 3620;
                case ReinforcementClass.A500: return 4430;
                case ReinforcementClass.B500: return 4230;
                default:
                    throw new ArgumentOutOfRangeException(nameof(reinforcementClass), reinforcementClass, null);
            }
        }
    }
}
using System;
using Calculators.Shared.Enums;

namespace BuilderCalculator.KZH_08
{
    public static class Extensions
    {
        public static double GetRb(this ConcreteClass concreteClass)
        {
            switch (concreteClass)
            {
                case ConcreteClass.B10:  return  61.2;
                case ConcreteClass.B15:  return  86.7;
                case ConcreteClass.B20:  return 117.3;
                case ConcreteClass.B25:  return 147.9;
                case ConcreteClass.B30:  return 173.4;
                case ConcreteClass.B35:  return 198.8;
                case ConcreteClass.B40:  return 224.3;
                case ConcreteClass.B45:  return 254.9;
                case ConcreteClass.B50:  return 280.4;
                case ConcreteClass.B55:  return 305.9;
                case ConcreteClass.B60:  return 336.5;
                default:
                    throw new ArgumentOutOfRangeException(nameof(concreteClass), concreteClass, null);
            }
        }
        
        public static double GetRbt(this ConcreteClass concreteClass)
        {
            switch (concreteClass)
            {
                case ConcreteClass.B10:  return   5.7;
                case ConcreteClass.B15:  return   7.6;
                case ConcreteClass.B20:  return   9.2;
                case ConcreteClass.B25:  return  10.7;
                case ConcreteClass.B30:  return  11.7;
                case ConcreteClass.B35:  return  13.3;
                case ConcreteClass.B40:  return  14.3;
                case ConcreteClass.B45:  return  15.3;
                case ConcreteClass.B50:  return  16.3;
                case ConcreteClass.B55:  return  17.3;
                case ConcreteClass.B60:  return  18.4;
                default:
                    throw new ArgumentOutOfRangeException(nameof(concreteClass), concreteClass, null);
            }
        }
    }
}
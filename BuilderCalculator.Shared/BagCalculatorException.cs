using System;

namespace Calculators.Shared
{
    public class BagCalculatorException : Exception
    {
        public BagCalculatorException() : base("Задан неверный тип калькулятора")
        {
        }
    }
}
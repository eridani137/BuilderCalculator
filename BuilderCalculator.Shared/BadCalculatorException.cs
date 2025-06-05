using System;

namespace Calculators.Shared
{
    public class BadCalculatorException : Exception
    {
        public BadCalculatorException() : base("Задан неверный тип калькулятора")
        {
        }
    }
}
using System;

namespace Calculators.Shared.Exceptions
{
    public class BadCalculatorException : Exception
    {
        public BadCalculatorException() : base("Задан неверный тип калькулятора")
        {
        }
    }
}
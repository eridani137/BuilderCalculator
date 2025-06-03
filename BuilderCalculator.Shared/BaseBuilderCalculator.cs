using System;
using System.Reflection;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace Calculators.Shared
{
    public abstract class BaseBuilderCalculator
    {
        public void Run()
        {
            EnteringParameters();
            var result = Calculate();
            result?.PrintParameters();
            result?.PrintSummary();

            AnsiConsole.MarkupLine("Нажмите любую клавишу для выхода...".MarkupPrimaryColor());
            Console.ReadKey(false);
        }
        
        public void EnteringParameters()
        {
            var type = GetType();
            var properties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttribute<ParameterAttribute>();
                if (attr == null || !prop.CanWrite) continue;
                var defaultValue = prop.GetValue(this);
                var value = attr.Name.EnterParameter(defaultValue);
                prop.SetValue(this, value);
            }
        }

        public abstract BaseCalculateResult Calculate();
    }
}
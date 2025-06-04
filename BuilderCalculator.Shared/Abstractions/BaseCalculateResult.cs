using System.Reflection;
using Calculators.Shared.Attributes;
using Spectre.Console;

namespace Calculators.Shared.Abstractions
{
    public abstract class BaseCalculateResult
    {
        protected BaseBuilderCalculator Calculator;

        protected BaseCalculateResult(BaseBuilderCalculator calculator)
        {
            Calculator = calculator;
        }
        
        public void PrintParameters()
        {
            var type = GetType();
            var properties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            var table = new Table();
            
            table.AddColumn("Параметр");
            table.AddColumn("Обозначение");
            table.AddColumn("Значение");

            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttribute<ResultValueAttribute>();
                if (attr == null || !prop.CanRead) continue;
                var value = prop.GetValue(this);
                
                table.AddRow(attr.Name, prop.Name, value.ToString());
            }

            AnsiConsole.Write(table);
        }
        
        public abstract void PrintSummary();
    }
}
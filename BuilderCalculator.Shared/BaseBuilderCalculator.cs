using System.Reflection;
using Calculators.Shared.Extensions;

namespace Calculators.Shared
{
    public abstract class BaseBuilderCalculator
    {
        public virtual void EnteringParameters()
        {
            var type = GetType();
            var properties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttribute<ParameterAttribute>();
                if (attr != null && prop.CanWrite)
                {
                    if (prop.PropertyType == typeof(double))
                    {
                        var defaultValue = prop.GetValue(this);
                        var value = attr.Name.EnterDoubleParameter((double)defaultValue);
                        prop.SetValue(this, value);
                    }
                    else if (prop.PropertyType == typeof(int))
                    {
                        
                    }
                }
            }
        }

        public abstract void Calculate();
        
        public abstract void PrintResults();
    }
}
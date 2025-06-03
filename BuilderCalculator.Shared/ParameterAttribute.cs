using System;

namespace Calculators.Shared
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ParameterAttribute : Attribute
    {
        public string Name { get; }

        public ParameterAttribute(string name)
        {
            Name = name;
        }
    }
}
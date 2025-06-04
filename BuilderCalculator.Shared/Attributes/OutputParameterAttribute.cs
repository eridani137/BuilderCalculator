using System;

namespace Calculators.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class OutputParameterAttribute : Attribute
    {
        public string Name { get; }

        public OutputParameterAttribute(string name)
        {
            Name = name;
        }
    }
}
using System;

namespace Calculators.Shared
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class InputParameterAttribute : Attribute
    {
        public string Name { get; }

        public InputParameterAttribute(string name)
        {
            Name = name;
        }
    }
}
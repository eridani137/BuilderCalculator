using System;

namespace Calculators.Shared.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class ResultValueAttribute : Attribute
    {
        public string Name { get; }

        public ResultValueAttribute(string name)
        {
            Name = name;
        }
    }
}
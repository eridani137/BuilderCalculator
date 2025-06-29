﻿using System;
using System.Reflection;
using Calculators.Shared.Attributes;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace Calculators.Shared.Abstractions
{
    public abstract class BaseBuilderCalculator
    {
        public void Run()
        {
            try
            {
                EnteringParameters();
                var result = Calculate();
                result?.PrintParameters();

                AnsiConsole.MarkupLine("Нажмите любую клавишу для выхода...".MarkupPrimaryColor());
                Console.ReadKey(false);
            }
            catch (Exception e)
            {
                AnsiConsole.WriteException(e);
            }
        }

        private void EnteringParameters()
        {
            var type = GetType();
            var properties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            foreach (var prop in properties)
            {
                var attr = prop.GetCustomAttribute<InputParameterAttribute>();
                if (attr == null || !prop.CanWrite) continue;
                var defaultValue = prop.GetValue(this);
                var value = attr.Name.EnterParameter(defaultValue);
                prop.SetValue(this, value);
            }
        }

        public abstract BaseCalculateResult Calculate();
    }
}
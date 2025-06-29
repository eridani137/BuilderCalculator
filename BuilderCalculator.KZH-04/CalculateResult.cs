﻿using Calculators.Shared.Abstractions;
using Calculators.Shared.Attributes;
using Calculators.Shared.Exceptions;
using Calculators.Shared.Extensions;
using Spectre.Console;

namespace Calculators.KZH_04
{
    public class CalculateResult : BaseCalculateResult
    {
        public CalculateResult(BaseBuilderCalculator calculator) : base(calculator)
        {
        }

        [OutputParameter("Приведенный момент инерции, см4")]
        public double I_red { get; set; }

        [OutputParameter("Приведенный момент сопротивления, см3")]
        public double W_red { get; set; }

        [OutputParameter("Пластический момент сопротивления, см3")]
        public double W_pl { get; set; }

        [OutputParameter("Эксцентриситет, см")]
        public double ex { get; set; }

        [OutputParameter("Момент трещинообразования, кг·см")]
        public double Mcrc { get; set; }

        [OutputParameter("Приведенный модуль упругости бетона, кг/см2")]
        public double Eb_red { get; set; }

        [OutputParameter("Отношение модулей упругости")]
        public double alpha_s1 { get; set; }

        [OutputParameter("Высота сжатой зоны, см")]
        public double xM { get; set; }

        [OutputParameter("Высота сжатой зоны с учетом нагрузок, см")]
        public double xm { get; set; }

        [OutputParameter("Приведенная площадь упругой стадии, см2")]
        public double A_red_elastic { get; set; }

        [OutputParameter("Приведенный момент инерции упругой стадии, см4")]
        public double I_red_elastic { get; set; }

        [OutputParameter("Приведенная площадь с трещиной, см2")]
        public double A_red { get; set; }

        [OutputParameter("Статический момент, см3")]
        public double St_red { get; set; }

        [OutputParameter("Расстояние до центра тяжести, см")]
        public double yc { get; set; }

        [OutputParameter("Высота растянутой зоны, см")]
        public double xt { get; set; }

        [OutputParameter("Напряжение в арматуре, кг/см2")]
        public double sigma_s { get; set; }

        [OutputParameter("Напряжение в арматуре при трещинообразовании, кг/см2")]
        public double sigma_s_crc { get; set; }

        [OutputParameter("Коэффициент")] 
        public double psi_s { get; set; }

        [OutputParameter("Ширина раскрытия от длительных нагрузок, см")]
        public double acrc1 { get; set; }

        [OutputParameter("Ширина раскрытия от полных нагрузок, см")]
        public double acrc2 { get; set; }

        [OutputParameter("Ширина раскрытия от кратковременных длительных нагрузок, см")]
        public double acrc3 { get; set; }

        [OutputParameter("Результат")]
        public bool Result { get; set; }
    }
}
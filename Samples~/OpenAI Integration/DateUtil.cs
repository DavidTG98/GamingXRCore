using System;
using System.Collections.Generic;

namespace GamingXRCore.OpenAIIntegration
{
    public static class DateUtil
    {
        private static readonly Dictionary<int, string> intMonths = new()
    {
        { 1, "Janeiro" },
        { 2, "Fevereiro" },
        { 3, "Março" },
        { 4, "Abril" },
        { 5, "Maio" },
        { 6, "Junho" },
        { 7, "Julho" },
        { 8, "Agosto" },
        { 9, "Setembro" },
        { 10, "Outubro" },
        { 11, "Novembro" },
        { 12, "Dezembro" },
    };

        public static string SystemDateInfo()
        {
            var currentDate = DateTime.Now;
            return String.Format("Tenha em mente que hoje é dia {0} de {1} de {2} e são {3}h e {4}min.",
                currentDate.Day,
                GetMonthString(currentDate.Month),
                currentDate.Year, currentDate.Hour,
                currentDate.Minute);

        }

        private static string GetMonthString(int monthNum)
        {
            return intMonths.GetValueOrDefault(monthNum, null);
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace congestion.calculator
{
    public class CongestionTaxCalculator
    {
        private Dictionary<DayOfWeek, bool> exemptDays;
        private Dictionary<int, int> timeIntervals;

        public CongestionTaxCalculator()
        {
            // Initialize the exempt days dictionary
            exemptDays = new Dictionary<DayOfWeek, bool>
            {
                { DayOfWeek.Saturday, true },
                { DayOfWeek.Sunday, true }
            };

            // Initialize the time intervals dictionary
            timeIntervals = new Dictionary<int, int>
            {
                { 6, 8 },
                { 7, 18 },
                { 8, 13 },
                { 15, 13 },
                { 17, 13 },
                { 18, 8 },
                { 19, 0 }
            };
        }

        public int CalculateCongestionTax(string[] dates)
        {
            int totalTax = 0;
            DateTime previousDate = DateTime.MinValue;
            int highestAmount = 0;

            foreach (string dateString in dates)
            {
                DateTime date = DateTime.Parse(dateString);

                // Skip if it's an exempt day
                if (IsExemptDay(date))
                    continue;

                // Skip if it's within 60 minutes of the previous date
                if ((date - previousDate).TotalMinutes <= 60)
                    continue;

                int amount = GetTaxAmount(date);
                highestAmount = Math.Max(highestAmount, amount);
                totalTax += amount;

                previousDate = date;
            }

            return Math.Min(totalTax, 60);
        }

        private bool IsExemptDay(DateTime date)
        {
            return exemptDays.ContainsKey(date.DayOfWeek)
                || date.Month == 7
                || IsPublicHoliday(date)
                || IsDayBeforePublicHoliday(date);
        }

        private bool IsPublicHoliday(DateTime date)
        {
            // Implement your logic for public holidays detection here
            return false;
        }

        private bool IsDayBeforePublicHoliday(DateTime date)
        {
            // Implement your logic for day before public holiday detection here
            return false;
        }

        private int GetTaxAmount(DateTime date)
        {
            int hour = date.Hour;
            int minute = date.Minute;

            foreach (var interval in timeIntervals)
            {
                if (hour < interval.Key || (hour == interval.Key && minute <= 29))
                    return interval.Value;
            }

            // Default tax amount
            return 0;
        }
    }
}

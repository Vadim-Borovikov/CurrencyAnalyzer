using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyAnalyzer
{
    internal class YearInfo
    {
        public YearInfo()
        {
            _byMounth = new Dictionary<uint, uint>();

            const uint MonthInYear = 12;
            for (uint m = 1; m <= MonthInYear; ++m)
            {
                _byMounth[m] = 0;
            }
        }

        public double Average => _byMounth.Values.Average(x => x);

        public void IncrementValueOf(DateTime date)
        {
            _byMounth[(uint) date.Month] = _byMounth[(uint) date.Month] + 1;
        }

        private readonly Dictionary<uint, uint> _byMounth;
    }
}
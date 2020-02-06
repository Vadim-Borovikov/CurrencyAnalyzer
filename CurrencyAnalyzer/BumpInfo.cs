using System;
using System.Collections.Generic;
using System.Linq;

namespace CurrencyAnalyzer
{
    internal class BumpInfo
    {
        public BumpInfo(uint startYear, uint finishYear)
        {
            _byYear = new Dictionary<uint, YearInfo>();

            for (uint y = startYear; y <= finishYear; ++y)
            {
                _byYear[y] = new YearInfo();
            }
        }

        public void IncrementValueOf(DateTime date)
        {
            YearInfo info = _byYear[(uint) date.Year];
            info.IncrementValueOf(date);
        }

        public double Average => _byYear.Average(y => y.Value.Average);

        private readonly Dictionary<uint, YearInfo> _byYear;
    }
}

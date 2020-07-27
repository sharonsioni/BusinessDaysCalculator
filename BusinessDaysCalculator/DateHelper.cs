using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessDaysCalculator
{
    /// <summary>
    /// This is a helper call which provides helper methods with dates.
    /// </summary>
    public class DateHelper
    {
        /// <summary>
        /// this generates a list of dates between given dates(exclusive).
        /// </summary>
        /// <param name="firstDate">from date(exclusive) to start listing dates from</param>
        /// <param name="secondDate">to date(exclusive) to stop listing dates</param>
        /// <returns></returns>
        public List<DateTime> GetSelectedDateRange(DateTime firstDate, DateTime secondDate)
        {
            var selectedDates = Enumerable
                                .Range(1, int.MaxValue)
                                .Select(index => (DateTime)(new DateTime?(firstDate.AddDays(index))))
                                .TakeWhile(date => date < secondDate)
                                .ToList();
            return selectedDates;
        }

        /// <summary>
        /// returns all weekdays dates for given date range
        /// </summary>
        /// <param name="selectedDateRange">date range to find week days for</param>
        /// <returns></returns>
        public List<DateTime> GetWeekDays(List<DateTime> selectedDateRange)
        {
            return selectedDateRange.FindAll(sd => sd.DayOfWeek != DayOfWeek.Sunday
                                                                      && sd.DayOfWeek != DayOfWeek.Saturday);
        }

        /// <summary>
        /// returns all weekends dates for given date range
        /// </summary>
        /// <param name="selectedDateRange">date range to find weekends for</param>
        /// <returns></returns>
        public List<DateTime> GetWeekEnds(List<DateTime> selectedDateRange)
        {
            return selectedDateRange.FindAll(sd => sd.DayOfWeek == DayOfWeek.Sunday
                                                                      || sd.DayOfWeek == DayOfWeek.Saturday);
        }

        /// <summary>
        /// This finds Nth occurrence of a given Day of Week in A given month
        /// </summary>
        /// <param name="dtmDate"></param>
        /// <param name="nOccurance"> Number specifying occurrence of a day</param>
        /// <param name="DayOfWeek"> Day of week (e.g Sunday, Monday ... Saturday) for which we need to find Nth occurrence</param>
        /// <returns></returns>
        public DateTime GetDayOfMonth(DateTime dtmDate, int nOccurance, int DayOfWeek)
        {
            // Get the first of the month.
            var dtmTemp = new DateTime(dtmDate.Year, dtmDate.Month, 1);

            // Get to the first Day of Week in the month.
            while (Convert.ToInt32(dtmTemp.DayOfWeek) != DayOfWeek)
                dtmTemp = dtmTemp.AddDays(1);

            // Now you've found the first Day of Week in the month.
            // Just add 7 for each N after that.
            return dtmTemp.AddDays((nOccurance - 1) * 7);
        }
    }
}

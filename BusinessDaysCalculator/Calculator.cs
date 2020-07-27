using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using BusinessDaysCalculator.Domain;

namespace BusinessDaysCalculator
{
    /// <summary>
    /// This class is responsible for calculating working days/ week days between give dates.
    /// </summary>
    public class Calculator
    {
        /// <summary>
        /// This counts number of weekdays between two dates (exclusive)
        /// </summary>
        /// <param name="firstDate">start date(exclusive)</param>
        /// <param name="secondDate">end date (exclusive)</param>
        /// <returns>Weekdays between given dates(exclusive) and error message if any errors present.</returns>
        public Tuple<int, string> WeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate)
        {
            try
            {
                // Check for Out of range exception.
                if (firstDate == DateTime.MinValue || secondDate == DateTime.MinValue
                    || firstDate == DateTime.MaxValue || secondDate == DateTime.MaxValue)
                {
                    return Tuple.Create(0, "Valid First and Second Dates are required for calculating business days");
                }
                else
                {
                    var DateHelper = new DateHelper();
                    var dateRange = DateHelper.GetSelectedDateRange(firstDate, secondDate);
                    var weekDays = DateHelper.GetWeekDays(dateRange);
                    return weekDays == null ? Tuple.Create(0, "error occurred while calculating business days")
                        : Tuple.Create(weekDays.Count, "");
                }

            }
            catch (Exception ex)
            {
                return Tuple.Create(0, "A fatal error occurred while calculating business days");
            }
        }

        /// <summary>
        /// Returns business days between given two dates exclusive taking out public holidays provided) 
        /// </summary>
        /// <param name="firstDate"> start date(exclusive) </param>
        /// <param name="secondDate">end date (exclusive)</param>
        /// <param name="publicHolidays">list of public holidays</param>
        /// <returns>Business days between given dates(exclusive) and error message if any errors present.</returns>
        public Tuple<int, string> BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<DateTime> publicHolidays)
        {
            try
            {
                var noOfPublicHolidays = 0;
                var weekDaysBetweenTwoDates = WeekdaysBetweenTwoDates(firstDate, secondDate);

                if (string.IsNullOrEmpty(weekDaysBetweenTwoDates.Item2)) // if no error then continue.
                {
                    if (publicHolidays != null)
                    {
                        noOfPublicHolidays = publicHolidays.ToList().FindAll(x => (firstDate.AddDays(1) <= x && x <= secondDate.AddDays(-1)
                                                          && x.DayOfWeek != DayOfWeek.Saturday
                                                          && x.DayOfWeek != DayOfWeek.Sunday)).Count;
                    }
                }

                return Tuple.Create(weekDaysBetweenTwoDates.Item1 - noOfPublicHolidays, weekDaysBetweenTwoDates.Item2);
            }
            catch (Exception ex)
            {
                return Tuple.Create(0, "A fatal error occurred while calculating business days");
            }
        }


        /// <summary>
        /// This takes two dates, applies rules specified for holidays and returns business days between two dates(exclusive) 
        /// </summary>
        /// <param name="firstDate"> date to start counting business day from (exclusive)</param>
        /// <param name="secondDate">date to stop counting business day to (exclusive) </param>
        /// <param name="holidayRule">list of rules specifying holidays</param>
        /// <returns> tuple containing no of Business days between two dates and validation message if validation failed </returns>
        public Tuple<int, string> BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, HolidayRule holidayRule)
        {
            List<DateTime> resultDateList = new List<DateTime>();
            List<DateTime> holidayOnWeekEndList = new List<DateTime>();
            List<DateTime> dateRange = new List<DateTime>();
            List<DateTime> selectedDates_Week_Days = new List<DateTime>();
            List<DateTime> selectedDates_WeekEnds = new List<DateTime>();

            try
            {
                string validationMessage = null;
                var holidayRuleValidator = new HolidayRuleValidator();

                if (holidayRule != null)
                {
                    validationMessage = holidayRuleValidator.ValidateHolidayRule(holidayRule);

                    // If rules configuration is valid allow to proceed further.
                    if (validationMessage == null)
                    {
                        var DateHelper = new DateHelper();

                        dateRange = DateHelper.GetSelectedDateRange(firstDate, secondDate);
                        selectedDates_Week_Days = DateHelper.GetWeekDays(dateRange);
                        selectedDates_WeekEnds = DateHelper.GetWeekEnds(dateRange);

                        // Find holidays which are always on same day and falls on week days.
                        foreach (Always_Same_Day always_Same_Day_Rule in holidayRule.Always_Same_Day)
                        {
                            resultDateList.AddRange(selectedDates_Week_Days.ToList().FindAll(d => d.Day == always_Same_Day_Rule.Day
                                                            && d.Month == always_Same_Day_Rule.Month
                                                            && !resultDateList.Any(rs => rs == d)));
                        }

                        // Find holidays which are always on same day except on weekend.
                        foreach (Always_On_Same_Day_Except_Weekend always_On_Same_Day_Except_Weekend in holidayRule.Always_On_Same_Day_Except_Weekend)
                        {
                            // Always on same day except on weekend, which are not on weekends
                            resultDateList.AddRange(selectedDates_Week_Days.ToList().FindAll(d => d.Day == always_On_Same_Day_Except_Weekend.Day
                                                           && d.Month == always_On_Same_Day_Except_Weekend.Month
                                                           && !resultDateList.Any(rs => rs == d)));

                            // So we are only interested in weekends which also are public holidays and are part of Always_On_Same_Day_Except_Weekend
                            holidayOnWeekEndList = selectedDates_WeekEnds.ToList().FindAll(d => d.Day == always_On_Same_Day_Except_Weekend.Day
                                                            && d.Month == always_On_Same_Day_Except_Weekend.Month);
                        }

                        // Find holidays which falls on Nth occurrence of given week day in given month.
                        foreach (Nth_Occurrence_Given_Month_Given_Week objRule in holidayRule.Nth_Occurance_Given_Month_Given_Week)
                        {
                            // We only need to consider week day as week end has already been taken care of.
                            resultDateList.AddRange(selectedDates_Week_Days.ToList().FindAll(d => d.Month == objRule.Month
                                                                         && Convert.ToInt32(d.DayOfWeek) == objRule.DayOfWeek
                                                                         && d.Date == DateHelper.GetDayOfMonth(d.Date
                                                                         , Convert.ToInt32(objRule.WeekOfMonth), Convert.ToInt32(objRule.DayOfWeek))));
                        }
                    }
                }

                return Tuple.Create(selectedDates_Week_Days.Count - (resultDateList.Count + holidayOnWeekEndList.Count), validationMessage);
            }
            catch (Exception ex)
            {
                return Tuple.Create(0, "A fatal error occurred while calculating business days");
            }
            finally
            {
                resultDateList = null;
                holidayOnWeekEndList = null;
                dateRange = null;
                selectedDates_Week_Days = null;
                selectedDates_WeekEnds = null;
            }

        }

        /// <summary>
        /// This get a list of rule from a JSON file and gives a business object back populating holiday rules.
        /// </summary>
        /// <param name="RuleJSON">TODO: This needs to be filled.</param>
        /// <returns>A business rule object containing all rules and inputs required for all rules.</returns>
        private HolidayRule GetRules(string filePath)
        {
            var RuleJSON = System.IO.File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<HolidayRule>(RuleJSON);
        }

    }
}

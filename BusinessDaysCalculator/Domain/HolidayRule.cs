using System.Collections.Generic;

namespace BusinessDaysCalculator.Domain
{
    /// <summary>
    /// Base class defining Holiday rule . THis will have collection of rules for each different rule type.
    /// </summary>
    public class HolidayRule
    {
        public List<Always_Same_Day> Always_Same_Day { get; set; } = new List<Always_Same_Day>();

        public List<Always_On_Same_Day_Except_Weekend> Always_On_Same_Day_Except_Weekend { get; set; } = new List<Always_On_Same_Day_Except_Weekend>();

        public List<Nth_Occurrence_Given_Month_Given_Week> Nth_Occurance_Given_Month_Given_Week { get; set; } = new List<Nth_Occurrence_Given_Month_Given_Week>();

    }

    /// <summary>
    /// Holiday rule type
    /// </summary>
    public enum BusinessRuleType
    {
        Always_Same_Day,
        Always_On_Same_Day_Except_Weekend,
        Nth_Occurance_Given_Month_Given_Week
    }
}

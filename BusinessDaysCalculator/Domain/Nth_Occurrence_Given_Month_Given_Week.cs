namespace BusinessDaysCalculator.Domain
{
    /// <summary>
    /// THis class specifies configuration for Given occurrence of given week day in a given month
    /// </summary>
    public class Nth_Occurrence_Given_Month_Given_Week : PublicHoliday
    {
        /// <summary>
        /// This specifies Day of Week (Sunday, Monday .... Friday)
        /// Nullable int to prevent falsely being set to 0 v/s not set at all.
        /// </summary>
        public int? DayOfWeek { get; set; }

        /// <summary>
        /// This specifies Week of Month ( 1st week, 2nd week, 3rd week and 4th week)
        /// Nullable int to prevent falsely being set to 0 v/s not set at all.
        /// </summary>
        public int? WeekOfMonth { get; set; }
    }
}

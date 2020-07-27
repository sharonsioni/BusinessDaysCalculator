namespace BusinessDaysCalculator.Domain
{
    /// <summary>
    /// This class specifies Configuration for always on same day rule.
    /// </summary>
    public class Always_Same_Day : PublicHoliday
    {
        /// <summary>
        /// This specifies Day of Month
        /// Nullable int to prevent falsely being set to 0 v/s not set at all.
        /// </summary>
        public int? Day { get; set; }
    }

}

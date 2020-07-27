namespace BusinessDaysCalculator.Domain
{
    /// <summary>
    /// Base class for all different kind of Holidays.
    /// </summary>
    public class PublicHoliday
    {
        /// <summary>
        /// Name of Holiday
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// Month Holidays falls onto (Jan, Feb .... Dec)
        /// Nullable int to prevent falsely being set to 0 v/s not set at all.
        /// </summary>
        public int? Month { get; set; }
    }

}

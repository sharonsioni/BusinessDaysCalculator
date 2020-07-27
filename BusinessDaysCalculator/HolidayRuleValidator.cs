using BusinessDaysCalculator.Domain;
using System.Collections.Generic;
using System.Linq;

namespace BusinessDaysCalculator
{
    /// <summary>
    /// This contains validation methods for validating input for Rule configurations.
    /// </summary>
    public class HolidayRuleValidator
    {
        /// <summary>
        /// Validate all input parameters has been set and have valid values.
        /// Validation is shared between Always_Same_Day and Always_On_Same_Day_Except_Weekend as both shares same input parameters.
        /// A separate validation method can be created for this is future if required. Always_On_Same_Day_Except_Weekend
        /// </summary>
        /// <param name="always_Same_Days"> collection of Always_Same_Day or Always_On_Same_Day_Except_Weekend Rule to be validated.</param>
        /// <returns>message containing why validation failed if validation failed if not failed will return null.</returns>
        private string Validate_Rule_Always_Same_Day<T>(List<T> always_Same_Days) where T : Always_Same_Day
        {
            string errorMessages = null;

            // No rules are defined, so this should be considered as valid.
            if (always_Same_Days != null)
            {
                if (always_Same_Days.Any(r => r.Day == null || r.Month == null))
                {
                    errorMessages = "Validation failed - Rule - Always on same day: Day and month of holiday is required";
                }
                else if (always_Same_Days.Any(r => r.Month <= 0 || r.Month > 12))
                {
                    errorMessages = "Validation failed - Rule - Always on same day: Month should be between 1 and 12 (inclusive)";
                }
                // check days in February is valid or not
                else if (always_Same_Days.Any(r => r.Month == 2 && r.Day > 29))
                {
                    errorMessages = "Validation failed - Rule - Always on same day: Day set for February can not be greater than 29";
                }

                // check days configured for 30 days months
                // April, June, Sept, November
                else if (always_Same_Days.Any(r => (r.Month == 4 || r.Month == 6 || r.Month == 9 || r.Month == 11) && r.Day > 30))
                {
                    errorMessages = "For months April, June, Sept and November Days can not be > 30";
                }

                // check days configured for 30 days months
                // Jan, March, May, July, August,October, December
                else if (always_Same_Days.Any(r => (r.Month == 4 || r.Month == 6 || r.Month == 9 || r.Month == 11) && r.Day > 30))
                {
                    errorMessages = "For months Jan, March, May, July, August,October and December Days can not be > 31";
                }
            }

            return errorMessages;
        }

        /// <summary>
        /// Validate all input parameters has been set and have valid values.
        /// </summary>
        /// <param name="rules">List of Nth_Occurrence_Given_Month_Given_Week to be validated</param>
        /// <returns>message containing why validation failed if validation failed if not failed will return null.</returns>
        private string Validate_Rule_Nth_Occurance_Given_Month_Given_Week(List<Nth_Occurrence_Given_Month_Given_Week> rules)
        {
            string errorMessages = null;

            // No rules are defined, so this should be considered as valid.
            if (rules != null)
            {
                if (rules.Any(r => r.WeekOfMonth == null || r.DayOfWeek == null))
                {
                    errorMessages = "Validation failed - Rule - Nth Occurrence Given Month Given Week: WeekOfMonth and DayOfWeek of holiday is required";
                }
                else if (rules.Any(r => r.WeekOfMonth <= 0 || r.WeekOfMonth > 4))
                {
                    errorMessages = "Validation failed - Rule - Nth Occurrence Given Month Given Week: WeekOfMonth should be between 1 and 4 (inclusive)";
                }
                else if (rules.Any(r => r.DayOfWeek <= 0 && r.DayOfWeek > 7))
                {
                    errorMessages = "Validation failed - Rule - Nth Occurrence Given Month Given Week: DayOfWeek should be between 1 and 7 (inclusive)";
                }
            }

            return errorMessages;
        }

        /// <summary>
        /// This method checks if validation rules have been set properly for all rules if not returns reason explaining what has not been set.
        /// </summary>
        /// <param name="holidayRule">List of rules to be validated.</param>
        /// <returns>Text explaining what has not been set properly for which rule. If all configuration valid returns null </returns>
        public string ValidateHolidayRule(HolidayRule holidayRule)
        {
            string validationMessage = null;
            var ruleConfigurationValid = true;
            HolidayRuleValidator holidayRuleValidator = new HolidayRuleValidator();

            // Check if configuration valid for Always_Same_Day
            if (holidayRule.Always_Same_Day != null)
            {
                validationMessage = holidayRuleValidator.Validate_Rule_Always_Same_Day<Always_Same_Day>(holidayRule.Always_Same_Day);

                if (validationMessage != null)
                {
                    ruleConfigurationValid = false;
                }
            }

            // Check if configuration is valid for Always_On_Same_Day_Except_Weekend
            if (ruleConfigurationValid && holidayRule.Always_On_Same_Day_Except_Weekend != null)
            {
                validationMessage = holidayRuleValidator.Validate_Rule_Always_Same_Day<Always_On_Same_Day_Except_Weekend>(holidayRule.Always_On_Same_Day_Except_Weekend);

                if (validationMessage != null)
                {
                    ruleConfigurationValid = false;
                }
            }

            // Check if configuration is valid for Always_On_Same_Day_Except_Weekend
            if (ruleConfigurationValid && holidayRule.Nth_Occurance_Given_Month_Given_Week != null)
            {
                validationMessage = holidayRuleValidator.Validate_Rule_Nth_Occurance_Given_Month_Given_Week(holidayRule.Nth_Occurance_Given_Month_Given_Week);
            }

            return validationMessage;
        }
    }
}

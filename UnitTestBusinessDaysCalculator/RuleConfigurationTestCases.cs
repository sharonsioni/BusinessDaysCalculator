using BusinessDaysCalculator;
using BusinessDaysCalculator.Domain;
using NUnit.Framework;
using System;

namespace UnitTestBusinessDaysCalculator
{
    [TestFixture]
    public class RuleConfigurationTestCases
    {
        private Calculator _calculator;
        [SetUp]
        public void Setup()
        {
            _calculator = new Calculator();
        }

        [Test]
        // This is to test configuration is valid for rule always on same day.
        public void Test_Validate_Business_Days_Rules_Alway_On_Same_Day()
        {
            HolidayRule holidayRule = new HolidayRule();
            holidayRule.Always_Same_Day.Add(new Always_Same_Day()
            { Name = "Anzac Day", Day = 40, Month = 30 });

            var result = _calculator.BusinessDaysBetweenTwoDates(new DateTime(2019, 4, 1), new DateTime(2019, 4, 30), holidayRule);
            Assert.NotNull(result.Item2);
        }

        [Test]
        // This is to test configuration is valid for rule always on same day except weekend.
        public void Test_Validate_Business_Days_Rules_Always_On_Same_Day_Except_Weekend()
        {
            HolidayRule holidayRule = new HolidayRule();
            holidayRule.Always_On_Same_Day_Except_Weekend.Add(new Always_On_Same_Day_Except_Weekend()
            { Name = "New Year Day", Day = 40, Month = 40 });

            var result = _calculator.BusinessDaysBetweenTwoDates(new DateTime(2019, 4, 1), new DateTime(2019, 4, 30), holidayRule);
            Assert.NotNull(result.Item2);
        }

        [Test]
        // This is to test configuration is valid for rule always on same day except weekend.
        public void Test_Validate_Business_Days_Rules_Nth_Occurance_Given_Month_Given_Week()
        {
            HolidayRule holidayRule = new HolidayRule();
            holidayRule.Nth_Occurance_Given_Month_Given_Week.Add(new Nth_Occurrence_Given_Month_Given_Week()
            { Name = "Queens Birthday", DayOfWeek = 12, WeekOfMonth = 60 });

            var result = _calculator.BusinessDaysBetweenTwoDates(new DateTime(2019, 4, 1), new DateTime(2019, 4, 30), holidayRule);
            Assert.NotNull(result.Item2);
        }
    }
}

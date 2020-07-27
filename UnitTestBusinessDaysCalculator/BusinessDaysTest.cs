using NUnit.Framework;
using System;
using System.Collections.Generic;
using BusinessDaysCalculator;
using BusinessDaysCalculator.Domain;

namespace UnitTestBusinessDaysCalculator
{
    [TestFixture]
    public class BusinessDaysTest
    {
        private Calculator _calculator;
        [SetUp]
        public void Setup()
        {
            _calculator = new Calculator();
        }

        [Test]
        // This test if min or max value of date time has been provided then no exception should eb raised.
        public void Test_Dates_Invalid_Values()
        {
            var r1 = _calculator.WeekdaysBetweenTwoDates(DateTime.MinValue, DateTime.MaxValue);
            Assert.IsNotNull(r1.Item2);
        }

        [Test]
        // This is to test rule if same start date and end date are provided code doesn't raise any exceptions 
        // and returns valid business days which should be 0.
        public void Test_Same_Start_End_Date()
        {
            var r1 = _calculator.WeekdaysBetweenTwoDates(new DateTime(2013, 10, 7), new DateTime(2013, 10, 7));
            Assert.AreEqual(r1.Item1, 0);

            var publicHolidats = new List<DateTime>();
            publicHolidats.Add(new DateTime(2013, 12, 25));
            publicHolidats.Add(new DateTime(2013, 12, 26));
            publicHolidats.Add(new DateTime(2014, 1, 1));
            var r2 = _calculator.BusinessDaysBetweenTwoDates(new DateTime(2013, 10, 7), new DateTime(2013, 10, 7), publicHolidats);
            Assert.AreEqual(r2.Item1, 0);


            HolidayRule holidayRule = new HolidayRule();
            holidayRule.Always_Same_Day.Add(new Always_Same_Day()
            { Name = "Anzac Day", Day = 25, Month = 4 });
            var result = _calculator.BusinessDaysBetweenTwoDates(new DateTime(2019, 4, 1), new DateTime(2019, 4, 1), holidayRule);
            Assert.AreEqual(result.Item1, 0);
        }

        [Test]
        // This is to test weekdays between two dates (exclusive).
        public void Test_Calculate_WeekDays_Days()
        {
            var r1 = _calculator.WeekdaysBetweenTwoDates(new DateTime(2013, 10, 7), new DateTime(2013, 10, 9));
            Assert.AreEqual(r1.Item1, 1);

            var r2 = _calculator.WeekdaysBetweenTwoDates(new DateTime(2013, 10, 5), new DateTime(2013, 10, 14));
            Assert.AreEqual(r2.Item1, 5);

            var r3 = _calculator.WeekdaysBetweenTwoDates(new DateTime(2013, 10, 7), new DateTime(2014, 1, 1));
            Assert.AreEqual(r3.Item1, 61);

            var r4 = _calculator.WeekdaysBetweenTwoDates(new DateTime(2013, 10, 7), new DateTime(2013, 10, 5));
            Assert.AreEqual(r4.Item1, 0);

        }

        [Test]
        // This is to get business days between two dates(exclusive) considering public holidays.
        public void Test_Calculate_Business_Days()
        {
            var publicHolidats = new List<DateTime>();
            publicHolidats.Add(new DateTime(2013, 12, 25));
            publicHolidats.Add(new DateTime(2013, 12, 26));
            publicHolidats.Add(new DateTime(2014, 1, 1));

            var r1 = _calculator.BusinessDaysBetweenTwoDates(new DateTime(2013, 10, 7), new DateTime(2013, 10, 9), publicHolidats);
            Assert.AreEqual(r1.Item1, 1);

            var r2 = _calculator.BusinessDaysBetweenTwoDates(new DateTime(2013, 12, 24), new DateTime(2013, 12, 27), publicHolidats);
            Assert.AreEqual(r2.Item1, 0);

            var r3 = _calculator.BusinessDaysBetweenTwoDates(new DateTime(2013, 10, 7), new DateTime(2014, 1, 1), publicHolidats);
            Assert.AreEqual(r3.Item1, 59);

        }

        [Test]
        // This is to test configuration is valid for rule given occurrence of given week day for a given month.
        public void Test_Calculate_Business_Days_Rules_Alway_On_Same_Day()
        {
            HolidayRule holidayRule = new HolidayRule();
            holidayRule.Always_Same_Day.Add(new Always_Same_Day() 
            { Name = "Anzac Day" ,Day = 25, Month = 4 });

            var result = _calculator.BusinessDaysBetweenTwoDates(new DateTime(2019, 4, 1), new DateTime(2019, 4, 30), holidayRule);
            Assert.AreEqual(19, result.Item1);
        }

        [Test]
        // This is to test business days calculation with rule always on same day except weekend, when next working day should be considered. 
        public void Test_Calculate_Business_Days_Rules_Always_On_Same_Day_Except_Weekend()
        {
            HolidayRule holidayRule = new HolidayRule();
            holidayRule.Always_On_Same_Day_Except_Weekend.Add(new Always_On_Same_Day_Except_Weekend() 
            { Name = "New Year Day", Day = 1, Month = 1 });

            var result = _calculator.BusinessDaysBetweenTwoDates(new DateTime(2017, 12, 30), new DateTime(2018, 1, 5), holidayRule);
            Assert.AreEqual(3, result.Item1);
        }

        [Test]
        // This is to test business days calculation with rule given occurrence of given week day for a given month.
        public void Test_Calculate_Business_Days_Rules_Nth_Occurance_Given_Month_Given_Week()
        {
            HolidayRule holidayRule = new HolidayRule();
            holidayRule.Nth_Occurance_Given_Month_Given_Week.Add(new Nth_Occurrence_Given_Month_Given_Week() 
            { Name = "Queens Birthday", DayOfWeek = Convert.ToInt32(DayOfWeek.Monday), Month = 6 , WeekOfMonth = 2 });

            var result = _calculator.BusinessDaysBetweenTwoDates(new DateTime(2020, 6, 1), new DateTime(2020, 6, 19), holidayRule);
            Assert.AreEqual(12, result.Item1);
        }
    }
}
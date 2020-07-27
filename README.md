# BusinessDays_Calculator
This a class library with Unit Testing to calculate business days between two dates.

This a class library with Unit Testing to calculate business days between two dates.

The class library consist of below methods to calculate no of days between two dates.

The class library has below Methods to calculate Week Days and Business days.

1. Calculate Week days between Two Dates.

 public Tuple<int, string> WeekdaysBetweenTwoDates(DateTime firstDate, DateTime secondDate)
 {
            
 }

        This methods takes firstDate and secondDate and calculates weekdays between these two dates (exclusive)

        Method returns a Tuple with first item being no of days between dates and second item being any errors if present e.g Dates are not valid. 
      

2. Calculate Business days between two dates considering public holidays provided.

public Tuple<int, string> BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, IList<DateTime> publicHolidays)
{

}

        This methods takes firstDate and secondDate and list of dates describing holidays and calculates business 
        days between these two dates (exclusive) excluding holidays provided

        Method returns a Tuple with first item being no of days between dates and second item being any errors if present. 


3. Calculate Business days between two dates (Exclusive) with given Holiday rules.
public Tuple<int, string> BusinessDaysBetweenTwoDates(DateTime firstDate, DateTime secondDate, HolidayRule holidayRule)
{

}

        This methods takes firstDate and secondDate and calculates weekdays between these two dates (exclusive).

        Third argument is List of Holiday rules which needs to be looked at when calculating business days.

        Definition of class is as below

        public List<Always_Same_Day> Always_Same_Day { get; set; } = new List<Always_Same_Day>();

        public List<Always_On_Same_Day_Except_Weekend> Always_On_Same_Day_Except_Weekend { get; set; } = new List<Always_On_Same_Day_Except_Weekend>();

        public List<Nth_Occurrence_Given_Month_Given_Week> Nth_Occurance_Given_Month_Given_Week { get; set; } = new List<Nth_Occurrence_Given_Month_Given_Week>();

        Rules being catered are below.

        1. Holidays which always falls on same day. 

        This rule can be configured setting below properties of Always_Same_Day class.

        Day : Day of Month
              Nullable int to prevent falsely being set to 0 v/s not set at all.

        Month: Integer value for Month Holidays falls onto (Jan = 1, Feb = 2 .... Dec = 12)
               This is a Nullable int to prevent falsely being set to 0 v/s not set at all.

        2. Holidays which falls on same day except when it falls on weekend.

        This rule can be configured setting below properties of class.

        Day : Day of Month
              Nullable int to prevent falsely being set to 0 v/s not set at all.

        Month: Integer value for Month Holidays falls onto (Jan = 1, Feb = 2 .... Dec = 12)
               This is a Nullable int to prevent falsely being set to 0 v/s not set at all.

        3. holidays which falls on certain occurrence of certain day in a certain month.

        This rule can be configured setting below properties of class.

        DayOfWeek:  This specifies Integer value for Day of Week (Sunday = 0 , Monday = 1 .... Friday)
                    Nullable int to prevent falsely being set to 0 v/s not set at all.

        WeekOfMonth: This specifies Integer value Week of Month ( 1 = 1st week, 2 = 2nd week, 3 = 3rd week and 4 = 4th week)
                     Nullable int to prevent falsely being set to 0 v/s not set at all.

Method returns a tuple with first items as No of days and second item as sting stating any validation errors while configuring rules. 

Unit test project.

This has been written using NUnit.

Two types of test have been written.

1. To test functionality of methods to calculate no of days between two dates.

2. To test Holidays rules has been setup properly or not. 

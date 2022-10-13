# WorkdayCalendar
Contains code for solving the WorkdayCalendar task.

[![.NET](https://github.com/chiefi/WorkdayCalendar/actions/workflows/dotnet.yml/badge.svg)](https://github.com/chiefi/WorkdayCalendar/actions/workflows/dotnet.yml)


### Problem description 
We need a way to calculate where we end up if we go back or forth x number of working days, from a given point in time.

A working day is defined as a day from Monday to Friday which is not a holiday.

We need to be able to set which days shall be regarded as holidays.
Holidays shall not count as a working day.

We also need to be able to register recurring holidays. A recurring holiday says that the given date is to be regarded as a holiday on the same date every year.

We need to be able to set when a working day starts and stops.
We need to be able to add some working days to a given start datetime and get the resulting datetime. The date in the result must be a workday.

The time in the result must be within the working hours set by the start and stop time, even though the start date need not follow this rule.
According to this rule then 15:07 + 0.25 working days will be 9:07, and 4:00 + 0.5 working days will be 12:00.


### Features
- Calculate where we end up if we go back or forth x number of working days, from a given point in time.
- Saturdays and Sundays are holidays.
- Possibility to add holidays and recurring holidays.
- Can set StartOfWorkday and EndOfWorkday, default 08:00 - 16:00.



### Platforms
.NET 6

### License
Free for commercial use under the MIT License 2.0.

### Usage
```csharp
> WorkdayCalendar.exe interactive
```

```csharp
> WorkdayCalendar.exe calc --startDate="2022-10-10 07:30" --increment=3
```

```
  --startDate       Required. Specify the date to start calculating from. Should be in format yyyy-MM-dd HH:mm
  --increment	    Required. The steps to calculate from the start date. Can be positive or negative and may contain decimals.

```

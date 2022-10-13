using Microsoft.Extensions.Logging;
using WorkdayCalendarLibrary.Model;
using WorkdayCalendarLibrary.Util;

namespace WorkdayCalendarLibrary.Service;

/// <summary>
/// Service to help find the closest workday to a start date using an offset.
/// </summary>
public interface ICalendarService
{
    /// <summary>
    /// Finds the workday and worktime from the supplied start date using the supplied workday offset.
    /// </summary>
    /// <param name="calendar">The calendar to use for determining workdays and working hours.</param>
    /// <param name="startDate">The DateTime object to start from.</param>
    /// <param name="workdays">The amount of workdays, given as a positive or negative double, to add to the start date.</param>
    /// <exception cref="InvalidOperationException">Thrown when workdays are not configured correctly. StartOfDay must be earlier than EndOfDay.</exception>
    /// <returns>A DateTime object containing the new Workday/WorkTime.</returns>
    DateTime FindWorkday(Calendar calendar, DateTime startDate, double workdays);
}

public class CalendarService : ICalendarService
{
    private readonly ILogger<CalendarService> logger;

    public CalendarService(ILogger<CalendarService> logger)
    {
        this.logger = logger;
    }

    public DateTime FindWorkday(Calendar calendar, DateTime startDate, double workdays)
    {
        var workDayLength = calendar.GetLengthOfWorkday();
        var currentDate = startDate;

        var minutes = workdays * workDayLength;

        logger.LogDebug("Calculating the new result from {StartDate} with {Workdays} workdays steps.", startDate, workdays);
        logger.LogDebug("Workday is {StartDate} and we need to travel {Minutes} minutes.", startDate, minutes);

        currentDate = workdays >= 0
            ? IncrementDateAndTime(currentDate, minutes, calendar)
            : SubtractMinutes(currentDate, -minutes, calendar);

        return currentDate.SetSecondsToZero();
    }

    private DateTime IncrementDateAndTime(DateTime date, double minutes, Calendar calendar)
    {
        if (TimeOnly.FromDateTime(date) < calendar.StartOfWorkday)
        {
            logger.LogDebug("Time ({Date}) is before StartOfWorkDay ({StartOfWorkday}). Setting Current Time to StartOfWorkday.", date, calendar.StartOfWorkday);
            date = date.SetTime(calendar.StartOfWorkday.Hour, calendar.StartOfWorkday.Minute);
        }

        if (TimeOnly.FromDateTime(date) >= calendar.EndOfWorkday)
        {
            logger.LogDebug("Time ({Date}) is after EndOfWorkday ({EndOfWorkDay}). Moving forward to StartOfWorkday ({StartOfWorkday}) on next working day.", date, calendar.EndOfWorkday, calendar.StartOfWorkday);
            date = calendar.MoveToNextWorkday(date);
        }

        if (!calendar.IsWorkday(date))
        {
            logger.LogDebug("Starting Day ({Date}) is not a workday. Moving forward to StartOfWorkday ({StartOfWorkday}) on next working day.", date, calendar.StartOfWorkday);
            date = calendar.MoveToNextWorkday(date);
        }

        var minutesRemainingInWorkday = (calendar.EndOfWorkday - TimeOnly.FromDateTime(date)).TotalMinutes;

        while (minutes > minutesRemainingInWorkday)
        {
            date = calendar.MoveToNextWorkday(date);

            logger.LogDebug("We are incrementing more minutes ({Minutes}) then we have left on the working day ({MinutesRemainingInWorkday}). Moving forward to StartOfWorkday ({StartOfWorkday}) on next working day ({Date}).", minutes, minutesRemainingInWorkday, calendar.StartOfWorkday, date);

            minutes -= minutesRemainingInWorkday;
            minutesRemainingInWorkday = (calendar.EndOfWorkday - TimeOnly.FromDateTime(date)).TotalMinutes;
        }

        date = date.AddMinutes(minutes);
        logger.LogDebug("We are incrementing ({Minutes}) minutes within the working day which gives us ({Date}).", minutes, date);

        return date;
    }

    private DateTime SubtractMinutes(DateTime date, double minutes, Calendar calendar)
    {
        if (TimeOnly.FromDateTime(date) > calendar.EndOfWorkday)
        {
            logger.LogDebug("Time ({Date}) is after EndOfWorkday ({EndOfWorkday}). Moving backwards to EndOfWorkday ({EndOfWorkday}).", date, calendar.EndOfWorkday, calendar.EndOfWorkday);
            date = date.SetTime(calendar.EndOfWorkday.Hour, calendar.EndOfWorkday.Minute);
        }

        if (TimeOnly.FromDateTime(date) < calendar.StartOfWorkday)
        {
            logger.LogDebug("Time ({Date}) is before StartOfWorkDay ({StartOfWorkday}). Setting Current Time to EndOfWorkday ({EndOfWorkday}) on the previous working day.", date, calendar.StartOfWorkday, calendar.EndOfWorkday);
            date = calendar.MoveToPreviousWorkday(date);
        }

        if (!calendar.IsWorkday(date))
        {
            logger.LogDebug("Starting Day ({Date}) is not a workday. Moving backwards to EndOfWorkday ({EndOfWorkday}) on previous working day.", date, calendar.EndOfWorkday);
            date = calendar.MoveToPreviousWorkday(date);
        }

        var minutesPassedInWorkday = (TimeOnly.FromDateTime(date) - calendar.StartOfWorkday).TotalMinutes;

        while (minutesPassedInWorkday < minutes)
        {
            date = calendar.MoveToPreviousWorkday(date);

            logger.LogDebug("We are decrementing more minutes ({Minutes}) then have passed on the working day ({MinutesPassedInWorkday}). Moving backwards to EndOfWorkday ({EndOfWorkday}) on previous working day ({Date}).", minutes, minutesPassedInWorkday, calendar.EndOfWorkday, date);

            minutes -= minutesPassedInWorkday;
            minutesPassedInWorkday = (TimeOnly.FromDateTime(date) - calendar.StartOfWorkday).TotalMinutes;
        }

        date = date.AddMinutes(-minutes);
        logger.LogDebug("We are decrementing ({Minutes}) minutes within the working day which gives us ({Date}).", minutes, date);

        return date;
    }
}

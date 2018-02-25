using System;

public static class TimeSpanExtensions
{
    /// <summary>
    /// Rounds the time to the nearest X minutes
    /// </summary>
    /// <param name="input">Time to round</param>
    /// <param name="minutes">Rounding interval</param>
    /// <returns>Rounded time</returns>
    // Solution by Larry C - https://stackoverflow.com/questions/24006244/roundoff-timespan-to-15-min-interval
    public static TimeSpan RoundToNearestMinutes(this TimeSpan input, int minutes)
    {
        var halfRange = new TimeSpan(0, minutes / 2, 0);
        if (input.Ticks < 0)
            halfRange = halfRange.Negate();
        var totalMinutes = (int)(input + halfRange).TotalMinutes;
        return new TimeSpan(0, totalMinutes - totalMinutes % minutes, 0);
    }
}
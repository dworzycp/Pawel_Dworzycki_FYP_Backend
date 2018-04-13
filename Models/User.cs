/**
 * This class models a User
 *
 * @author Pawel Dworzycki
 * @version 13/04/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;

class User
{

    public string userId { get; private set; }
    public Day[] days { get; private set; }
    public Dictionary<string, Cluster> idToClusterMap;
    public List<GeoPoint> points;           // All points
    public List<GeoPoint> unassignedPoints; // Points which haven't been assigned a cluster yet

    public User(string userId)
    {
        this.userId = userId;
        this.idToClusterMap = new Dictionary<string, Cluster>();
        points = new List<GeoPoint>();
        unassignedPoints = new List<GeoPoint>();

        initiliseDaysOfWeek();
    }

    /// <summary>
    /// Creates a day for each day in a week
    /// </summary>
    private void initiliseDaysOfWeek()
    {
        days = new Day[7];
        days[0] = new Day(DayOfWeek.Monday);
        days[1] = new Day(DayOfWeek.Tuesday);
        days[2] = new Day(DayOfWeek.Wednesday);
        days[3] = new Day(DayOfWeek.Thursday);
        days[4] = new Day(DayOfWeek.Friday);
        days[5] = new Day(DayOfWeek.Saturday);
        days[6] = new Day(DayOfWeek.Sunday);
    }

    /// <summary>
    /// Returns a Day model for the corresponding DayOfWeek
    /// </summary>
    /// <param name="dayOfWeek">Day of week to be returned</param>
    /// <returns>Day model</returns>
    public Day GetDay(DayOfWeek dayOfWeek)
    {
        Day day = null;
        switch (dayOfWeek)
        {
            case DayOfWeek.Monday:
                day = days[0];
                break;
            case DayOfWeek.Tuesday:
                day = days[1];
                break;
            case DayOfWeek.Wednesday:
                day = days[2];
                break;
            case DayOfWeek.Thursday:
                day = days[3];
                break;
            case DayOfWeek.Friday:
                day = days[4];
                break;
            case DayOfWeek.Saturday:
                day = days[5];
                break;
            case DayOfWeek.Sunday:
                day = days[6];
                break;
        }
        return day;
    }

}
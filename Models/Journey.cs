/**
 * This class models a journey
 *
 * @author Pawel Dworzycki
 * @version 15/04/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;

class Journey
{

    public DateTime startTime { get; private set; }
    public DateTime endTime { get; private set; }

    public int startClusterID { get; private set; }
    public int endClusterID { get; private set; }

    public string userId { get; private set; }

    public Journey(DateTime startTime, DateTime endTime, int startClusterID, int endClusterID, string userId)
    {
        this.startTime = startTime;
        this.endTime = endTime;
        this.startClusterID = startClusterID;
        this.endClusterID = endClusterID;
        this.userId = userId;
    }

    public double LengthInMins()
    {
        TimeSpan travelTime = endTime - startTime;
        return travelTime.TotalMinutes;
    }

    public override string ToString()
    {
        return "Origin cluster " + startClusterID + " at " + startTime + ". Destination cluster " + endClusterID + " at " + endTime + ". Journey's length " + LengthInMins() + "mins.";
    }

}
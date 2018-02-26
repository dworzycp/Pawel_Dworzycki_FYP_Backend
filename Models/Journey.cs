/**
 * This class models a journey
 *
 * @author Pawel Dworzycki
 * @version 26/02/2018
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

    public Journey(DateTime startTime, DateTime endTime, int startClusterID, int endClusterID)
    {
        this.startTime = startTime;
        this.endTime = endTime;

        this.startClusterID = startClusterID;
        this.endClusterID = endClusterID;
    }

    public double LengthInMins()
    {
        TimeSpan travelTime = endTime - startTime;
        return travelTime.TotalMinutes;
    }

    public override string ToString()
    {
        return "Left cluster " + startClusterID + " at " + startTime + ". Entered cluster " + endClusterID + " at " + endTime + ". Journey's length " + LengthInMins() + "mins." ;
    }

}
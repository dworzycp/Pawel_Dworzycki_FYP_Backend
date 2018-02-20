/**
 * This class models a day for a cluster of co-ordinates
 * @author Pawel Dworzycki
 *
 * @version 17/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;


class ClusterDay
{
    public List<Point> points;

    public int day = 0; // Numeric representation of the day for this cluster. 1 = Monday ... 7 = Sunday

    public DateTime avgEnterTime;
    public DateTime avgLeaveTime;

    public Cluster nextCluster = null;

    public ClusterDay()
    {
        points = new List<Point>();
    }

    public double journeyTimeToNextClusterInMins()
    {
        if (nextCluster == null)
            throw new Exception("nextCluster isn't set");

        if (day == 0)
            throw new Exception("Day for this cluster hasn't been set");

        try
        {
            // Find the next same day in the cluster
            ClusterDay nextDay = new ClusterDay();
            foreach (ClusterDay cd in nextCluster.days.Values)
            {
                if (cd.day == day)
                {
                    nextDay = cd;
                    break;
                }
            }

            TimeSpan travelTime = nextDay.avgEnterTime - avgLeaveTime;
            return travelTime.TotalMinutes;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

}
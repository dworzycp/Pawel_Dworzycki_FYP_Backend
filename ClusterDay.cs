/**
 * This class models a day for a cluster of co-ordinates
 * @author Pawel Dworzycki
 *
 * @version 21/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;


class ClusterDay
{
    public List<GeoPoint> points;

    public DayOfWeek day;

    public DateTime avgEnterTime;
    public DateTime avgLeaveTime;

    public Dictionary<Cluster, int> historialNextClusters;
    public Cluster nextCluster = null;

    public ClusterDay()
    {
        points = new List<GeoPoint>();
        historialNextClusters = new Dictionary<Cluster, int>();
    }

    public double journeyTimeToNextClusterInMins()
    {
        if (nextCluster == null)
            throw new Exception("nextCluster isn't set");

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

            TimeSpan travelTime = nextDay.avgLeaveTime - avgEnterTime;
            return travelTime.TotalMinutes;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

}
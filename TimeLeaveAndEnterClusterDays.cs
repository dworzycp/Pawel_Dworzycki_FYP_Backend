/**
 * This class works out at what time the user entered and left a cluster
 * @author Pawel Dworzycki
 *
 * @version 09/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

class TimeLeaveAndEnterClusterDays
{

    public TimeLeaveAndEnterClusterDays(List<Point> points)
    {
        EnterAndLeaveTime(points);
    }

    /**
         * A cluster has been entered if points are:
         * P(!c), P(c), P(c)
         *
         * A cluster has been left if points are:
         * P(c), P(c), P(!c), P(!c)
         */
    private void EnterAndLeaveTime(List<Point> points)
    {
        foreach (Point p in points)
        {
            // TODO if someone does enter or leave the cluster at left most or right most value
            // TODO for now it will be ignored

            int indexOfPoint = points.IndexOf(p);

            if (indexOfPoint == 0 || indexOfPoint == 1) { } // Ignore left most 2 values
            else if (indexOfPoint == points.Count - 1 || indexOfPoint == points.Count - 2) { }  // Ignore right most 2 values
            else
            {
                // Check that the point belongs to a cluster
                if (p.cluster != null)
                {
                    // Get neghbouring clusters
                    Cluster leftCluster = null;
                    if (points[indexOfPoint - 1].cluster != null)
                        leftCluster = points[indexOfPoint - 1].cluster;
                    Cluster left2Cluster = null;
                    if (points[indexOfPoint - 2].cluster != null)
                        left2Cluster = points[indexOfPoint - 2].cluster;

                    Cluster rightCluster = null;
                    if (points[indexOfPoint + 1].cluster != null)
                        rightCluster = points[indexOfPoint + 1].cluster;
                    Cluster right2Cluster = null;
                    if (points[indexOfPoint + 2].cluster != null)
                        right2Cluster = points[indexOfPoint + 2].cluster;

                    // Firstly check if the leftCluster is the same
                    if (leftCluster == p.cluster)
                    {
                        if (left2Cluster == p.cluster)
                        {
                            // Check if the righCluster is the different
                            if (rightCluster != p.cluster && right2Cluster != p.cluster)
                            {
                                // This is the leave point
                                // Set leave time for the day to the point's creation date
                                DateTime time = DateTime.ParseExact(p.createdAt.ToShortTimeString(), "HH:mm", CultureInfo.InvariantCulture);
                                p.cluster.days[p.createdAt.ToShortDateString()].avgLeaveTime = time;
                                //Console.WriteLine("Left " + p.cluster.name + " on " + p.createdAt.ToShortDateString() + " at " + time);
                            }
                        }
                    }
                    else
                    {
                        // Check if the righCluster is the same
                        if (rightCluster == p.cluster && right2Cluster == p.cluster)
                        {
                            // This is the enter point
                            // Set enter time for the day to the point's creation date
                            DateTime time = DateTime.ParseExact(p.createdAt.ToShortTimeString(), "HH:mm", CultureInfo.InvariantCulture);
                            p.cluster.days[p.createdAt.ToShortDateString()].avgEnterTime = time;
                            //Console.WriteLine("Entered " + p.cluster.name + " on " + p.createdAt.ToShortDateString() + " at " + time);
                        }
                    }
                }
            }
        }
    }

}
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

class HistoricalJourneys
{
    // For predicting which cluster a user will go to next
    GeoPoint clusterBeingLeft = null;

    public HistoricalJourneys(List<GeoPoint> points, User user)
    {
        EnterAndLeaveTime(points, user);
    }

    /**
     * A cluster has been entered if points are:
     * P(!c), P(c), P(c)
     *
     * A cluster has been left if points are:
     * P(c), P(c), P(!c), P(!c)
     */
    private void EnterAndLeaveTime(List<GeoPoint> points, User user)
    {
        foreach (GeoPoint p in points)
        {
            int indexOfPoint = points.IndexOf(p);

            // TODO if someone does enter or leave the cluster at left most or right most 2 values -- for now it will be ignored
            if (indexOfPoint == 0 || indexOfPoint == 1) { } // Ignore left most 2 values
            else if (indexOfPoint == points.Count - 1 || indexOfPoint == points.Count - 2) { }  // Ignore right most 2 values
            else
            {
                // Check that the point belongs to a cluster
                if (p.ClusterId != 0 || p.ClusterId != -1)
                {
                    // Get neghbouring clusters
                    int leftCluster = 0;
                    if (points[indexOfPoint - 1].ClusterId != 0 || points[indexOfPoint - 1].ClusterId != -1)
                        leftCluster = points[indexOfPoint - 1].ClusterId;
                    int left2Cluster = 0;
                    if (points[indexOfPoint - 2].ClusterId != 0 || points[indexOfPoint - 2].ClusterId != -1)
                        left2Cluster = points[indexOfPoint - 2].ClusterId;

                    int rightCluster = 0;
                    if (points[indexOfPoint + 1].ClusterId != 0 || points[indexOfPoint + 1].ClusterId != -1)
                        rightCluster = points[indexOfPoint + 1].ClusterId;
                    int right2Cluster = 0;
                    if (points[indexOfPoint + 2].ClusterId != 0 || points[indexOfPoint + 2].ClusterId != -1)
                        right2Cluster = points[indexOfPoint + 2].ClusterId;

                    // Firstly check if the leftCluster is the same
                    if (leftCluster == p.ClusterId)
                    {
                        if (left2Cluster == p.ClusterId)
                        {
                            // Check if the righCluster is the different
                            if (rightCluster != p.ClusterId && right2Cluster != p.ClusterId && p.ClusterId != -1)
                            {
                                // This is the leave point
                                // Set which cluster the user just left
                                clusterBeingLeft = p;
                            }
                        }
                    }
                    else
                    {
                        // Check if the righCluster is the same
                        if (rightCluster == p.ClusterId && right2Cluster == p.ClusterId && p.ClusterId != -1)
                        {
                            // This is the enter point
                            // Canot enter a cluster without having left one
                            if (clusterBeingLeft != null)
                                EnteredCluster(p, user);
                        }
                    }
                }
            }
        }
    }

    private void EnteredCluster(GeoPoint p, User user)
    {
        // Create a historical journey for the day
        Journey j = new Journey(clusterBeingLeft.createdAt, p.createdAt, clusterBeingLeft.ClusterId, p.ClusterId);
        user.GetDay(p.createdAt.DayOfWeek).AddHistoricalJourney(j);
        // Reset clusterBeingLeft
        clusterBeingLeft = null;
    }

}
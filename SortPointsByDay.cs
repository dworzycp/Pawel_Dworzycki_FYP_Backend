/**
 * Sorts all of the GPS co-ordinates by day
 *
 * @author Pawel Dworzycki
 * @version 10/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;

class SortPointsByDay
{

    public Dictionary<String, List<Point>> daysWithoutClusters { get; private set; }

    // TODO rename as it sorts by day into clusters
    public SortPointsByDay(List<Point> points, Cluster cluster = null)
    {
        if (cluster != null)
            SplitPointsByDay(cluster, points);
        else
            SortPointsWithoutClusters(points);
    }

    private void SplitPointsByDay(Cluster cluster, List<Point> points)
    {
        // Sort the points
        foreach (Point p in points)
            SortPoint(cluster.days, p);
    }

    private void SortPoint(Dictionary<String, ClusterDay> daysDic, Point p)
    {
        // Check if the key already exists
        String k = p.createdAt.ToShortDateString();
        if (daysDic.ContainsKey(k))
        {
            // Key exists, Add value to the list of points for that key
            ClusterDay day = daysDic[k];
            day.points.Add(p);
        }
        else
        {
            // Create key
            daysDic.Add(k, new ClusterDay());
            // Now that the key exists, call the method again with the same parameters
            SortPoint(daysDic, p);
        }
    }

    private void SortPointsWithoutClusters(List<Point> points)
    {
        daysWithoutClusters = new Dictionary<string, List<Point>>();

        foreach (Point p in points)
        {
            // Check if key already exists
            if (daysWithoutClusters.ContainsKey(p.createdAt.ToShortDateString()))
            {
                daysWithoutClusters[p.createdAt.ToShortDateString()].Add(p);
            }
            // Create a new key
            else
            {
                daysWithoutClusters.Add(p.createdAt.ToShortDateString(), new List<Point>());
                // Add the point
                daysWithoutClusters[p.createdAt.ToShortDateString()].Add(p);
            }
        }

    }

}
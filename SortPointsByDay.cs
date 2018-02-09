/**
 * Sorts all of the GPS co-ordinates by day
 *
 * @author Pawel Dworzycki
 * @version 09/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;

class SortPointsByDay
{

    public SortPointsByDay(Cluster cluster, List<Point> points)
    {
        SplitPointsByDay(cluster, points);
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

}
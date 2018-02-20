/**
 * Sorts all of the GPS co-ordinates by day
 *
 * @author Pawel Dworzycki
 * @version 17/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;

class SortPointsByDay
{

    public Dictionary<String, List<Point>> daysWithoutClusters { get; private set; }

    // TODO rename as it sorts by day into clusters
    public SortPointsByDay(List<Point> points, Cluster cluster)
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
            // Set the day para. in the model
            ClusterDay c = new ClusterDay();
            c.day = DayAsNum(p);
            // Create key
            daysDic.Add(k, c);
            // Now that the key exists, call the method again with the same parameters
            SortPoint(daysDic, p);
        }
    }

    /**
     * Returns a numeric representation of the day as a number
     * 1 = Monday ... 7 = Sunday
     */
    private int DayAsNum(Point p)
    {
        switch (p.createdAt.DayOfWeek)
        {
            case DayOfWeek.Monday:
                return 1;
            case DayOfWeek.Tuesday:
                return 2;
            case DayOfWeek.Wednesday:
                return 3;
            case DayOfWeek.Thursday:
                return 4;
            case DayOfWeek.Friday:
                return 5;
            case DayOfWeek.Saturday:
                return 6;
            case DayOfWeek.Sunday:
                return 7;
            default:
                return 0;
        }
    }

}
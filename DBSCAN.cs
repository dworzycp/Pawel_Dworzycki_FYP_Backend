// http://www.c-sharpcorner.com/uploadfile/b942f9/implementing-the-dbscan-algorithm-using-C-Sharp/ 

using System;
using System.Collections.Generic;
using System.Linq;

class DBSCAN
{
    public DBSCAN(List<Point> points)
    {
        double eps = 0.0005;
        int minPts = 3;
        List<List<Point>> clusters = GetClusters(points, eps, minPts);
        Console.Clear();
        // print points to console
        // Console.WriteLine("The {0} points are :\n", points.Count);
        // foreach (Point p in points) Console.Write(" {0} ", p);
        // Console.WriteLine();

        // print clusters to console
        int total = 0;
        for (int i = 0; i < clusters.Count; i++)
        {
            int count = clusters[i].Count;
            total += count;
            string plural = (count != 1) ? "s" : "";
            Console.WriteLine("\nCluster {0} consists of the following {1} point{2} :\n", i + 1, count, plural);
            foreach (Point p in clusters[i]) Console.WriteLine(" {0} ", p);
            Console.WriteLine();
        }
        // print any points which are NOISE
        total = points.Count - total;
        if (total > 0)
        {
            string plural = (total != 1) ? "s" : "";
            string verb = (total != 1) ? "are" : "is";
            Console.WriteLine("\nThe following {0} point{1} {2} NOISE :\n", total, plural, verb);
            foreach (Point p in points)
            {
                if (p.ClusterId == Point.NOISE) Console.WriteLine(" {0} ", p);
            }
            Console.WriteLine();
        }
        else
        {
            Console.WriteLine("\nNo points are NOISE");
        }
        Console.ReadKey();
    }
    static List<List<Point>> GetClusters(List<Point> points, double eps, int minPts)
    {
        if (points == null) return null;
        List<List<Point>> clusters = new List<List<Point>>();
        eps *= eps; // square eps
        int clusterId = 1;
        for (int i = 0; i < points.Count; i++)
        {
            Point p = points[i];
            if (p.ClusterId == Point.UNCLASSIFIED)
            {
                if (ExpandCluster(points, p, clusterId, eps, minPts)) clusterId++;
            }
        }
        // sort out points into their clusters, if any
        int maxClusterId = points.OrderBy(p => p.ClusterId).Last().ClusterId;
        if (maxClusterId < 1) return clusters; // no clusters, so list is empty
        for (int i = 0; i < maxClusterId; i++) clusters.Add(new List<Point>());
        foreach (Point p in points)
        {
            if (p.ClusterId > 0) clusters[p.ClusterId - 1].Add(p);
        }
        return clusters;
    }
    static List<Point> GetRegion(List<Point> points, Point p, double eps)
    {
        List<Point> region = new List<Point>();
        for (int i = 0; i < points.Count; i++)
        {
            double distSquared = Point.DistanceSquared(p, points[i]);
            if (distSquared <= eps) region.Add(points[i]);
        }
        return region;
    }
    static bool ExpandCluster(List<Point> points, Point p, int clusterId, double eps, int minPts)
    {
        List<Point> seeds = GetRegion(points, p, eps);
        if (seeds.Count < minPts) // no core point
        {
            p.ClusterId = Point.NOISE;
            return false;
        }
        else // all points in seeds are density reachable from point 'p'
        {
            for (int i = 0; i < seeds.Count; i++) seeds[i].ClusterId = clusterId;
            seeds.Remove(p);
            while (seeds.Count > 0)
            {
                Point currentP = seeds[0];
                List<Point> result = GetRegion(points, currentP, eps);
                if (result.Count >= minPts)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        Point resultP = result[i];
                        if (resultP.ClusterId == Point.UNCLASSIFIED || resultP.ClusterId == Point.NOISE)
                        {
                            if (resultP.ClusterId == Point.UNCLASSIFIED) seeds.Add(resultP);
                            resultP.ClusterId = clusterId;
                        }
                    }
                }
                seeds.Remove(currentP);
            }
            return true;
        }
    }
}
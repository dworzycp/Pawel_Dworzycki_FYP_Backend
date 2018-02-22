/**
 * DBSCAN based on http://www.c-sharpcorner.com/uploadfile/b942f9/implementing-the-dbscan-algorithm-using-C-Sharp/ 
 *
 * @author Pawel Dworzycki
 * @version 11/02/2018
 */

using System;
using System.Collections.Generic;
using System.Linq;

class DBSCAN
{

    public List<List<GeoPoint>> clusters { get; private set; }

    public DBSCAN(List<GeoPoint> points)
    {
        double eps = 0.002;
        int minPts = 20;
        clusters = GetClusters(points, eps, minPts);
    }

    static List<List<GeoPoint>> GetClusters(List<GeoPoint> points, double eps, int minPts)
    {
        if (points == null) 
            return null;
            
        List<List<GeoPoint>> clusters = new List<List<GeoPoint>>();
        eps *= eps; // square eps
        int clusterId = 1;
        for (int i = 0; i < points.Count; i++)
        {
            GeoPoint p = points[i];
            if (p.ClusterId == GeoPoint.UNCLASSIFIED)
                if (ExpandCluster(points, p, clusterId, eps, minPts)) 
                    clusterId++;
        }
        // sort out points into their clusters, if any
        int maxClusterId = points.OrderBy(p => p.ClusterId).Last().ClusterId;
        if (maxClusterId < 1) return clusters; // no clusters, so list is empty
        for (int i = 0; i < maxClusterId; i++) clusters.Add(new List<GeoPoint>());
        foreach (GeoPoint p in points)
        {
            if (p.ClusterId > 0) clusters[p.ClusterId - 1].Add(p);
        }
        return clusters;
    }

    static List<GeoPoint> GetRegion(List<GeoPoint> points, GeoPoint p, double eps)
    {
        List<GeoPoint> region = new List<GeoPoint>();
        for (int i = 0; i < points.Count; i++)
        {
            double distSquared = GeoPoint.DistanceSquared(p, points[i]);
            if (distSquared <= eps) region.Add(points[i]);
        }
        return region;
    }

    static bool ExpandCluster(List<GeoPoint> points, GeoPoint p, int clusterId, double eps, int minPts)
    {
        List<GeoPoint> seeds = GetRegion(points, p, eps);
        if (seeds.Count < minPts) // no core point
        {
            p.ClusterId = GeoPoint.NOISE;
            return false;
        }
        else // all points in seeds are density reachable from point 'p'
        {
            for (int i = 0; i < seeds.Count; i++) seeds[i].ClusterId = clusterId;
            seeds.Remove(p);
            while (seeds.Count > 0)
            {
                GeoPoint currentP = seeds[0];
                List<GeoPoint> result = GetRegion(points, currentP, eps);
                if (result.Count >= minPts)
                {
                    for (int i = 0; i < result.Count; i++)
                    {
                        GeoPoint resultP = result[i];
                        if (resultP.ClusterId == GeoPoint.UNCLASSIFIED || resultP.ClusterId == GeoPoint.NOISE)
                        {
                            if (resultP.ClusterId == GeoPoint.UNCLASSIFIED) seeds.Add(resultP);
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
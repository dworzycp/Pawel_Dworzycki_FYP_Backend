/**
 * This class models a cluster of co-ordinates
 * @author Pawel Dworzycki
 *
 * @version 17/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;


class Cluster
{
    public GeoPoint centrePoint;
    public double radiusInMeters;
    // If the label isn't HOME or WORK get it from GPS data -- Google API?
    public string SemanticLabel = "";
    public string userId;
    public int clusterId;
    public string clusterDBId;
    public List<GeoPoint> points;

    public Cluster()
    {
        points = new List<GeoPoint>();
    }

    public void CalculateCentrePoint()
    {
        if (points.Count == 0)
            throw new Exception("There are no points assigned to cluster " + clusterId);

        try
        {
            double totalLat = 0;
            double totalLng = 0;
            int count = 0;

            foreach (GeoPoint p in points)
            {
                totalLat += p.latitude;
                totalLng += p.longitude;
                count++;
            }

            double centreLat = totalLat / count;
            double centreLng = totalLng / count;

            GeoPoint pt = new GeoPoint(centreLat, centreLng, userId);
            centrePoint = pt;
        }
        catch (System.Exception) { throw; }
    }

    public void CalculateRadius()
    {
        if (centrePoint == null)
            CalculateCentrePoint();

        double distance = 0;
        GeoPoint pointFurthest = new GeoPoint(0.0, 0.0, userId);

        foreach (GeoPoint p in points)
        {
            double d = p.DistanceBetweenPointsInMeters(centrePoint);
            if (d > distance)
            {
                distance = d;
                pointFurthest = p;
            }
        }

        radiusInMeters = distance;
    }

}
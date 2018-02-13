/**
 * This class models a cluster of co-ordinates
 * @author Pawel Dworzycki
 *
 * @version 13/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;


class Cluster
{
    public Dictionary<String, ClusterDay> days;
    public String name;
    public Point centrePoint;
    public double radiusInMeters;

    public Cluster()
    {
        days = new Dictionary<String, ClusterDay>();
    }

    public void CalculateCentrePoint()
    {
        double totalLat = 0;
        double totalLng = 0;
        int count = 0;

        foreach (ClusterDay cd in days.Values)
            foreach (Point p in cd.points)
            {
                totalLat += p.latitude;
                totalLng += p.longitude;
                count++;
            }

        double centreLat = totalLat / count;
        double centreLng = totalLng / count;

        Point pt = new Point(centreLat, centreLng);
        centrePoint = pt;
    }

    public void CalculateRadius()
    {
        if (centrePoint == null)
            CalculateCentrePoint();

        double distance = 0;
        Point pointFurthest = new Point(0.0, 0.0);

        foreach (ClusterDay cd in days.Values)
            foreach (Point p in cd.points)
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
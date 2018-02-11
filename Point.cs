/**
 * This class defines a GPS Coordinate
 * @author Pawel Dworzycki
 * @version 09/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;

// TO CHECK
// What if a point is classified as 'NOISE' -- can it never be used again??
// Can points become part of different clusters, i.e.: merge

class Point
{
    public double latitude { get; private set; }
    public double longitude { get; private set; }
    public DateTime createdAt { get; private set; }
    public int ClusterId;
    public const int UNCLASSIFIED = 0;
    public const int NOISE = -1;

    public Cluster cluster;

    public Point(double latitude, double longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
    }

    // TODO : there's a bug, it always returns 0??
    // https://stackoverflow.com/questions/26157199/how-to-calculate-distance-between-2-coordinates -- LOOK AT THIS
    public double DistanceBetweenPointsInMeters(Point p1, Point p2)
    {
        // Using Haversine forumla
        // Earth's radius in meters
        var R = 6371e3;
        // Find the difference between the two points
        var dLat = toRadians(p2.latitude - p1.latitude);
        var dLon = toRadians(p2.longitude - p1.longitude);
        // Convert latitudes to radians for future use
        p1.latitude = toRadians(p1.latitude);
        p2.latitude = toRadians(p2.latitude);

        // Apply Haversine forumla
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + Math.Sin(dLon / 2) *
                Math.Sin(dLon / 2) * Math.Cos(p1.latitude) * Math.Cos(p2.latitude);
        var c = 2 * Math.Asin(Math.Sqrt(a));
        return R * 2 * Math.Asin(Math.Sqrt(a));
    }

    public static double DistanceSquared(Point p1, Point p2)
    {
        double diffX = p2.latitude - p1.latitude;
        double diffY = p2.longitude - p1.longitude;
        return diffX * diffX + diffY * diffY;
    }

    private static double toRadians(double angle)
    {
        return Math.PI * angle / 180.0;
    }

    public override string ToString()
    {
        return latitude.ToString() + ", " + longitude.ToString();
        //return "(" + latitude.ToString() + ", " + longitude.ToString() + ") Created at: " + createdAt.ToString();
    }

    public void SetTime(DateTime newTime)
    {
        createdAt = newTime;
    }

}
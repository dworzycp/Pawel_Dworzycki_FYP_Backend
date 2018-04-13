/**
 * This class defines a GPS Coordinate
 * @author Pawel Dworzycki
 * @version 13/04/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Device.Location;

class GeoPoint
{
    public double latitude { get; private set; }
    public double longitude { get; private set; }
    public DateTime createdAt { get; private set; }
    public string userId { get; private set; }

    public int ClusterId = 0;
    public const int UNCLASSIFIED = 0;
    public const int NOISE = -1;

    public Cluster cluster;

    public GeoPoint(double latitude, double longitude, string userId)
    {
        this.latitude = latitude;
        this.longitude = longitude;
        this.userId = userId;
    }

    /// <summary>
    /// Return the distance between this point and another using Haversine formula
    /// </summary>
    /// <param name="otherPoint">Point to calculate distance to</param>
    /// <returns></returns>
    public double DistanceBetweenPointsInMeters(GeoPoint otherPoint)
    {
        GeoCoordinate gp1 = new GeoCoordinate(latitude, longitude);
        GeoCoordinate gp2 = new GeoCoordinate(otherPoint.latitude, otherPoint.longitude);
        return gp1.GetDistanceTo(gp2);
    }

    // TODO replace with ^^??
    public static double DistanceSquared(GeoPoint p1, GeoPoint p2)
    {
        double diffX = p2.latitude - p1.latitude;
        double diffY = p2.longitude - p1.longitude;
        return diffX * diffX + diffY * diffY;
    }

    /// <summary>
    /// Convert an angle into radians
    /// </summary>
    /// <param name="angle">Angle in degrees</param>
    /// <returns></returns>
    private static double toRadians(double angle)
    {
        return Math.PI * angle / 180.0;
    }

    public override string ToString()
    {
        return latitude.ToString() + ", " + longitude.ToString();
    }

    public void SetTime(DateTime newTime)
    {
        createdAt = newTime;
    }

    public bool DoesPointBelongToCluster(Cluster c)
    {
        // Check if the distance between this point and c's centre is less than c's radius
        if (DistanceBetweenPointsInMeters(c.centrePoint) < c.radiusInMeters)
            return true;
        else
            return false;
    }

}
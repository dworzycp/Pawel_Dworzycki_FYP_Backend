/**
 * This generates fake data.
 * Generates random points with a given radius to simulate real user
 *
 * @author Pawel Dworzycki
 * @version 11/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;

class FakeDataGen
{

    // User's home and work are hard coded
    // West Brom
    private const double homeLat = 52.530851;
    private const double homeLng = -1.996753;
    // Aston University
    private const double workLat = 52.486695;
    private const double workLng = -1.890520;
    // Radius
    private const int radiusInMeters = 50;
    // Fake userId
    private const string userId = "11111";

    public List<GeoPoint> points { get; private set; }

    public FakeDataGen(int numOfDays)
    {
        points = new List<GeoPoint>();
        DateTime time = DateTime.Today;
        // For the next 7 days, generate data every 30 mins
        for (int i = 0; i < numOfDays; i++)
        {
            if (time.DayOfWeek == DayOfWeek.Monday ||
                time.DayOfWeek == DayOfWeek.Tuesday ||
                time.DayOfWeek == DayOfWeek.Wednesday ||
                time.DayOfWeek == DayOfWeek.Thursday ||
                time.DayOfWeek == DayOfWeek.Friday)
            {
                // Weekday
                // 00:00 - 07:00 - home
                // 07:30 - 09:00 - travelling / noise
                // 09:30 - 16:30 - work
                // 17:00 - 19:00 - travelling / noise
                // 19:30 - 23:30 - home

                // Between 00:00 and 7:00 (15 points at home)
                for (int k = 0; k < 15; k++)
                {
                    GeoPoint p = GenerateRandomPoint(homeLat, homeLng, radiusInMeters, time);
                    AddPoint(p, points, time, i);
                    time = time.AddMinutes(30);
                }
                // Between 7:30 and 9:00 (4 points travelling/NOISE)
                // For this the line between two points and get the co-ordinates on that line
                // n - 2 ; as the splits do not include end points
                foreach (GeoPoint p in SplitLine(4 - 2))
                {
                    p.SetTime(time);
                    AddPoint(p, points, time, i);
                    time = time.AddMinutes(30);
                }
                // Between 9:30 and 16:30 (15 points at work)
                for (int k = 0; k < 15; k++)
                {
                    GeoPoint p = GenerateRandomPoint(workLat, workLng, radiusInMeters, time);
                    AddPoint(p, points, time, i);
                    time = time.AddMinutes(30);
                }
                // Between 17:00 and 19:00 (5 points travelling/NOISE)
                foreach (GeoPoint p in SplitLine(5 - 2, true))
                {
                    p.SetTime(time);
                    AddPoint(p, points, time, i);
                    time = time.AddMinutes(30);
                }
                // Between 19:30 and 23:30 (9 points at home)
                for (int k = 0; k < 9; k++)
                {
                    GeoPoint p = GenerateRandomPoint(homeLat, homeLng, radiusInMeters, time);
                    AddPoint(p, points, time, i);
                    time = time.AddMinutes(30);
                }
            }
            else
            {
                // It's the weekend
                // Assuming the user does NOT work on the weekend
                // Their activity will be described as:
                // 00:00 - 12:30 - 90% chance of being within 50m of home
                //                 10% chance of being within 250m of home
                // 13:00 - 17:00 - 40% chance of being within 500m of home
                //                 40% chance of being within 1.5km of home
                //                 20% chance of being within 2.5km of home
                // 17:30 - 20:00 - 80% chance of being within 50m of home
                //                 20% chance of being within 250m of home
                // 20:30 - 23:30 - 90% chance of being within 50m of home
                //                 10% chance of being within 250m of home

                Random rng = new Random();
                int radius;

                // 00:00 - 12:30 (26 points)
                for (int k = 0; k < 26; k++)
                {
                    int r = rng.Next(100);
                    if (r < 90)
                        radius = 50;
                    else
                        radius = 250;

                    GeoPoint p = GenerateRandomPoint(homeLat, homeLng, radius, time);
                    AddPoint(p, points, time, i);
                    time = time.AddMinutes(30);
                }
                // 13:00 - 17:00 (9 points)
                for (int k = 0; k < 9; k++)
                {
                    int r = rng.Next(100);
                    if (r < 40)
                        radius = 500;
                    else if (r > 40 && r < 80)
                        radius = 1500;
                    else
                        radius = 2500;

                    GeoPoint p = GenerateRandomPoint(homeLat, homeLng, radius, time);
                    AddPoint(p, points, time, i);
                    time = time.AddMinutes(30);
                }
                // 17:30 - 20:00 (6 points)
                for (int k = 0; k < 6; k++)
                {
                    int r = rng.Next(100);
                    if (r < 80)
                        radius = 50;
                    else
                        radius = 250;

                    GeoPoint p = GenerateRandomPoint(homeLat, homeLng, radius, time);
                    AddPoint(p, points, time, i);
                    time = time.AddMinutes(30);
                }
                // 20:30 - 23:30 (7 points)
                for (int k = 0; k < 7; k++)
                {
                    int r = rng.Next(100);
                    if (r < 90)
                        radius = 50;
                    else
                        radius = 250;

                    GeoPoint p = GenerateRandomPoint(homeLat, homeLng, radius, time);
                    AddPoint(p, points, time, i);
                    time = time.AddMinutes(30);
                }
            }
        }
    }

    private void AddPoint(GeoPoint point, List<GeoPoint> collectionOfPoints, DateTime time, int i)
    {
        collectionOfPoints.Add(point);
        //Console.WriteLine("Added a point for " + point.createdAt.ToShortDateString() + " at " + time.ToShortTimeString() + " (run: " + i + ")");
    }

    /**
     * Create a random point within a given radius
     * based on: https://stackoverflow.com/questions/36905396/randomly-generating-a-latlng-within-a-radius-yields-a-point-out-of-bounds
     */
    public GeoPoint GenerateRandomPoint(double lat, double lng, int radiusInMeters, DateTime fakeTimeOfPoint)
    {
        double x0 = lng;
        double y0 = lat;

        Random random = new Random();

        // Convert radius from meters to degrees.
        double radiusInDegrees = radiusInMeters / 111320f;

        // Get a random distance and a random angle.
        double u = random.NextDouble();
        double v = random.NextDouble();
        double w = radiusInDegrees * Math.Sqrt(u);
        double t = 2 * Math.PI * v;
        // Get the x and y delta values.
        double x = w * Math.Cos(t);
        double y = w * Math.Sin(t);

        // Compensate the x value.
        double new_x = x / Math.Cos(ToRadians(y0));

        double foundLatitude;
        double foundLongitude;

        foundLatitude = y0 + y;
        foundLongitude = x0 + new_x;

        GeoPoint point = new GeoPoint(foundLatitude, foundLongitude, userId);
        point.SetTime(fakeTimeOfPoint);
        return point;
    }

    private double ToRadians(double angle)
    {
        return (Math.PI / 180) * angle;
    }

    // Based on https://stackoverflow.com/questions/21249739/how-to-calculate-the-points-between-two-given-points-and-given-distance
    public static IList<GeoPoint> SplitLine(int count, bool reverse = false)
    {
        count = count + 1;

        double startLat = homeLat;
        double startLng = homeLng;
        double endLat = workLat;
        double endLng = workLng;

        if (reverse == true)
        {
            startLat = workLat;
            startLng = workLng;
            endLat = homeLat;
            endLng = homeLng;
        }

        Double d = Math.Sqrt((startLat - endLat) * (startLat - endLat) + (startLng - endLng) * (startLng - endLng)) / count;
        Double fi = Math.Atan2(endLng - startLng, endLat - startLat);

        List<GeoPoint> points = new List<GeoPoint>(count + 1);

        for (int i = 0; i <= count; ++i)
            points.Add(new GeoPoint(startLat + i * d * Math.Cos(fi), startLng + i * d * Math.Sin(fi), userId));

        return points;
    }

}
using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            // List<Point> points = new List<Point>();
            // // Retrieve GPS coordinates from DB for userId 1
            // Database db = new Database();
            // points = db.GetUnclassifiedCoordinates("1", "2017-12-12");

            // //Console.WriteLine(points[1].DistanceBetweenPointsInMeters(points[1], points[2]));

            // //foreach (Point p in points) Console.WriteLine(p.ToString());

            // DBSCAN dbscan = new DBSCAN(points);

            // FakeData Gen
            FakeDataGen fdg = new FakeDataGen();
            foreach (Point p in fdg.points)
            {
                Console.WriteLine(p.ToString());
            }
        }
    }
}

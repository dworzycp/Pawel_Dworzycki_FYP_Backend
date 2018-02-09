using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Point> points = new List<Point>();
            List<Cluster> clusters = new List<Cluster>();

            #region Actual DB connection
            // // Retrieve GPS coordinates from DB for userId 1
            // Database db = new Database();
            // points = db.GetUnclassifiedCoordinates("1", "2017-12-12");

            // //Console.WriteLine(points[1].DistanceBetweenPointsInMeters(points[1], points[2]));

            // //foreach (Point p in points) Console.WriteLine(p.ToString());
            #endregion

            // FakeData Gen
            FakeDataGen fdg = new FakeDataGen();
            foreach (Point p in fdg.points)
                points.Add(p);

            // Run DBSCAN to find clusters
            DBSCAN dbscan = new DBSCAN(points);
            foreach (List<Point> dbscanCluster in dbscan.clusters)
            {

                // TODO check if a cluster corresponds to an existing cluster

                // Create a new Cluster
                Cluster c = new Cluster();
                // Sort points by day
                SortPointsByDay sort = new SortPointsByDay(c, dbscanCluster);
                // Add to list of clusters
                clusters.Add(c);
            }

            // Work out the average time a user leaves the cluster
            // TimePrediction times = new TimePrediction(clusters);
        }
    }
}

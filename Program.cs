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
                c.name = "Cluster " + clusters.Count + 1;
                // Sort points by day
                SortPointsByDay sort = new SortPointsByDay(dbscanCluster, c);
                // Add to list of clusters
                clusters.Add(c);
            }

            // For every point in a cluster -- reference cluster in the point
            foreach (Cluster c in clusters)
                foreach (ClusterDay cd in c.days.Values)
                    foreach (Point p in cd.points)
                        p.cluster = c;

            TimeLeaveAndEnterClusterDays times = new TimeLeaveAndEnterClusterDays(points);
            // TODO histogram

            // Calculate cluster's mid point and radius
            foreach (Cluster c in clusters)
                c.CalculateRadius();

            // Find HOME and WORK clusters
            IdentifyHomeAndWorkClusters idCLusters = new IdentifyHomeAndWorkClusters(clusters);

            // PRINT
            foreach (Cluster c in clusters)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("#######################################");
                if (c.SemanticLabel != "" || c.SemanticLabel != null)
                    Console.WriteLine("### " + c.SemanticLabel + " ###");
                else
                    Console.WriteLine("### " + " NEW CLUSTER " + " ###");
                Console.WriteLine("#######################################");
                Console.WriteLine("Centre " + c.centrePoint.ToString() + " radius " + c.radiusInMeters);
                foreach (KeyValuePair<String, ClusterDay> cd_pair in c.days)
                {
                    Console.WriteLine();
                    Console.WriteLine("#######################################");
                    Console.WriteLine("### " + cd_pair.Key + " ###");
                    Console.WriteLine("#######################################");
                    Console.WriteLine("Enter time: " + cd_pair.Value.avgEnterTime);
                    Console.WriteLine("Leave time: " + cd_pair.Value.avgLeaveTime);
                }
            }

        }

    }
}

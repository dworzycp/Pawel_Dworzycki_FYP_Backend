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

            Dictionary<String, List<Point>> pointsByDay = new Dictionary<string, List<Point>>();

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

            // Also sort the points by day without clutsers
            // TODO redundant?
            SortPointsByDay sort2 = new SortPointsByDay(points);
            pointsByDay = sort2.daysWithoutClusters;

            // For every point in a cluster -- reference cluster in the point
            foreach (Cluster c in clusters)
            {
                foreach (ClusterDay cd in c.days.Values)
                {
                    foreach (Point p in cd.points)
                    {
                        p.cluster = c;
                    }
                }
            }

            // Work out the time a user leaves and enters clusters
            // TODO which points to pass? all? clusters all? days only?
            // ^^ doesn't matter but what's best?
            // TimeLeaveAndEnterClusterDays times = new TimeLeaveAndEnterClusterDays(points);


            foreach (List<Point> points_day in pointsByDay.Values)
            {
                TimeLeaveAndEnterClusterDays times = new TimeLeaveAndEnterClusterDays(points_day);
            }

            //TimeLeaveAndEnterClusterDays times = new TimeLeaveAndEnterClusterDays(fakeDataForTimeTest());

            // TODO now need to average it out between past weeks

            // PRINT
            foreach (Cluster c in clusters)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("#######################################");
                Console.WriteLine("### " + " NEW CLUSTER " + " ###");
                Console.WriteLine("#######################################");
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

            // foreach (List<Point> pts in pointsByDay.Values)
            // {
            //     Console.WriteLine("#######################################");
            //     Console.WriteLine("#### " + " NEW DAY " + " ###");
            //     Console.WriteLine("#######################################");
            //     foreach (Point p in pts)
            //     {
            //         if (p.cluster != null)
            //             Console.WriteLine(p.latitude + ", " + p.longitude + "  - " + p.cluster.name + " - " + p.createdAt);
            //         else
            //             Console.WriteLine(p.latitude + ", " + p.longitude +  "  - NOISE" + " - " + p.createdAt);
            //     }
            // }

        }

    }
}

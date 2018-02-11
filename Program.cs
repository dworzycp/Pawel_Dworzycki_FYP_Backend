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
            // foreach (Cluster c in clusters)
            // {
            //     Console.WriteLine();
            //     Console.WriteLine();
            //     Console.WriteLine("#######################################");
            //     Console.WriteLine("### " + " NEW CLUSTER " + " ###");
            //     Console.WriteLine("#######################################");
            //     foreach (KeyValuePair<String, ClusterDay> cd_pair in c.days)
            //     {
            //         Console.WriteLine();
            //         Console.WriteLine("#######################################");
            //         Console.WriteLine("### " + cd_pair.Key + " ###");
            //         Console.WriteLine("#######################################");
            //         Console.WriteLine("Enter time: " + cd_pair.Value.avgEnterTime);
            //         Console.WriteLine("Leave time: " + cd_pair.Value.avgLeaveTime);
            //     }
            // }

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

        // TODO probably needs fixing after changes to TimeLeave... needing 2 clusters
        private static List<Point> fakeDataForTimeTest()
        {
            List<Point> points = new List<Point>();

            // Set up clusters
            Cluster c_home = new Cluster();
            c_home.days = new Dictionary<string, ClusterDay>();
            Cluster c_work = new Cluster();
            c_work.days = new Dictionary<string, ClusterDay>();

            Point p = null;
            // Leaving home
            c_home.days.Add(DateTime.Today.ToShortDateString(), new ClusterDay());

            p = new Point(1.0, 1.0);
            p.cluster = c_home;
            p.SetTime(DateTime.Today.AddHours(6));
            points.Add(p);
            c_home.days[DateTime.Today.ToShortDateString()].points.Add(p);

            p = new Point(1.0, 1.0);
            p.cluster = c_home;
            p.SetTime(DateTime.Today.AddHours(7));
            points.Add(p);
            c_home.days[DateTime.Today.ToShortDateString()].points.Add(p);

            // Travel
            p = new Point(1.5, 1.5);
            p.cluster = null;
            p.SetTime(DateTime.Today.AddHours(8));
            points.Add(p);

            // Enter work
            c_work.days.Add(DateTime.Today.ToShortDateString(), new ClusterDay());

            p = new Point(2.0, 2.0);
            p.cluster = c_work;
            p.SetTime(DateTime.Today.AddHours(9));
            points.Add(p);
            c_work.days[DateTime.Today.ToShortDateString()].points.Add(p);

            p = new Point(2.0, 2.0);
            p.cluster = c_work;
            p.SetTime(DateTime.Today.AddHours(10));
            points.Add(p);
            c_work.days[DateTime.Today.ToShortDateString()].points.Add(p);

            // Leave work
            p = new Point(2.0, 2.0);
            p.cluster = c_work;
            p.SetTime(DateTime.Today.AddHours(11));
            points.Add(p);
            c_work.days[DateTime.Today.ToShortDateString()].points.Add(p);

            // Travel
            p = new Point(1.5, 1.5);
            p.cluster = null;
            p.SetTime(DateTime.Today.AddHours(12));
            points.Add(p);

            // Enter home
            p = new Point(1.0, 1.0);
            p.cluster = c_home;
            p.SetTime(DateTime.Today.AddHours(13));
            points.Add(p);
            c_home.days[DateTime.Today.ToShortDateString()].points.Add(p);

            p = new Point(1.0, 1.0);
            p.cluster = c_home;
            p.SetTime(DateTime.Today.AddHours(14));
            points.Add(p);
            c_home.days[DateTime.Today.ToShortDateString()].points.Add(p);

            return points;
        }

    }
}

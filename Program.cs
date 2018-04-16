using System;
using System.Collections.Generic;
using System.Linq;

namespace Backend
{
    class Program
    {

        static void Main(string[] args)
        {
            // Initialise variables
            Dictionary<string, User> userIdToUserMap = new Dictionary<string, User>();
            Dictionary<int, Cluster> idToCluterMap = new Dictionary<int, Cluster>();
            List<GeoPoint> dbPoints = new List<GeoPoint>();
            List<Cluster> dbClusters = new List<Cluster>();
            int highestClusterIndex = 0;

            // Get points
            // FAKE
            //FakeDataGen fdg = new FakeDataGen(21);
            //dbPoints = fdg.points;

            // REAL
            Database db = new Database();
            // TODO do I want to get classified points too?
            dbPoints = db.GetUnclassifiedCoordinates();
            dbClusters = db.GetClusters();

            // Set highest cluster index after reading clusters
            foreach (Cluster c in dbClusters)
            {
                if (c.clusterId > highestClusterIndex)
                    highestClusterIndex = c.clusterId;

                // Add all clusters to idToCluterMap
                idToCluterMap.Add(c.clusterId, c);
            }

            // Assign all of the points to users
            // Also assigns existing clusters
            // IMPORTANT: this also creates users
            AssignPoints pp = new AssignPoints(dbPoints, userIdToUserMap, dbClusters);

            // Find clusters for each user
            foreach (User u in userIdToUserMap.Values)
            {
                // Check if a point belongs to an existing cluster first
                foreach (GeoPoint p in u.points.ToList())
                {
                    foreach (Cluster c in u.idToClusterMap.Values)
                    {
                        if (p.DoesPointBelongToCluster(c))
                        {
                            // Set p's cluster to c
                            p.cluster = c;
                            p.ClusterId = c.clusterId;
                            // Add point to cluser
                            c.points.Add(p);
                            // Remove point from list, so it doesn't go to DBSCAN
                            u.unassignedPoints.Remove(p);
                        }
                    }
                }

                // DBSCAN remaining points
                if (u.unassignedPoints.Count > 0)
                {
                    DBSCAN dbscan = new DBSCAN(u.unassignedPoints);

                    // Save new Clusters to DB and user's local list
                    foreach (List<GeoPoint> dbscanCluster in dbscan.clusters)
                    {
                        // Create a new Cluster
                        Cluster c = new Cluster();
                        c.userId = u.userId;
                        // Set c's id
                        c.clusterId = highestClusterIndex + 1;
                        highestClusterIndex++;
                        // Assign cluster points to cluster
                        c.points = dbscanCluster;
                        // Add cluster to user's list
                        u.idToClusterMap.Add(c.clusterId, c);
                        // Calculate cluster's mid point and radius
                        c.CalculateRadius();
                        // Set semtainc label
                        c.SemanticLabel = "Location";
                        // Save cluster in DB
                        db.SaveCluster(c);
                        // TODO in GPS_Coords assign clusterId to point
                        // TODO handle noise
                        // Add cluster to idToCluterMap
                        idToCluterMap.Add(c.clusterId, c);
                    }

                    // Sort ALL points (dbscan and existing) within clusters by day
                    SortPointsByDay sort = new SortPointsByDay(u.points, u);

                }

                foreach (Day d in u.days)
                {
                    // Now that all of the points have been added with cluster references
                    // Sort them by creation time
                    d.historialGPSData.Sort((x, y) => DateTime.Compare(x.createdAt, y.createdAt));

                    // Find historical journeys
                    HistoricalJourneys hj = new HistoricalJourneys(d.historialGPSData, u);
                }

                // Calculate cluster's mid point and radius
                // foreach (Cluster c in u.idToClusterMap.Values)
                //     c.CalculateRadius();

                // Identify HOME and WORK clusters
                IdentifyHomeAndWorkClusters idCLusters = new IdentifyHomeAndWorkClusters(u.idToClusterMap, db);

                AnalyseHistoricalJourneys a = new AnalyseHistoricalJourneys(u.days);
                // Console.WriteLine(a.ToString());

                PredictJourneys pj = new PredictJourneys(a.predictions, db, idToCluterMap);

                foreach (Day d in u.days)
                {
                    foreach (Journey j in d.journeys)
                    {
                        Console.WriteLine(j.ToString());
                    }
                }

            }

            // Print
            if (userIdToUserMap.Count == 0)
            {
                Console.WriteLine("No data has been processed. (0 users)");
            }

            // foreach (User u in userIdToUserMap.Values)
            // {
            //     Console.WriteLine("User ID - " + u.userId);
            //     Console.WriteLine("Clusters - " + u.idToClusterMap.Values.Count);
            //     foreach (Cluster c in u.idToClusterMap.Values)
            //     {
            //         Console.WriteLine("Cluster " + c.SemanticLabel + " centre at - " + c.centrePoint + " with r - " + c.radiusInMeters);
            //     }
            //     Console.WriteLine();

            //     foreach (Day d in u.days)
            //     {
            //         Console.WriteLine(d.dayOfWeek.ToString());
            //         Console.WriteLine("Historical points - " + d.historialGPSData.Count);
            //         Console.WriteLine("Historical journeys - " + d.historialJourneys.Count);
            //         foreach (Journey j in d.historialJourneys)
            //         {
            //             Console.WriteLine(j.ToString());
            //         }
            //         // foreach (GeoPoint p in d.historialGPSData)
            //         // {
            //         //     //if (p.ClusterId != -1 && p.ClusterId != 0)
            //         //         Console.WriteLine(p.ToString() + " - " + p.createdAt.ToShortTimeString() + " - " + p.ClusterId);
            //         // }
            //         Console.WriteLine();
            //     }
            // }

        }

    }
}

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

            // Get points
            FakeDataGen fdg = new FakeDataGen(14);

            // Assign all of the points to users
            AssignPoints pp = new AssignPoints(fdg.points, userIdToUserMap);

            // Find clusters for each user
            foreach (User u in userIdToUserMap.Values)
            {
                // TODO check if a point belongs to an existing cluster first

                DBSCAN dbscan = new DBSCAN(u.points);
                foreach (List<GeoPoint> dbscanCluster in dbscan.clusters)
                {
                    // TODO check if a cluster corresponds to an existing cluster

                    // Create a new Cluster
                    Cluster c = new Cluster();
                    c.userId = u.userId;
                    // TODO is this correct?
                    c.clusterId = dbscanCluster[0].ClusterId.ToString();
                    // Sort points by day
                    SortPointsByDay sort = new SortPointsByDay(dbscanCluster, u);
                    // Assign cluster points to cluster
                    c.points = dbscanCluster;
                    // Add cluster to user's list
                    u.idToClusterMap.Add(c.clusterId, c);
                }

                // Find historical journeys
                foreach (Day d in u.days)
                {
                    HistoricalJourneys hj = new HistoricalJourneys(d.historialGPSData, u);
                }

                // Calculate cluster's mid point and radius
                foreach (Cluster c in u.idToClusterMap.Values)
                    c.CalculateRadius();
            }

            // Print
            foreach (User u in userIdToUserMap.Values)
            {
                Console.WriteLine("User ID - " + u.userId);
                Console.WriteLine("Clusters - " + u.idToClusterMap.Values.Count);
                foreach (Cluster c in u.idToClusterMap.Values)
                {
                    Console.WriteLine("Cluster " + c.clusterId + " centre at - " + c.centrePoint + " with r - " + c.radiusInMeters);
                }
                Console.WriteLine();

                foreach (Day d in u.days)
                {
                    Console.WriteLine(d.dayOfWeek.ToString());
                    Console.WriteLine("Historical points - " + d.historialGPSData.Count);
                    Console.WriteLine("Historical journeys - " + d.historialJourneys.Count);
                    foreach (Journey j in d.historialJourneys)
                    {
                        Console.WriteLine(j.ToString());
                    }
                    Console.WriteLine();
                }
            }

        }

    }
}

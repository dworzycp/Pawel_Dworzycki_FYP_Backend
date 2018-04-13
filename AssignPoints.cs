/**
 * Assigns points and clusters to its user
 *
 * @author Pawel Dworzycki
 * @version 13/04/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;

class AssignPoints
{

    public AssignPoints(List<GeoPoint> points, Dictionary<string, User> userIdToUserMap, List<Cluster> clusters)
    {
        // Assign points to users
        foreach (GeoPoint p in points)
            SortPointsByUserByDay(p, userIdToUserMap);

        // Assign clusters
        AssignClustersFromDB(clusters, userIdToUserMap);
    }

    private void SortPointsByUserByDay(GeoPoint p, Dictionary<string, User> userIdToUserMap)
    {
        // Sort points for each user
        // Check if user already exists
        if (userIdToUserMap.ContainsKey(p.userId))
        {
            // Add the point to the user's list of points
            User user = userIdToUserMap[p.userId];
            user.points.Add(p);
            user.unassignedPoints.Add(p);
        }
        else
        {
            // Create user
            userIdToUserMap.Add(p.userId, new User(p.userId));
            // Recall the method with the same point
            SortPointsByUserByDay(p, userIdToUserMap);
        }
    }

    private void AssignClustersFromDB(List<Cluster> clusters, Dictionary<string, User> userIdToUserMap)
    {
        foreach (Cluster c in clusters)
        {
            userIdToUserMap[c.userId].idToClusterMap.Add(c.clusterId, c);
        }
    }

}
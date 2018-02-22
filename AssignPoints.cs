/**
 * Assigns a point to its user
 *
 * @author Pawel Dworzycki
 * @version 22/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;

class AssignPoints
{

    public AssignPoints(List<GeoPoint> points, Dictionary<string, User> userIdToUserMap)
    {
        // Assign points to users
        foreach (GeoPoint p in points)
            SortPointsByUserByDay(p, userIdToUserMap);
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
        }
        else
        {
            // Create user
            userIdToUserMap.Add(p.userId, new User(p.userId));
            // Recall the method with the same point
            SortPointsByUserByDay(p, userIdToUserMap);
        }
    }

}
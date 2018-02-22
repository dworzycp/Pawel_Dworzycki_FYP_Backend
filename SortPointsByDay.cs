/**
 * Sorts all of the GPS co-ordinates by day
 *
 * @author Pawel Dworzycki
 * @version 21/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;

class SortPointsByDay
{

    public SortPointsByDay(List<GeoPoint> points, User user)
    {
        SplitPointsByDay(user, points);
    }

    private void SplitPointsByDay(User user, List<GeoPoint> points)
    {
        foreach (GeoPoint p in points)
            SortPoint(user, p);
    }

    private void SortPoint(User user, GeoPoint p)
    {
        // Get the day of the week this point belongs to
        Day d = user.GetDay(p.createdAt.DayOfWeek);
        // Add the point to user's day
        d.AddGPSpoint(p);
    }

}
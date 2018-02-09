/**
 * This class models a day for a cluster of co-ordinates
 * @author Pawel Dworzycki
 *
 * @version 09/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;


class ClusterDay
{
    public List<Point> points;
    public DateTime avgEnterTime;
    public DateTime avgLeaveTime;

    public ClusterDay()
    {
        points = new List<Point>();
    }
}
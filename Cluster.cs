/**
 * This class models a cluster of co-ordinates
 * @author Pawel Dworzycki
 *
 * @version 09/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;


class Cluster
{
    public Dictionary<String, ClusterDay> days;

    public Cluster()
    {
        days = new Dictionary<String, ClusterDay>();
    }
}
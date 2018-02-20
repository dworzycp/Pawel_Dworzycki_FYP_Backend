/**
 * Identifies a cluster as Home and Work based on the time spent in them.
 * The cluster with the most time spent in it will be marked as HOME
 * The cluster with the second most time as WORK
 *
 * @author Pawel Dworzycki
 * @version 17/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;


class IdentifyHomeAndWorkClusters
{

    Dictionary<Cluster, double> clusterAndStayTime;

    public IdentifyHomeAndWorkClusters(List<Cluster> clusters)
    {
        clusterAndStayTime = new Dictionary<Cluster, double>();

        // Work out how long a user spends in each cluster
        foreach (Cluster c in clusters)
        {
            double timeSpentInCluster = 0.0;
            // Get the first and last co-ords in the cluster for each day
            foreach (ClusterDay cd in c.days.Values)
            {
                Point enterPoint = cd.points[0];
                Point lastPoint = cd.points[cd.points.Count - 1];
                timeSpentInCluster = (lastPoint.createdAt - enterPoint.createdAt).TotalMinutes;
            }
            clusterAndStayTime.Add(c, timeSpentInCluster);
        }

        // Find HOME cluster
        Cluster home = FindClusterWithMostTime();
        home.SemanticLabel = "HOME";
        // Remove HOME cluster from list
        clusterAndStayTime.Remove(home);
        // Rerun to find WORK
        Cluster work = FindClusterWithMostTime();
        work.SemanticLabel = "WORK";

    }

    private Cluster FindClusterWithMostTime()
    {
        Cluster currentLongestCluster = new Cluster();
        double currentLongestTime = 0.0;

        foreach (KeyValuePair<Cluster, double> c in clusterAndStayTime)
        {
            if (c.Value > currentLongestTime)
            {
                currentLongestTime = c.Value;
                currentLongestCluster = c.Key;
            }
        }

        return currentLongestCluster;
    }

}
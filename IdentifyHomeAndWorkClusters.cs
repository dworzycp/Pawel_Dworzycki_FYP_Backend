/**
 * Identifies a cluster as Home and Work based on the time spent in them.
 * The cluster with the most time spent in it will be marked as HOME
 * The cluster with the second most time as WORK
 *
 * @author Pawel Dworzycki
 * @version 22/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;


class IdentifyHomeAndWorkClusters
{

    Dictionary<Cluster, double> clusterAndStayTime;

    public IdentifyHomeAndWorkClusters(Dictionary<string, Cluster> clusters, Database db)
    {
        clusterAndStayTime = new Dictionary<Cluster, double>();

        // Work out how long a user spends in each cluster
        foreach (Cluster c in clusters.Values)
        {
            double timeSpentInCluster = 0.0;
            // Get the first and last co-ords in the cluster for each day
            GeoPoint enterPoint = c.points[0];
            GeoPoint lastPoint = c.points[c.points.Count - 1];
            timeSpentInCluster = (lastPoint.createdAt - enterPoint.createdAt).TotalMinutes;
            clusterAndStayTime.Add(c, timeSpentInCluster);
        }

        // Find HOME cluster
        Cluster home = FindClusterWithMostTime();
        home.SemanticLabel = "HOME";
        // Save in DB
        db.UpdateClustersLabel(home.clusterId, "HOME");
        // Remove HOME cluster from list
        clusterAndStayTime.Remove(home);
        // Rerun to find WORK
        Cluster work = FindClusterWithMostTime();
        work.SemanticLabel = "WORK";
        db.UpdateClustersLabel(work.clusterId, "WORK");
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
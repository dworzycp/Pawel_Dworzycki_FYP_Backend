/**
 * Model for historical data
 *
 * @author Pawel Dworzycki
 * @version 26/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class HistoricalModel
{

    public Dictionary<int, PredictionHistogram> clusterHistograms { get; private set; }

    public HistoricalModel()
    {
        clusterHistograms = new Dictionary<int, PredictionHistogram>();
    }

    public void AddEnterTime(int destinationClusterId, DateTime time, int originClusterId)
    {
        // Check if the cluster exists
        if (clusterHistograms.ContainsKey(destinationClusterId))
        {
            clusterHistograms[destinationClusterId].AddEnterTime(time);
            // Add destination cluster for origin cluster
            clusterHistograms[originClusterId].AddDestinationCluster(destinationClusterId);
        }
        else
        {
            // Create the key
            clusterHistograms.Add(destinationClusterId, new PredictionHistogram());
            // Call the method again with same values
            AddEnterTime(destinationClusterId, time, originClusterId);
        }
    }

    public void AddLeaveTime(int originClusterId, DateTime time)
    {
        // Check if the cluster exists
        if (clusterHistograms.ContainsKey(originClusterId))
            clusterHistograms[originClusterId].AddLeaveTime(time);
        else
        {
            // Create the key
            clusterHistograms.Add(originClusterId, new PredictionHistogram());
            // Call the method again with same values
            AddLeaveTime(originClusterId, time);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("");

        foreach (KeyValuePair<int, PredictionHistogram> cluster in clusterHistograms)
        {
            sb.Append("## " + cluster.Key);
            sb.AppendLine();
            sb.Append(cluster.Value.ToString());
        }

        return sb.ToString();
    }

}
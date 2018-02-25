/**
 * Model for historical data
 *
 * @author Pawel Dworzycki
 * @version 25/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class HistoricalModel
{

    Dictionary<int, PredictionHistogram> clusterHistograms;

    public HistoricalModel()
    {
        clusterHistograms = new Dictionary<int, PredictionHistogram>();
    }

    public void AddEnterTime(int destinationClusterId, DateTime time)
    {
        // Check if the cluster exists
        if (clusterHistograms.ContainsKey(destinationClusterId))
        {
            clusterHistograms[destinationClusterId].AddEnterTime(time);
            // Add destination cluster
            clusterHistograms[destinationClusterId].AddDestinationCluster(destinationClusterId);
        }
        else
        {
            // Create the key
            clusterHistograms.Add(destinationClusterId, new PredictionHistogram());
            // Call the method again with same values
            AddEnterTime(destinationClusterId, time);
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
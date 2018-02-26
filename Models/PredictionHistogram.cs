/**
 * A histogram for historical data
 * All timestamps are rounded to the nearest quarter of an hour
 *
 * @author Pawel Dworzycki
 * @version 26/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PredictionHistogram
{

    public Dictionary<TimeSpan, int> enterTime { get; private set; }
    public Dictionary<TimeSpan, int> leaveTime { get; private set; }
    public Dictionary<int, int> destination { get; private set; }

    public PredictionHistogram()
    {
        enterTime = new Dictionary<TimeSpan, int>();
        leaveTime = new Dictionary<TimeSpan, int>();
        destination = new Dictionary<int, int>();
    }

    public void AddEnterTime(DateTime time)
    {
        // Round the time to the nearest 15 mins
        TimeSpan roundedTime = TimeSpanExtensions.RoundToNearestMinutes(time.TimeOfDay, 15);
        // Check if the time already exists
        if (enterTime.ContainsKey(roundedTime))
            enterTime[roundedTime]++;
        else
            enterTime.Add(roundedTime, 1);
    }

    public void AddLeaveTime(DateTime time)
    {
        // Round the time to the nearest 15 mins
        TimeSpan roundedTime = TimeSpanExtensions.RoundToNearestMinutes(time.TimeOfDay, 15);
        // Check if the time already exists
        if (leaveTime.ContainsKey(roundedTime))
            leaveTime[roundedTime]++;
        else
            leaveTime.Add(roundedTime, 1);
    }

    public void AddDestinationCluster(int destinationId)
    {
        // Check if the destination already exists
        if (destination.ContainsKey(destinationId))
            destination[destinationId]++;
        else
            destination.Add(destinationId, 1);
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("");

        sb.Append("ENTERED THE CLUSTER");
        sb.AppendLine();
        // Append enter times
        foreach (KeyValuePair<TimeSpan, int> keyVal in enterTime)
        {
            sb.Append(keyVal.Key + " - " + keyVal.Value);
            sb.AppendLine();
        }
        sb.Append("LEFT THE CLUSTER");
        sb.AppendLine();
        // Append leave times
        foreach (KeyValuePair<TimeSpan, int> keyVal in leaveTime)
        {
            sb.Append(keyVal.Key + " - " + keyVal.Value);
            sb.AppendLine();
        }
        sb.Append("DESTIONATIONS");
        sb.AppendLine();
        // Append destinations
        foreach (KeyValuePair<int, int> keyVal in destination)
        {
            sb.Append(keyVal.Key + " - " + keyVal.Value);
            sb.AppendLine();
        }
        sb.AppendLine();

        return sb.ToString();
    }

}
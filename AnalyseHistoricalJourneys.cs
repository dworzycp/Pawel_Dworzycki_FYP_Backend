/**
 * This class looks at user's histoircal journeys to create histograms
 *
 * @author Pawel Dworzycki
 * @version 26/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class AnalyseHistoricalJourneys
{

    public Dictionary<Day, HistoricalModel> predictions { get; private set; }

    public AnalyseHistoricalJourneys(Day[] days)
    {
        predictions = new Dictionary<Day, HistoricalModel>();

        foreach (Day d in days)
        {
            // Create a model for each day
            predictions.Add(d, new HistoricalModel());
            // Create the prediction model
            CreateAPredictionModel(d.historialJourneys, predictions[d]);
        }
    }

    private void CreateAPredictionModel(List<Journey> historicalJourneys, HistoricalModel histogram)
    {
        foreach (Journey j in historicalJourneys)
        {
            // Start time of a journey is the leave time for origin cluster
            histogram.AddLeaveTime(j.startClusterID, j.startTime);
            // End time of a journey is the enter time for destination cluster
            histogram.AddEnterTime(j.endClusterID, j.endTime, j.startClusterID);
        }
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder("");

        foreach (KeyValuePair<Day, HistoricalModel> day in predictions)
        {
            sb.Append("####################");
            sb.AppendLine();
            sb.Append("## " + day.Key.dayOfWeek);
            sb.AppendLine();
            sb.Append("####################");
            sb.AppendLine();
            sb.Append(day.Value.ToString());
        }

        return sb.ToString();
    }

}
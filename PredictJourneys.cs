/**
 * This class looks at user's histograms to make prdictions about future movement.
 *
 * @author Pawel Dworzycki
 * @version 26/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PredictJourneys
{

    public PredictJourneys(Dictionary<Day, HistoricalModel> histograms)
    {
        foreach (KeyValuePair<Day, HistoricalModel> dayHisto in histograms)
        {
            PredictMovement(dayHisto.Value, dayHisto.Key);
        }
    }

    private void PredictMovement(HistoricalModel model, Day day)
    {
        foreach (KeyValuePair<int, PredictionHistogram> clusterHistogram in model.clusterHistograms)
        {
            // Predict start time of the journey (i.e. LEAVE time for origin cluster)
            //      Get origin cluster from clusterHistogram.Key
            int startClusterId = clusterHistogram.Key;
            DateTime startTime = PredictTime(clusterHistogram.Value.leaveTime, day);

            // Predict which cluster the user will go to
            int desClusterId = PredictCluster(clusterHistogram.Value.destination);
            //Console.WriteLine(startClusterId + " " + desClusterId);

            // Predict end time of the journey (i.e. ENTER time for the destination cluster)
            // Check if the destination cluster has a histogram
            if (model.clusterHistograms.ContainsKey(desClusterId) != true)
                throw new Exception("Destination cluster does not have a histogram");

            try
            {
                DateTime endTime = PredictTime(model.clusterHistograms[desClusterId].enterTime, day);
                CreateJourney(startTime, endTime, startClusterId, desClusterId, day);
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }

    // Uses the most common value in the histogram
    private DateTime PredictTime(Dictionary<TimeSpan, int> timeValDictionary, Day day)
    {
        TimeSpan mostCommonVal = new TimeSpan();
        int mostCommonValCount = 0;

        foreach (KeyValuePair<TimeSpan, int> timeValPair in timeValDictionary)
            if (timeValPair.Value > mostCommonValCount)
            {
                mostCommonValCount = timeValPair.Value;
                mostCommonVal = timeValPair.Key;
            }

        // Convert TimeSpan to DateTime
        // IMPORTANT this step, although necessary, can be illogical as TimeSpan is a duration e.g.: 6 hours 15 mins
        //           whereas DateTime is a reference to a date/time e.g.: Monday 7th January 7:15 AM
        DateTime returnVal = TimeSpanToDateTime(mostCommonVal, day);

        return returnVal;
    }

    // Uses the most common value in the histogram
    private int PredictCluster(Dictionary<int, int> clusterValDictionary)
    {
        int desClusterId = 0;
        int mostCommonValCount = 0;

        foreach (KeyValuePair<int, int> clusValPair in clusterValDictionary)
            if (clusValPair.Value > mostCommonValCount)
            {
                mostCommonValCount = clusValPair.Value;
                //Console.WriteLine("Key with highest count: " + clusValPair.Key);
                desClusterId = clusValPair.Key;
            }

        if (desClusterId == 0)
            throw new Exception("Predicted a journey with no destination cluster");

        try
        {
            return desClusterId;
        }
        catch (System.Exception)
        {
            throw;
        }
    }

    private DateTime TimeSpanToDateTime(TimeSpan ts, Day d)
    {
        DateTime dT = DateTime.Today;
        // Add days
        for (int i = 0; i < 7; i++)
            if (dT.DayOfWeek != d.dayOfWeek)
                dT.AddDays(1);
        // Add time
        dT = dT + ts;
        return dT;
    }

    private void CreateJourney(DateTime startTime, DateTime endTime, int startClusterId, int endClusterId, Day d)
    {
        Journey j = new Journey(startTime, endTime, startClusterId, endClusterId);
        // Push the journey to the user's day
        d.AddJourney(j);
    }

}
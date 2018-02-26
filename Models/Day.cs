/**
 * This class models a day
 *
 * @author Pawel Dworzycki
 * @version 26/02/2018
 */
using System;
using System.Collections.Generic;
using System.Linq;

class Day
{

    public DayOfWeek dayOfWeek { get; private set; }
    public List<GeoPoint> historialGPSData { get; private set; }
    public List<Journey> historialJourneys { get; private set; }
    public List<Journey> journeys { get; private set; }

    public Day(DayOfWeek dayOfWeek)
    {
        this.dayOfWeek = dayOfWeek;
        historialGPSData = new List<GeoPoint>();
        historialJourneys = new List<Journey>();
        journeys = new List<Journey>();
    }

    public void AddGPSpoint(GeoPoint p)
    {
        historialGPSData.Add(p);
    }

    public void AddHistoricalJourney(Journey j)
    {
        historialJourneys.Add(j);
    }

    public void AddJourney(Journey j)
    {
        journeys.Add(j);
    }

}
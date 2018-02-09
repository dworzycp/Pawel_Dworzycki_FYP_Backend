// /**
//  * Predict the times a user leaves and enters a cluster
//  *
//  * @author Pawel Dworzycki
//  * @version 09/02/2018
//  */
// using System;
// using System.Collections.Generic;
// using System.Linq;

// class TimePrediction
// {

//     public double avgTimeHome_Enter { get; private set; }
//     public double avgTimeHome_Leave { get; private set; }

//     public double avgTimeWork_Enter { get; private set; }
//     public double avgTimeWork_Leave { get; private set; }

//     public TimePrediction(List<Cluster> clusters)
//     {
//         foreach (Cluster cluster in clusters)
//         {
//             #region Console Print
//             Console.WriteLine();
//             Console.WriteLine();
//             Console.WriteLine("##################################");
//             Console.WriteLine("#           NEW CLUSTER          #");
//             Console.WriteLine("##################################");
//             Console.WriteLine();
//             #endregion
//             // Split the points by day
//             Dictionary<String, List<Point>> pointsByDay = SplitPointsByDay(cluster.points);

//         }
//     }

//     #region Sorting

//     private Dictionary<String, List<Point>> SplitPointsByDay(List<Point> points)
//     {
//         Dictionary<String, List<Point>> pointsByDay = new Dictionary<String, List<Point>>();

//         // Sort the points
//         foreach (Point p in points)
//             SortPoint(pointsByDay, p);

//         return pointsByDay;
//     }

//     private void SortPoint(Dictionary<String, List<Point>> daysDic, Point p)
//     {
//         // Check if the key already exists
//         String k = p.createdAt.ToShortDateString();
//         if (daysDic.ContainsKey(k))
//         {
//             // Key exists, Add value to the list of points for that key
//             List<Point> day = daysDic[k];
//             day.Add(p);
//         }
//         else
//         {
//             // Create key
//             daysDic.Add(k, new List<Point>());
//             // Now that the key exists, call the method again with the same parameters
//             SortPoint(daysDic, p);
//         }
//     }

//     #endregion

//     /**
//      * ASSUMPTION can distingush between home and work clusters
//      * The time a user enters work will be the earliest point at the work cluster
//      * 
//      *
//      *
//      * TODO how about people who work nightshift?
//      */
//     private void Predict_Work()
//     {

//     }

// }
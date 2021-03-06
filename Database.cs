/**
 * This class connects to a database and retireves data from it
 * @author Pawel Dworzycki
 * @version 18/04/2018
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

class Database
{
    SqlConnection connection;
    bool isUsingTestData;

    public Database(bool isUsingTestData = false)
    {
        this.isUsingTestData = isUsingTestData;
    }

    public List<GeoPoint> GetUnclassifiedCoordinates(int daysToGenerateFakeDataFor = 21)
    {
        List<GeoPoint> points = new List<GeoPoint>();

        if (isUsingTestData == false)
        {
            string cmdString = "SELECT * FROM GPS_Coords WHERE clusterId IS NULL";
            SetUpConnection();
            try
            {
                using (connection)
                {
                    var command = new SqlCommand(cmdString, connection);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            double lat = Convert.ToDouble(reader["latitude"].ToString());
                            double lon = Convert.ToDouble(reader["longitude"].ToString());
                            string userId = Convert.ToString(reader["user_id"].ToString());
                            DateTime createdDate = Convert.ToDateTime(reader["actual_createdAt"].ToString());

                            GeoPoint p = new GeoPoint(lat, lon, userId);
                            p.SetTime(createdDate);
                            points.Add(p);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
        else
        {
            FakeDataGen fdg = new FakeDataGen(daysToGenerateFakeDataFor);
            points = fdg.points;
        }

        return points;
    }

    public void SaveCluster(Cluster c)
    {
        if (isUsingTestData == false)
        {
            string cmdString = "INSERT INTO Clusters (c_centre_lat, c_centre_long, c_radius, c_label, userId, c_id) VALUES ('" + c.centrePoint.latitude + "', '" + c.centrePoint.longitude + "', '" + c.radiusInMeters + "', '" + c.SemanticLabel + "', '" + c.userId + "', '" + c.clusterId + "')";
            SetUpConnection();
            try
            {
                using (connection)
                {
                    var command = new SqlCommand(cmdString, connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public List<Cluster> GetClusters()
    {
        List<Cluster> clusters = new List<Cluster>();

        if (isUsingTestData == false)
        {
            string cmdString = "SELECT * FROM Clusters";
            SetUpConnection();
            try
            {
                using (connection)
                {
                    var command = new SqlCommand(cmdString, connection);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = Convert.ToString(reader["id"].ToString());
                            double lat = Convert.ToDouble(reader["c_centre_lat"].ToString());
                            double lon = Convert.ToDouble(reader["c_centre_long"].ToString());
                            double r = Convert.ToDouble(reader["c_radius"].ToString());
                            string name = Convert.ToString(reader["c_label"].ToString());
                            string userId = Convert.ToString(reader["userId"].ToString());
                            int clusterIndex = Convert.ToInt32(reader["c_id"].ToString());

                            Cluster c = new Cluster();
                            c.clusterDBId = id;
                            c.centrePoint = new GeoPoint(lat, lon, userId);
                            c.radiusInMeters = r;
                            c.SemanticLabel = name;
                            c.userId = userId;
                            c.clusterId = clusterIndex;

                            clusters.Add(c);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        return clusters;
    }

    public void UpdateClustersLabel(String clusterId, String label)
    {
        if (clusterId != null && isUsingTestData == false)
        {
            string cmdString = "UPDATE Clusters SET c_label = '" + label + "' WHERE id = '" + clusterId + "'";

            SetUpConnection();

            try
            {
                using (connection)
                {
                    var command = new SqlCommand(cmdString, connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
    }

    public void SavePrediction(Journey j, Dictionary<int, Cluster> idToCluterMap)
    {
        if (isUsingTestData == false)
        {
            string originClusterName = idToCluterMap[j.startClusterID].SemanticLabel;
            string origin_lat = idToCluterMap[j.startClusterID].centrePoint.latitude.ToString();
            string origin_long = idToCluterMap[j.startClusterID].centrePoint.longitude.ToString();

            string endClusterName = idToCluterMap[j.endClusterID].SemanticLabel;
            string end_lat = idToCluterMap[j.endClusterID].centrePoint.latitude.ToString();
            string end_long = idToCluterMap[j.endClusterID].centrePoint.longitude.ToString();

            // To insert the date into the DB it has to be of yyyy/M/dd hh:mm:ss tt format
            string cmdString = "INSERT INTO Predictions (OriginClusterID, DestClusterID, LeaveTime, EnterTime, UserID, LengthInMins, OriginClusterName, DestClusterName, origin_lat, origin_long, dest_lat, dest_long) VALUES ('" + j.startClusterID + "', '" + j.endClusterID + "', '" + ConvertDateForDB(j.startTime) + "', '" + ConvertDateForDB(j.endTime) + "', '" + j.userId + "', '" + j.LengthInMins() + "', '" + originClusterName + "', '" + endClusterName + "', '" + origin_lat + "', '" + origin_long + "', '" + end_lat + "', '" + end_long + "')";

            SetUpConnection();

            try
            {
                using (connection)
                {
                    var command = new SqlCommand(cmdString, connection);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
        }
    }

    private void SetUpConnection()
    {
        connection = new SqlConnection("Server=tcp:pawelfypdb.database.windows.net,1433;Initial Catalog=PawelFYPDB;Persist Security Info=True;User Id=pawel;Password=Twirlbites11");
    }

    private String ConvertDateForDB(DateTime date)
    {
        String rtnVal = "";

        // Append the year/month/day
        rtnVal += date.ToString("yyyy-M-dd");
        rtnVal += " ";
        rtnVal += date.ToShortTimeString();
        rtnVal += ":00";
        // Need a more robust way of checking if summer time
        // TODO -- will be fine for now
        rtnVal += "+01:00";

        return rtnVal;
    }

}
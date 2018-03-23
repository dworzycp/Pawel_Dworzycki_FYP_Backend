/**
 * This class connects to a database and retireves data from it
 * @author Pawel Dworzycki
 * @version 23/03/2018
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.SqlClient;

class Database
{
    SqlConnection connection;

    public Database()
    {
        connection = new SqlConnection("Server=tcp:pawelfypdb.database.windows.net,1433;Initial Catalog=PawelFYPDB;Persist Security Info=True;User Id=pawel;Password=Twirlbites11");
    }

    public List<GeoPoint> GetUnclassifiedCoordinates()
    {
        List<GeoPoint> points = new List<GeoPoint>();
        string cmdString = "SELECT * FROM GPS_Coords WHERE clusterId IS NULL";
        //string cmdString = "SELECT latitude, longitude FROM GPS_Coords WHERE user_id = '" + userId + "' AND clusterId IS NULL AND createdAt LIKE '" + date + "%'";

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

        return points;
    }
}
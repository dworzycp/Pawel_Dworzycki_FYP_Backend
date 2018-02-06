/**
 * This class connects to a database and retireves data from it
 * @author Pawel Dworzycki
 * @version 25/01/2018
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

    public List<Point> GetUnclassifiedCoordinates(String userId, String date)
    {
        List<Point> points = new List<Point>();
        string cmdString = "SELECT latitude, longitude FROM GPS_Coords WHERE user_id = '" + userId + "' AND clusterId IS NULL AND createdAt LIKE '" + date + "%'";

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
                        var lat = Convert.ToDouble(reader["latitude"].ToString());
                        var lon = Convert.ToDouble(reader["longitude"].ToString());
                        Point p = new Point(lat, lon);
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
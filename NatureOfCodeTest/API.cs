using NatureOfCodeTest.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace NatureOfCodeTest
{
    public class API
    {
        private const string URL = "https://api.openweathermap.org/data/2.5/weather?q=Southampton,uk&appid=8316724b67d895ab2649049167efaf76";
        //private const string URL = "http://api.wunderground.com/api/4d8a9c758fdb28de/conditions/q/Geneva.json";
        private const string PlanetURL = "https://exoplanetarchive.ipac.caltech.edu/TAP/sync?query=select+top+50+hostname,pl_name,pl_masse,pl_orbsmax,pl_orbeccen,pl_orbincl,pl_orblper+from+ps+where+pl_rade+<+=+1.8+and+pl_masse+>+0+and+rv_flag=1+and+pl_orbsmax+is+not+null+and+pl_orbeccen+is+not+null+and+pl_orbincl+is+not+null+and+pl_orblper+is+not+null&format=json";
        private const string StarURL = "https://exoplanetarchive.ipac.caltech.edu/TAP/sync?query=select+top+50+hostname,st_mass,st_lum+from+stellarhosts+where+st_mass+is+not+null+and+st_lum+is+not+null&format=json";
        private const string DATA = @"{""object"":{""name"":""Name""}}";
        public async void Token()
        {
            string returnedData = await GetDataAsync(PlanetURL);
            var weather = JsonConvert.DeserializeObject<List<PlanetFJson>>(returnedData);
        }
        private async Task<string> GetDataAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return responseBody;
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                }
            }
            return "";
        }
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = " + Environment.CurrentDirectory + @"\StellarWobble.accdb";
        public void AddStudent(PlanetFJson planet)
        {
            string sql = "INSERT INTO tblStudent (FirstName, LastName, StudentDOB) VALUES (?, ?, ?)";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@FirstName", student.FirstName);
                cmd.Parameters.AddWithValue("@LastName", student.LastName);
                cmd.Parameters.AddWithValue("@StudentDOB", student.StudentDOB.ToString("yyyy-MM-dd"));
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        //pasted json string to json2csharp
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class PlanetFJson
        {
            public string HostName { get; set; }
            public string pl_Name { get; set; }
            public double pl_Masse { get; set; } // planet mass, how many times of earth's mass
            public double pl_orbsmax { get; set; } // a - planet SemiMajorAxis
            public double pl_orbeccen { get; set; }  // e - planet Eccentricity
            public double pl_orbincl { get; set; }   // i (degrees) - planet  Inclination
            public double pl_orblper { get; set; } // ω - planet ArgumentOfPeriapsis 
            //  MeanAnomalyAtEpoch and EpochTime are not needed, it only used to determine the initial state, which could be anywhere when being observed in real life.
            //  We just want to set up certain place for simplicity.
        }
        public class StarFJson
        {
            public string HostName { get; set; } // star name
            public double st_mass { get; set; } // star mass
            public double st_lum { get; set; } // star luminosity
        }
        public class Clouds
        {
            public int all { get; set; }
        }

        public class Coord
        {
            public double lon { get; set; }
            public double lat { get; set; }
        }

        public class Main
        {
            public double temp { get; set; }
            public double feels_like { get; set; }
            public double temp_min { get; set; }
            public double temp_max { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public int sea_level { get; set; }
            public int grnd_level { get; set; }
        }

        public class Root
        {
            public Coord coord { get; set; }
            public List<Weather> weather { get; set; }
            public string @base { get; set; }
            public Main main { get; set; }
            public int visibility { get; set; }
            public Wind wind { get; set; }
            public Clouds clouds { get; set; }
            public int dt { get; set; }
            public Sys sys { get; set; }
            public int timezone { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public int cod { get; set; }
        }

        public class Sys
        {
            public int type { get; set; }
            public int id { get; set; }
            public string country { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

        public class Wind
        {
            public double speed { get; set; }
            public int deg { get; set; }
        }
    }

}

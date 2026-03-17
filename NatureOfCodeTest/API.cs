using NatureOfCodeTest.Model;
//using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace NatureOfCodeTest
{
    public class API
    {
        private const string URL = "https://api.openweathermap.org/data/2.5/weather?q=Southampton,uk&appid=8316724b67d895ab2649049167efaf76";
        //private const string URL = "http://api.wunderground.com/api/4d8a9c758fdb28de/conditions/q/Geneva.json";
        private const string PlanetURL = "exoplanetarchive.ipac.caltech.edu/TAP/sync?query=select+top+50+hostname,pl_name,pl_masse,pl_orbsmax,pl_orbeccen,pl_orbincl,pl_orblper+from+ps+where+pl_rade+<+=+1.8+and+pl_masse+>+0+and+rv_flag=1+and+pl_orbsmax+is+not+null+and+pl_orbeccen+is+not+null+and+pl_orbincl+is+not+null+and+pl_orblper+is+not+null&format=json";
        private const string StarURL = "exoplanetarchive.ipac.caltech.edu/TAP/sync?query=select+top+50+hostname,st_mass,st_lum+from+stellarhosts+where+st_mass+is+not+null+and+st_lum+is+not+null&format=json";
        private const string DATA = @"{""object"":{""name"":""Name""}}";
        private async void Token()
        {
            string returnedData = await GetDataAsync(URL);
            var weather = JsonSerializer.Deserialize<List<List<Planet>>(returnedData);
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
        //pasted json string to json2csharp
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class PlanetFJson
        {
            public string HostName { get; set; }
            public string Name { get; set; }
            public double Mass { get; set; }
            public double SemiMajorAxis { get; set; } // a
            public double Eccentricity { get; set; }  // e
            public double Inclination { get; set; }   // i (độ)
            public double ArgumentOfPeriapsis { get; set; } // ω
        }
        public class StarFJson
        {
            public string Name { get; set; }
            public double Mass { get; set; }
            public double Luminosity { get; set; }
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

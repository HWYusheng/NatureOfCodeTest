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
        CelesBodyRepositary celesBodyRepositary = new CelesBodyRepositary();
        public async void UpdateDataToAccessDB()
        {
            string returnedPlanetData = await GetDataAsync(PlanetURL);
            string returnedStarData = await GetDataAsync(StarURL);
            var planetsCol = JsonConvert.DeserializeObject<List<PlanetFJson>>(returnedPlanetData);
            var starsCol = JsonConvert.DeserializeObject<List<StarFJson>>(returnedStarData);
            foreach (var item in planetsCol)
            {
                Planet _planet = new Planet
                {
                    Name = item.pl_Name,
                    Mass = item.pl_Masse,
                    Orbit.SemiMajorAxis = item.pl_orbsmax,
                    Orbit.Eccentricity = item.pl_orbeccen,
                    Orbit.Inclination  = item.pl_inclination,
                    Orbit.ArgumentOfPeriapsis = item.pl_orblper,
                };
                celesBodyRepositary.AddPlanet(_planet);
            }
            foreach (var item in starsCol)
            {
                Star _star = new Star
                {
                    Name = item.HostName,
                    Mass = item.st_mass,
                    Luminosity = item.st_lum,
                };
                celesBodyRepositary.AddStar(_star);
            }
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
    }

}

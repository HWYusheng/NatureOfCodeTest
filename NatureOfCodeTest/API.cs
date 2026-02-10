using NatureOfCodeTest.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest
{
    public static class Consts
    {
        public static string SPOONACULAR_API_KEY = "MY_KEY";
    }
    public class SpoonacularService
    {
        public async Task<IEnumerable<Star>> GetStars(String query)
        {
            List<Star> starList = new List<Star>();

            var url = $"https://exoplanetarchive.ipac.caltech.edu/cgi-bin/nstedAPI/nph-nstedAPI?";
            var parameters = $"?query={query}&apiKey={Consts.SPOONACULAR_API_KEY}&number=5";

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            HttpResponseMessage response = await client.GetAsync(parameters).ConfigureAwait(false);

            if (response.IsSuccessStatusCode)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                var stars = JsonConvert.DeserializeObject<List<Star>>(jsonString);

                if (stars != null)
                {
                    starList.AddRange(starList);
                }
            }

            return starList;
        }
    }
}

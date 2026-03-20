using NatureOfCodeTest.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest
{
    internal class CelesBodyRepositary
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = " + Environment.CurrentDirectory + @"\StellarWobble.accdb";
        public void AddPlanet(PlanetFJson planet)
        {
            string sql = "INSERT INTO tblPlanet (HostName, pl_Name, pl_Masse, pl_SemiMajorAxis, pl_Eccentricity, pl_Inclination, pl_ArgumentOfPeriapsis) VALUES (?, ?, ?, ?, ?, ?, ?)";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@HostName", planet.HostName);
                cmd.Parameters.AddWithValue("@pl_Name", planet.pl_Name);
                cmd.Parameters.AddWithValue("@pl_Masse", planet.pl_Masse);
                cmd.Parameters.AddWithValue("@pl_SemiMajorAxis", planet.pl_orbsmax);
                cmd.Parameters.AddWithValue("@pl_Eccentricity", planet.pl_orbeccen);
                cmd.Parameters.AddWithValue("@pl_Inclination", planet.pl_orbincl);
                cmd.Parameters.AddWithValue("@pl_ArgumentOfPeriapsis", planet.pl_orblper);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void AddStar(StarFJson star)
        {
            string sql = "INSERT INTO tblStar (HostName, st_Mass, st_Luminosity) VALUES (?, ?, ?)";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@HostName", star.HostName);
                cmd.Parameters.AddWithValue("@st_Mass", star.st_mass);
                cmd.Parameters.AddWithValue("@st_Luminosity", star.st_lum);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        // This one will try to get a whole star system. Meaning: host star + planet(s)
        // Probably a list with different name for same parameters of the 2, or try to use the celesbody. Or do a method that can store 2 different types at the same time.
        public List<CelestialBody> GetSystem()
        {
            List<CelestialBody> CelesBodies = new List<CelestialBody>();
            string sqlFStar = "SELECT * FROM tblStar";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(sqlFStar, conn))
            {
                conn.Open();
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Star star = new Star
                        {
                        };
                        CelesBodies.Add(star);
                    }
                }
            }
            string sqlFPlanet = "SELECT * FROM tblPlanet";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(sqlFPlanet, conn))
            {
                conn.Open();
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Planet planet = new Planet
                        {
                        };
                        CelesBodies.Add(planet);
                    }
                }
            }
            return CelesBodies;
        }
        //pasted json string to json2csharp
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        [Serializable]
        public abstract class CelestialBodyFJson
        {
            [JsonProperty("name")]
            public string Name { get; set; }
            public double Mass { get; set; }

        }
        public class PlanetFJson : CelestialBodyFJson
        {
            // 7 in total
            public string HostName { get; set; }
            public string pl_Name { get; set; }
            public double pl_Masse { get; set; } // planet mass, how many times of earth's mass
            public double pl_orbsmax { get; set; } // a - planet SemiMajorAxis
            public double pl_orbeccen { get; set; }  // e - planet Eccentricity
            public double pl_orbincl { get; set; }   // i (degrees) - planet  Inclination
            public double pl_orblper { get; set; } // ω - planet ArgumentOfPeriapsis 
            //  MeanAnomalyAtEpoch and EpochTime are not needed, it only used to determine the initial state, which could be anywhere when being observed in real life.
            //  We just want to set up a certain place for simplicity.
        }
        public class StarFJson : CelestialBodyFJson
        {
            // 3 in total
            public string HostName { get; set; } // basiclly star name
            public double st_mass { get; set; } // star mass
            public double st_lum { get; set; } // star luminosity
        }
    }
}

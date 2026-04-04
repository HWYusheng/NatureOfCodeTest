using NatureOfCodeTest.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest
{
    public class SystemFetchData
    {
        public Star HostStar { get; set; }
        public Planet OrbitingPlanet { get; set; }
        public int PlanetID { get; set; }
    }

    internal class CelesBodyRepositary
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = " + Environment.CurrentDirectory + @"\StellerWobble.accdb";
        
        public void AddPlanet(Planet planet)
        {
            int StarID = GetStarID(planet.HostName);
            if (StarID == -1)
            {
                return;
            }
            string sql = "INSERT INTO tblPlanet (StarID, pl_Name, pl_Masse, pl_orbsmax, pl_orbeccen, pl_orbincl, pl_orblper) VALUES (?, ?, ?, ?, ?, ?, ?)";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@StarID", StarID);
                cmd.Parameters.AddWithValue("@pl_Name", planet.Name);
                cmd.Parameters.AddWithValue("@pl_Masse", planet.Mass);
                cmd.Parameters.AddWithValue("@pl_orbsmax", planet.Orbit.SemiMajorAxis);
                cmd.Parameters.AddWithValue("@pl_orbeccen", planet.Orbit.Eccentricity);
                cmd.Parameters.AddWithValue("@pl_orbincl", planet.Orbit.Inclination);
                cmd.Parameters.AddWithValue("@pl_orblper", planet.Orbit.ArgumentOfPeriapsis);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        //public string HostName { get; set; }
        //public string pl_Name { get; set; }
        //public double pl_Masse { get; set; } // planet mass, how many times of earth's mass
        //public double pl_orbsmax { get; set; } // a - planet SemiMajorAxis
        //public double pl_orbeccen { get; set; }  // e - planet Eccentricity
        //public double pl_orbincl { get; set; }   // i (degrees) - planet  Inclination
        //public double pl_orblper { get; set; } // ω - planet ArgumentOfPeriapsis 
        public void AddStar(Star star)
        {
            string sql = "INSERT INTO tblStar (HostName, st_Mass, st_Lum) VALUES (?, ?, ?)";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@HostName", star.Name);
                cmd.Parameters.AddWithValue("@st_Mass", star.Mass);
                cmd.Parameters.AddWithValue("@st_Lum", star.Luminosity);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public int GetStarID(string HostName) 
        {
            int _starID = -1;
            string sql = "SELECT StarID FROM tblStar WHERE HostName = ?"; // ? is a placeholder for parameters in OleDb
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(sql, conn))
            {
                conn.Open();
                cmd.Parameters.AddWithValue("@HostName", HostName); // add the parameter value

                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        _starID = reader.GetInt32(0);
                    };
                }
            }

            return _starID;
        }
        public void DeleteAllSQL()
        {
            List<string> tblList = new List<string> { "tblStar", "tblPlanet" };
            foreach (var item in tblList)
            {
                string deleteAllSql = "DELETE FROM " + item;
                using (OleDbConnection conn = new OleDbConnection(connectionString))
                using (OleDbCommand cmd = new OleDbCommand(deleteAllSql, conn))
                {
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }


        }
        // This one will try to get a whole star system. Meaning: host star + planet(s)
        // Probably a list with different name for same parameters of the 2, or try to use the celesbody. Or do a method that can store 2 different types at the same time.
        public List<CelestialBody> GetSystem()
        {
            List<CelestialBody> CelesBodies = new List<CelestialBody>();
            string sqlFStar = "SELECT * FROM tblStar, tblPlanet WHERE ";
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

        public SystemFetchData GetRandomSystem()
        {
            SystemFetchData data = new SystemFetchData();
            List<int> planetIds = new List<int>();
            string sqlGetIds = "SELECT PlanetID FROM tblPlanet";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            {
                conn.Open();
                using (OleDbCommand cmd = new OleDbCommand(sqlGetIds, conn))
                {
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            planetIds.Add(reader.GetInt32(0));
                        }
                    }
                }
                
                if (planetIds.Count == 0) return null;

                Random rnd = new Random();
                int selectedPlanetId = planetIds[rnd.Next(planetIds.Count)];

                string sqlPlanet = "SELECT * FROM tblPlanet WHERE PlanetID = ?";
                int starId = -1;
                using (OleDbCommand cmd = new OleDbCommand(sqlPlanet, conn))
                {
                    cmd.Parameters.AddWithValue("@PlanetID", selectedPlanetId);
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            data.PlanetID = reader.GetInt32(0);
                            starId = reader.GetInt32(1);
                            
                            Planet p = new Planet();
                            p.Name = reader.GetString(2);
                            p.Mass = Convert.ToDouble(reader.GetValue(3)); 
                            double a = Convert.ToDouble(reader.GetValue(4)); 
                            double e = Convert.ToDouble(reader.GetValue(5));
                            double i = Convert.ToDouble(reader.GetValue(6));
                            double w = Convert.ToDouble(reader.GetValue(7));
                            
                            p.Orbit = new OrbitalElements
                            {
                                SemiMajorAxis = a,
                                Eccentricity = e,
                                Inclination = i,
                                ArgumentOfPeriapsis = w
                            };
                            data.OrbitingPlanet = p;
                        }
                    }
                }

                string sqlStar = "SELECT * FROM tblStar WHERE StarID = ?";
                using (OleDbCommand cmd = new OleDbCommand(sqlStar, conn))
                {
                    cmd.Parameters.AddWithValue("@StarID", starId);
                    using (OleDbDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            Star s = new Star();
                            s.Name = reader.GetString(1);
                            s.Mass = Convert.ToDouble(reader.GetValue(2)); 
                            s.Luminosity = Convert.ToDouble(reader.GetValue(3));
                            s.Position = new Vector2(0, 0); 
                            data.HostStar = s;
                        }
                    }
                }
            }
            return data;
        }
        //pasted json string to json2csharp
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        //[Serializable]
        //public abstract class CelestialBodyFJson
        //{
        //    [JsonProperty("name")]
        //    public string Name { get; set; }
        //    public double Mass { get; set; }

        //}
        //public class PlanetFJson : CelestialBodyFJson
        //{
        //    // 7 in total
        //    public string HostName { get; set; }
        //    public string pl_Name { get; set; }
        //    public double pl_Masse { get; set; } // planet mass, how many times of earth's mass
        //    public double pl_orbsmax { get; set; } // a - planet SemiMajorAxis
        //    public double pl_orbeccen { get; set; }  // e - planet Eccentricity
        //    public double pl_orbincl { get; set; }   // i (degrees) - planet  Inclination
        //    public double pl_orblper { get; set; } // ω - planet ArgumentOfPeriapsis 
        //    //  MeanAnomalyAtEpoch and EpochTime are not needed, it only used to determine the initial state, which could be anywhere when being observed in real life.
        //    //  We just want to set up a certain place for simplicity.
        //}
        //public class StarFJson : CelestialBodyFJson
        //{
        //    // 3 in total
        //    public string HostName { get; set; } // basiclly star name
        //    public double st_mass { get; set; } // star mass
        //    public double st_lum { get; set; } // star luminosity
        //}
    }
}

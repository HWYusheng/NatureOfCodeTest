using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NatureOfCodeTest
{
    internal class CelesBodyRepositary
    {
        string connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source = " + Environment.CurrentDirectory + @"\StellarWobble.accdb";
        public void AddPlanet(PlanetFJson planet)
        {
            string sql = "INSERT INTO tblStudent (FirstName, LastName, StudentDOB) VALUES (?, ?, ?, ?, ?, ?, ?)";
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
            string sql = "INSERT INTO tblStudent (FirstName, LastName, StudentDOB) VALUES (?, ?, ?)";
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
        public List<Student> GetSystem()
        {
            List<Student> students = new List<Student>();
            string sql = "SELECT * FROM tblStudent";
            using (OleDbConnection conn = new OleDbConnection(connectionString))
            using (OleDbCommand cmd = new OleDbCommand(sql, conn))
            {
                conn.Open();
                using (OleDbDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Student student = new Student
                        {
                            StudentID = reader.GetInt32(0), // the first column is StudentID
                            FirstName = reader.GetString(1), // the second column is FirstName
                            LastName = reader.GetString(2), // the third column is LastName
                            StudentDOB = reader.GetDateTime(3) // the fourth column is StudentDOB
                        };
                        students.Add(student);
                    }
                }
            }
            return students;
        }
        //pasted json string to json2csharp
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class PlanetFJson
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
            //  We just want to set up certain place for simplicity.
        }
        public class StarFJson
        {
            // 3 in total
            public string HostName { get; set; } // star name
            public double st_mass { get; set; } // star mass
            public double st_lum { get; set; } // star luminosity
        }
    }
}

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
            public double pl_Masse { get; set; } // planet mass
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

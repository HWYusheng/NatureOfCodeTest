using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NatureOfCodeTest
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }

    static void Main(string[] args)
        {
            // 1. Setup Physical Entities
            Star sun = new Star
            {
                Name = "Sol",
                Mass = PhysicalConstants.SolarMass,
                Position = new Vector2(0, 0)
            };

            Planet earth = new Planet
            {
                Name = "Earth",
                Mass = 5.972e24, // kg
                Orbit = new OrbitalElements
                {
                    SemiMajorAxis = PhysicalConstants.AU,
                    Eccentricity = 0.0, // Circular orbit
                    Inclination = 0,
                    ArgumentOfPeriapsis = 0,
                    MeanAnomalyAtEpoch = 0,
                    EpochTime = 0
                }
            };

            // 2. Initialize Engine
            SimulationEngine engine = new SimulationEngine
            {
                HostStar = sun,
                OrbitingPlanet = earth,
                TimeStep = 86400 * 30 // Move by 30 days per step
            };

            Console.WriteLine($"Simulating {earth.Name} orbiting {sun.Name}...");
            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine($"{"Day",-10} | {"Pos X (AU)",-12} | {"Pos Y (AU)",-12} | {"Vel (km/s)",-10}");
            Console.WriteLine("--------------------------------------------------------------------------------");

            // 3. Run Simulation for 12 steps (approx. 1 year)
            for (int i = 0; i <= 12; i++)
            {
                // Print current state
                double posX_AU = earth.Position.X / PhysicalConstants.AU;
                double posY_AU = earth.Position.Y / PhysicalConstants.AU;
                double vel_kms = earth.Velocity.Magnitude() / 1000.0;

                Console.WriteLine($"{i * 30,-10} | {posX_AU,12:F4} | {posY_AU,12:F4} | {vel_kms,10:F2}");

                // Advance simulation
                engine.Step();
            }

            Console.WriteLine("--------------------------------------------------------------------------------");
            Console.WriteLine("Simulation Complete.");
            Console.ReadLine();
        }
    }

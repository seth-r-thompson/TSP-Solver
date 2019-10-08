using System;
using System.Collections.Generic;

namespace TravellingSalesmanProblemLibrary
{
    public class Tsp
    {
        public string Name { get; set; }
        public List<string> Comments { get; set; }
        public int Dimension { get; set; }
        public string EdgeWeight { get; set; }
        public Path Cities { get; set; }
        public bool IsSolved { get; set; }
        public TspSolution Solution { get; set; }

        public Tsp()
        {
            this.Comments = new List<String>();
            this.Cities = new Path();
        }

        /// <summary>
        /// Prints TSP info to debug console.
        /// </summary>
        public void Print()
        {
            System.Diagnostics.Debug.WriteLine($"Name: {this.Name}\nComments:");

            foreach (var comment in this.Comments)
            {
                System.Diagnostics.Debug.WriteLine($"\t{comment}");
            }

            System.Diagnostics.Debug.WriteLine($"Graph Type: {this.EdgeWeight}\nCity Count: {this.Dimension}\nCities:");

            foreach (var city in Cities)
            {
                System.Diagnostics.Debug.WriteLine($"\tCity {city.Name}: {city.Coordinate.X}, {city.Coordinate.Y}");
            }
        }
    }
}

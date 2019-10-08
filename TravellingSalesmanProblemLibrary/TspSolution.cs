using System.Collections.Generic;

namespace TravellingSalesmanProblemLibrary
{
    public class TspSolution
    {
        /// <summary>
        /// Shortest path to traverse the TSP.
        /// </summary>
        public Path ShortestPath { get; set; }

        /// <summary>
        /// The runtime the solver took to get the solution.
        /// </summary>
        public double Runtime { get; set; }

        /// <summary>
        /// Amount of distance calculations performed to get the solution.
        /// </summary>
        public int CalculationAmount { get; set; }
    }
}

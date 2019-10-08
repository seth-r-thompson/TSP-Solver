using System;
using System.Collections.Generic;
using System.Text;

namespace TravellingSalesmanProblemLibrary
{
    public class City
    {
        public int Name { get; set; }

        public Coordinate Coordinate { get; set; }

        /// <summary>
        /// Initializes a city node.
        /// </summary>
        /// <param name="name">Key which identifies the city.</param>
        /// <param name="coordinate">The coordinate pair where the city is located.</param>
        public City(int name, Coordinate coordinate)
        {
            this.Name = name;
            this.Coordinate = coordinate;
        }
    }

    public struct Coordinate
    {
        public double X { get; set; }

        public double Y { get; set; }

        /// <summary>
        /// Initializes a Cartesian coordinate pair.
        /// </summary>
        /// <param name="x">Horizontal value of the coordiante pair.</param>
        /// <param name="y">Vertical value of the coordinate pair.</param>
        public Coordinate(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }
    }
}

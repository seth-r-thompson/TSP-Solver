using System;
using System.Collections.Generic;
using System.Text;

namespace TravellingSalesmanProblemLibrary
{
    public class Distance
    {
        /// <summary>
        /// Calculates the distance between 2 cities.
        /// </summary>
        /// <param name="origin">Origin city.</param>
        /// <param name="destination">Destination city.</param>
        /// <returns>The distance between the 2 cities using the distance formula.</returns>
        public static double Between(City origin, City destination)
        {
            //Returns calculation of the distance formula
            return Math.Sqrt(Math.Pow(destination.Coordinate.X - origin.Coordinate.X, 2) + Math.Pow(destination.Coordinate.Y - origin.Coordinate.Y, 2));
        }

        /// <summary>
        /// Calculates the distance between a city and a line segment between two other cities.
        /// </summary>
        /// <param name="origin">Origin city.</param>
        /// <param name="edgeStart">First vertex of the line segment.</param>
        /// <param name="edgeEnd">Second vertex of the line segment.</param>
        /// <returns>The distance between a city and the line segment between two other cities.</returns>
        public static double Between(City origin, City edgeStart, City edgeEnd)
        {
            // Length of edge, squared
            var edgeLength = Math.Pow(Distance.Between(edgeStart, edgeEnd), 2);

            if (edgeLength == 0)
            {
                return Distance.Between(origin, edgeStart);
            }

            var parameterization = ((origin.Coordinate.X - edgeStart.Coordinate.X) * (edgeEnd.Coordinate.X - edgeStart.Coordinate.X) 
                                    + (origin.Coordinate.Y - edgeStart.Coordinate.Y) * (edgeEnd.Coordinate.Y - edgeStart.Coordinate.Y)) 
                                    / edgeLength;

            if (parameterization < 0)
            {
                return Distance.Between(origin, edgeStart);
            }
            else if (parameterization > 1)
            {
                return Distance.Between(origin, edgeEnd);
            }
            else
            {
                var projectionCoordinate = new Coordinate
                {
                    X = edgeStart.Coordinate.X + (edgeEnd.Coordinate.X - edgeStart.Coordinate.X) * parameterization,
                    Y = edgeStart.Coordinate.Y + (edgeEnd.Coordinate.Y - edgeStart.Coordinate.Y) * parameterization
                };

                var projection = new City(0, projectionCoordinate);

                return Distance.Between(origin, projection);
            }
        }
    }
}

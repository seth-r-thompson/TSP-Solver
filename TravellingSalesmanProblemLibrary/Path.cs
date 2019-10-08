using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TravellingSalesmanProblemLibrary
{
    public class Path : List<City>
    {
        /// <summary>
        /// The length of the path if the path were complete
        /// </summary>
        public double CompleteLength
        {
            get
            {
                double pathLength = 0;
                var path = new Path(this);
                var firstCity = path[0];

                foreach (var city in path)
                {
                    // If it's the last city, calculate the distance between it and the first
                    if (city == path.Last())
                    {
                        pathLength += Distance.Between(city, firstCity);
                    }
                    // Otherwise calculate the distance between the current and next cities
                    else
                    {
                        pathLength += Distance.Between(city, path[path.IndexOf(city) + 1]);
                    }
                }

                return pathLength;
            }
        }

        /// <summary>
        /// The length of the path
        /// </summary>
        public double Length
        {
            get
            {
                double pathLength = 0;
                var path = new Path(this);

                foreach (var city in path)
                {
                    if (city != path.Last())
                    {
                        pathLength += Distance.Between(city, path[path.IndexOf(city) + 1]);
                    }
                }

                return pathLength;
            }
        }

        public Path()
        {

        }

        public Path(Path path) : base(path as List<City>)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using TravellingSalesmanProblemLibrary;

namespace SolverLibrary
{
    public class GreedySolver : Solver
    {
        protected override void TraverseNext(ref Path path, ref Path unvisitedCities, ref int calculationAmount)
        {
            var shortestDistance = double.MaxValue;
            City nextCity = null;
            var index = 0;

            // Path is empty, so start at first city
            if (path.Count == 0)
            {
                // Next city is the first city in the queue
                nextCity = unvisitedCities.First();

                // Add next city
                path.Add(nextCity);
                unvisitedCities.Remove(nextCity);
            }
            // Make first edge
            else if (path.Count == 1)
            {
                foreach (var unvisitedCity in unvisitedCities)
                {
                    var distance = Distance.Between(unvisitedCity, path.Last());

                    // Increase calculation counter
                    calculationAmount++;

                    if (distance < shortestDistance)
                    {
                        shortestDistance = distance;
                        nextCity = unvisitedCity;
                    }
                }

                // Add next city
                path.Add(nextCity);
                unvisitedCities.Remove(nextCity);
            }
            // Search for closest city to an edge in the path
            else
            {
                foreach (var unvisitedCity in unvisitedCities)
                {

                    for (int i = 0; i < path.Count - 1; i++)
                    {
                        var firstEdgeCity = path[i];
                        var secondEdgeCity = path[i + 1];

                        // Distance from unvisited city to the line segment between first edge city and second edge city
                        var distance = Distance.Between(unvisitedCity, firstEdgeCity, secondEdgeCity);

                        // Increment calculation counter
                        calculationAmount++;

                        if (distance < shortestDistance)
                        {
                            // Update shortest distance and the next city
                            shortestDistance = distance;
                            nextCity = unvisitedCity;
                            index = path.IndexOf(secondEdgeCity);
                        }
                    }
                }

                System.Diagnostics.Debug.Write($"{nextCity.Name} -> ");

                // Add next city
                path.Insert(index, nextCity);
                unvisitedCities.Remove(nextCity);
            }
        }
    }
}

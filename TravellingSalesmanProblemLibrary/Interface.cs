using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TravellingSalesmanProblemLibrary
{
    public class Interface
    {
        public static bool TryTextToTsp(string fileName, out Tsp tsp)
        {
            tsp = new Tsp();

            try
            {
                // Throw error if the file does not exist.
                if (!File.Exists(fileName))
                {
                    throw new Exception();
                }

                // Get each line of the file to process it line-by-line
                string[] lines = File.ReadAllLines(fileName);

                var isCityLine = false;

                foreach (var line in lines)
                {
                    // If after NODE_COORD_SECTION, process lines as cities
                    if (isCityLine)
                    {
                        // Split the line into three parts based on spaces
                        string[] sublines = line.Split(' ');

                        // Get city values from the line
                        var number = Convert.ToInt32(sublines[0]);
                        var x = Convert.ToDouble(sublines[1]);
                        var y = Convert.ToDouble(sublines[2]);

                        // Add the city to the TSP
                        tsp.Cities.Add(new City(number, new Coordinate(x, y)));
                    }
                    else
                    {
                        // Split the line into it's type and content based on the semi-colon
                        var lineType = line.Split(':')[0];
                        var lineContent = line.Contains(":") ? line.Split(':')[1].Trim() : string.Empty;

                        // Set TSP values according to the line type
                        switch (lineType)
                        {
                            case "NAME":
                                tsp.Name = lineContent;
                                break;
                            case "TYPE":
                                // Do nothing with type
                                break;
                            case "COMMENT":
                                tsp.Comments.Add(lineContent);
                                break;
                            case "DIMENSION":
                                tsp.Dimension = Convert.ToInt32(lineContent);
                                break;
                            case "EDGE_WEIGHT_TYPE":
                                tsp.EdgeWeight = lineContent;
                                break;
                            case "NODE_COORD_SECTION":
                                isCityLine = true; // after NODE_COORD_SECTION, lines are the values of cities
                                break;
                            default:
                                break;
                        }
                    }
                }

                return true;
            }
            catch
            {
                tsp = null;
                return false;
            }
        }
    }
}

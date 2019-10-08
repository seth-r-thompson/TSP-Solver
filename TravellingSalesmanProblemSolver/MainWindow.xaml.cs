using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TravellingSalesmanProblemLibrary;
using SolverLibrary;
using System.ComponentModel;

namespace TravellingSalesmanProblemSolver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private const int GREEDY_DRAW_DELAY = 500;

        public MainWindow()
        {
            this.InitializeComponent();
        }

        #region Event Handlers

        /// <summary>
        /// Handles the solve button click.
        /// </summary>
        private void OnSolveClick(object sender, RoutedEventArgs e)
        {
            // Gets the file the user entered in the textbox
            var file = problemEntry.Text.Trim();

            // Process the TSP
            this.ProcessTsp(file);
        }

        #endregion

        #region Business Logic

        /// <summary>
        /// Processes a TSP and reports errors.
        /// </summary>
        /// <param name="file">File name of TSP being processed.</param>
        private void ProcessTsp(string file)
        {
            System.Diagnostics.Debug.WriteLine($"Loading TSP at path {file} ...\n");

            // Try convert text to TSP
            var success = Interface.TryTextToTsp(file, out var tsp);

            // If conversion was a success
            if (success)
            {
                // Print TSP info to debug console
                tsp.Print();

                // Update name of problem on graph
                this.problemName.Text = tsp.Name;

                // Create solver and event handlers
                Solver solver = null;
                ProgressChangedEventHandler updateHandler = null;
                RunWorkerCompletedEventHandler completionHandler = null;
                int drawDelay = 0;

                // Set solvers and event handlers based on the chosen setting
                if (bruteForceRadioButton.IsChecked == true)
                {
                    solver = new BruteForceSolver();

                    MessageBox.Show("Solution method not yet implemented after refactoring.");
                    return;
                }
                else if (bfsRadioButton.IsChecked == true || dfsRadioButton.IsChecked == true)
                {
                    var goal = Convert.ToInt32(searchGoal.Text);

                    if (bfsRadioButton.IsChecked == true)
                    {
                        solver = new BfsSolver();
                    }
                    else
                    {
                        solver = new DfsSolver();
                    }

                    MessageBox.Show("Solution method not yet implemented after refactoring.");
                    return;
                }
                else if (greedyRadioButton.IsChecked == true)
                {
                    solver = new GreedySolver();
                    updateHandler = GreedyProgressChanged;
                    completionHandler = GreedyCompletion;
                    drawDelay = GREEDY_DRAW_DELAY;
                }
                else if (geneticRadioButton.IsChecked == true)
                {
                    solver = new GeneticSolver();
                    updateHandler = GeneticProgressChanged;
                    completionHandler = GreedyCompletion;
                    drawDelay = GREEDY_DRAW_DELAY;
                }
                else
                {
                    MessageBox.Show("No solution method was selected.");
                    return;
                }

                if (progressCheckBox.IsChecked == true)
                {
                    // Update continously
                    solver.ProgressChanged += updateHandler;
                }
                else
                {
                    // Update only at the end
                    solver.RunWorkerCompleted += completionHandler;
                }

                var workerArgs = new Tuple<Tsp, int>(tsp, progressCheckBox.IsChecked == true ? drawDelay : 0);

                // Run solver and draw output
                solver.RunWorkerAsync(workerArgs);
            }
            // If conversion failed
            else
            {
                MessageBox.Show($"Could not convert TSP from text file: \n\n{file}");
            }
        }

        #endregion

        #region Async Methods

        private void GreedyProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                this.completionPercentage.Text = $"{e.ProgressPercentage}%";
                this.DrawGraphOf(e.UserState as TspSolution);
            }
        }

        private void GreedyCompletion(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                this.completionPercentage.Text = "100%";
                this.DrawGraphOf(e.Result as TspSolution);
            }
        }

        private void GeneticProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.UserState != null)
            {
                this.completionPercentage.Text = $"{e.ProgressPercentage}%";
                this.DrawGraphOf(e.UserState as TspSolution);
            }
        }

        private void GeneticCompletion(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result != null)
            {
                this.completionPercentage.Text = "100%";
                this.DrawGraphOf(e.Result as TspSolution);
            }
        }

        #endregion

        #region Graphics
        private const int CITY_CIRCLE_DIAMETER = 10;
        private const int CITY_COORDINATE_MODIFIER = 3;
        private const int CITY_LABEL_BUFFER = 10;

        private void DrawGraphOf(TspSolution tspSolution, bool graphShouldConnect = true, TravellingSalesmanProblemLibrary.Path cities = null)
        {
            // Clear the graph of previous solution
            this.cityMap.Children.Clear();

            // Set the list of all cities to the shortest path if it is null
            cities = cities == null ? new TravellingSalesmanProblemLibrary.Path(tspSolution.ShortestPath) : cities;

            // Plot each city on the graph
            foreach (var city in cities)
            {
                // Dimensions are modified to space them out on the graph
                var cityX = city.Coordinate.X * CITY_COORDINATE_MODIFIER;
                var cityY = city.Coordinate.Y * CITY_COORDINATE_MODIFIER;

                // Create a visual representation of the point on the graph
                var cityEllipse = new Ellipse()
                {
                    Height = CITY_CIRCLE_DIAMETER,
                    Width = CITY_CIRCLE_DIAMETER,
                    Stroke = Brushes.Black,
                    Margin = new Thickness(cityX - CITY_CIRCLE_DIAMETER / 2, cityY - CITY_CIRCLE_DIAMETER / 2, 0, 0)
                };

                // Add ellipse to graph
                this.cityMap.Children.Add(cityEllipse);

                // Create a label for the point
                var cityTextBlock = new TextBlock()
                {
                    Text = Convert.ToString(city.Name)
                };

                // Set label coordinates
                Canvas.SetLeft(cityTextBlock, cityX + CITY_LABEL_BUFFER);
                Canvas.SetTop(cityTextBlock, cityY - CITY_LABEL_BUFFER);

                // Add label to graph
                this.cityMap.Children.Add(cityTextBlock);
            }

            // Create points list for graphing
            var points = new TravellingSalesmanProblemLibrary.Path(tspSolution.ShortestPath);

            // Re-add the first city to connect the graph
            if (graphShouldConnect)
            {
                points.Add(points[0]);
            }

            // Add lines to the graph
            for (int i = 0; i < points.Count - 1; i++)
            {
                // Create a line between the current point and the next
                var line = new Line()
                {
                    X1 = points[i].Coordinate.X * CITY_COORDINATE_MODIFIER,
                    Y1 = points[i].Coordinate.Y * CITY_COORDINATE_MODIFIER,
                    X2 = points[i + 1].Coordinate.X * CITY_COORDINATE_MODIFIER,
                    Y2 = points[i + 1].Coordinate.Y * CITY_COORDINATE_MODIFIER,
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 2
                };

                // Add line to graph
                this.cityMap.Children.Add(line);
            }

            // Update all label text
            var shortestPathLength = graphShouldConnect ? tspSolution.ShortestPath.CompleteLength : tspSolution.ShortestPath.Length;

            this.shortestPath.Text = $"{Math.Round(shortestPathLength, 4)} m";
            this.runtime.Text = $"{Math.Round(tspSolution.Runtime * 1000, 4)} ms";
            this.calculationCount.Text = $"{tspSolution.CalculationAmount}";
        }
        #endregion
    }
}

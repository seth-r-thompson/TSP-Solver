using System;
using System.Collections.Generic;
using System.ComponentModel;
using TravellingSalesmanProblemLibrary;

namespace SolverLibrary
{
    public abstract class Solver : BackgroundWorker
    {
        public Solver() : base()
        {
            this.WorkerReportsProgress = true;
            this.DoWork += SolverDoWork;
        }

        /// <summary>
        /// Perform calculation for next path iteration, reporting progress.
        /// Progress report must be handled in class creating a member of this class.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void SolverDoWork(object sender, DoWorkEventArgs e)
        {
            // Start calculating runtime
            var startTime = DateTime.Now;

            // Thread sleeps allow user time to visually process each update
            var threadSleepLength = (e.Argument as Tuple<Tsp, int>).Item2;
            var sleepCount = 0;

            var tsp = (e.Argument as Tuple<Tsp, int>).Item1;
            var tspSolution = new TspSolution();

            var shortestPath = new Path();
            var unvisitedCities = new Path(tsp.Cities);
            var calculationAmount = 0;

            while (shortestPath.Count != tsp.Cities.Count)
            {
                // Get next stage of path
                this.TraverseNext(ref shortestPath, ref unvisitedCities, ref calculationAmount);

                // Update solution
                tspSolution.ShortestPath = shortestPath;
                tspSolution.Runtime = (DateTime.Now - startTime).TotalSeconds - ((double)threadSleepLength / 1000) * sleepCount;
                tspSolution.CalculationAmount = calculationAmount;

                // Calculate completion percentage
                var progressPercentage = Convert.ToInt32((shortestPath.Count * 100) / tsp.Cities.Count);

                // Report progress
                (sender as BackgroundWorker).ReportProgress(progressPercentage, tspSolution);

                // Sleep to allow user to visually process update
                if (threadSleepLength > 0)
                {
                    sleepCount++;
                    System.Threading.Thread.Sleep(threadSleepLength);
                }
            }

            // Return solution
            e.Result = tspSolution;
        }

        protected abstract void TraverseNext(ref Path path, ref Path unvisitedCities, ref int calculationAmount);
    }
}

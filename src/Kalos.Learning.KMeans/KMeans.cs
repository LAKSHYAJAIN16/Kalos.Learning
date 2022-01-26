using System;
using System.Text;
using System.Collections.Generic;
using Kalos.Learning.Datasets;
using Kalos.Learning.Linq.Graphs;

namespace Kalos.Learning.KMeans
{
    public class KMeans
    {
        /// <summary>
        /// The Naive Bayes Magic Number
        /// </summary>
        public const int NaiveBayesNum = 10;

        /// <summary>
        /// Cluster Data
        /// </summary>
        public static int[] Cluster(double[][] rawData, int numClusters)
        {
            //Normalize Data, so that Large Values don't crash the system
            double[][] data = Normalized(rawData); // so large values don't dominate

            //Some Bools
            bool changed = true;
            bool success = true;

            //Initialize Clusters
            int[] clustering = InitClustering(data.Length, numClusters);

            //Allocate Means
            double[][] means = Allocate(numClusters, data[0].Length);

            //Just a sanity check
            int maxCount = data.Length * NaiveBayesNum;

            //Loop through
            int ct = 0;
            while (changed == true && success == true && ct < maxCount)
            {
                ct++;
                success = UpdateMeans(data, clustering, means);
                changed = UpdateClustering(data, clustering, means);
            }

            return clustering;
        }

        /// <summary>
        /// Normalizes Data so that All numbers are given equal importance
        /// </summary>
        private static double[][] Normalized(double[][] rawData)
        {
            //Define Result
            double[][] result = new double[rawData.Length][];

            //Copy Array
            for (int i = 0; i < rawData.Length; i++){
                result[i] = new double[rawData[i].Length];
                Array.Copy(rawData[i], result[i], rawData[i].Length);
            }

            for (int j = 0; j < result[0].Length; j++)
            {
                //Define ColumnSum
                double colSum = 0.0;

                //Add up
                for (int i = 0; i < result.Length; i++)
                    colSum += result[i][j];

                //Calculate Estimate or Mean
                double mean = colSum / result.Length;

                //Define ANOTHER sum
                double sum = 0.0;

                //Increment that using the Naive Bayes Formula
                for (int i = 0; i < result.Length; i++)
                    sum += (result[i][j] - mean) * (result[i][j] - mean);

                //ANOTHER MEAN HAHA
                double sd = sum / result.Length;

                //The Llyods formula.
                for (int i = 0; i < result.Length; i++)
                    result[i][j] = (result[i][j] - mean) / sd;
            }
            return result;
        }

        /// <summary>
        /// Initializes Country
        /// </summary>
        private static int[] InitClustering(int numTuples, int numClusters)
        {
            //Initialize Random
            Random random = new Random();
            int[] clustering = new int[numTuples];

            //Make Sure each cluster has at least one container
            for (int i = 0; i < numClusters; i++)
                clustering[i] = i;

            //Other Assignments are random
            for (int i = numClusters; i < clustering.Length; i++)
                clustering[i] = random.Next(0, numClusters);

            return clustering;
        }

        /// <summary>
        /// Allocate cluster to column
        /// </summary>
        /// <returns></returns>
        private static double[][] Allocate(int numClusters, int numColumns)
        {
            //Make Matrix
            double[][] result = new double[numClusters][];

            //Loop through
            for (int k = 0; k < numClusters; k++)
                result[k] = new double[numColumns];

            return result;
        }

        /// <summary>
        /// Updates the Means for the Model
        /// </summary>
        /// <returns></returns>
        private static bool UpdateMeans(double[][] data, int[] clustering, double[][] means)
        {
            //Number of Clusters
            int numClusters = means.Length;
            int[] clusterCounts = new int[numClusters];

            //Increment Data
            for (int i = 0; i < data.Length; i++){
                int cluster = clustering[i];
                clusterCounts[cluster]++;
            }

            //If this is true, it means really bad, since our cluster is bad. We return false
            for (int k = 0; k < numClusters; k++)
                if (clusterCounts[k] == 0)
                    return false;

            //Update, so that we can use the matrix
            for (int k = 0; k < means.Length; k++)
                for (int j = 0; j < means[k].Length; j++)
                    means[k][j] = 0.0;


            //Accumulate Sum
            for (int i = 0; i < data.Length; i++)
            {
                int cluster = clustering[i];
                for (int j = 0; j < data[i].Length; j++)
                    means[cluster][j] += data[i][j];
            }

            //Just another MEAN HAHA
            for (int k = 0; k < means.Length; k++)
                for (int j = 0; j < means[k].Length; j++)
                    means[k][j] /= clusterCounts[k];

            return true;
        }

        /// <summary>
        /// Updates the Clusters
        /// </summary>
        private static bool UpdateClustering(double[][] data, int[] clustering, double[][] means)
        {
            //Again the number of clusters
            int numClusters = means.Length;

            //Bool to see if we already found a nice result
            bool changed = false;

            //Proposed Result
            int[] newClustering = new int[clustering.Length];

            //COPY THAT BOIZ
            Array.Copy(clustering, newClustering, clustering.Length);

            //Distance array
            double[] distances = new double[numClusters];

            //Loop through
            for (int i = 0; i < data.Length; i++)
            {
                //Compute distance from each cluster
                for (int k = 0; k < numClusters; k++)
                    distances[k] = FindBayesDistance(data[i], means[k]);

                //Find closest Mean ID
                int newClusterID = CalculateMinID(distances);
                if (newClusterID != newClustering[i]){
                    changed = true;
                    newClustering[i] = newClusterID; // update
                }
            }

            //No change so bail and don't update clustering[][]
            if (changed == false)
                return false;

            //Check proposed clustering[] cluster counts
            int[] clusterCounts = new int[numClusters];
            for (int i = 0; i < data.Length; i++){
                int cluster = newClustering[i];
                clusterCounts[cluster]++;
            }

            //Again, check if the cluster was 0
            for (int k = 0; k < numClusters; k++)
                if (clusterCounts[k] == 0)
                    return false;

            //Copy the ARRAY NERD
            Array.Copy(newClustering, clustering, newClustering.Length);
            return true; 
        }

        /// <summary>
        /// Calculates The Naive Bayes Distance
        /// </summary>
        private static double FindBayesDistance(double[] tuple, double[] mean)
        {
            // Euclidean distance between two vectors for UpdateClustering()
            double sumSquaredDiffs = 0.0;

            //Increment
            for (int j = 0; j < tuple.Length; j++)
                sumSquaredDiffs += Math.Pow((tuple[j] - mean[j]), 2);

            //Return The SquareRoot of that
            return Math.Sqrt(sumSquaredDiffs);
        }

        /// <summary>
        /// Calculates MinID
        /// </summary>
        private static int CalculateMinID(double[] distances)
        {
            //Calculates Index of smallest value in array
            int indexOfMin = 0;
            double smallDist = distances[0];
            for (int k = 0; k < distances.Length; k++)
            {
                if (distances[k] < smallDist){
                    smallDist = distances[k];
                    indexOfMin = k;
                }
            }
            return indexOfMin;
        }

        /// <summary>
        /// Shows the Data(For COnvinience XD)
        /// </summary>
        public static void ShowData(double[][] data, int decimals, bool indices, bool newLine)
        {
            for (int i = 0; i < data.Length; i++)
            {
                if (indices) Console.Write(i.ToString().PadLeft(3) + " ");
                for (int j = 0; j < data[i].Length; j++)
                {
                    if (data[i][j] >= 0.0) Console.Write(" ");
                    Console.Write(data[i][j].ToString("F" + decimals) + " ");
                }
                Console.WriteLine("");
            }
            if (newLine) Console.WriteLine("");
        }

        /// <summary>
        /// Shows a COlumn Vector
        /// </summary>
        public static void ShowVector(int[] vector, bool newLine)
        {
            for (int i = 0; i < vector.Length; i++)
                Console.Write(vector[i] + " ");
            if (newLine) Console.WriteLine("\n");
        }

        /// <summary>
        /// Shows the Clusters
        /// </summary>
        public static void ShowClustered(double[][] data, int[] clustering, int numClusters, int decimals)
        {
            for (int k = 0; k < numClusters; k++)
            {
                Console.WriteLine("===================");
                for (int i = 0; i < data.Length; i++)
                {
                    int clusterID = clustering[i];
                    if (clusterID != k) continue;
                    Console.Write(i.ToString().PadLeft(3) + " ");
                    for (int j = 0; j < data[i].Length; j++)
                    {
                        if (data[i][j] >= 0.0) Console.Write(" ");
                        Console.Write(data[i][j].ToString("F" + decimals) + " ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("===================");
            }
        }
    }
}
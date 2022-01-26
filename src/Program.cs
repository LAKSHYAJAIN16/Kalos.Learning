using System;
using System.Text;
using System.Linq;
using System.Collections.Generic;
using Kalos.Learning.Models;
using Kalos.Learning.Layers;
using Kalos.Learning.Datasets;
using Kalos.Learning.Linq.Helpers;
using Kalos.Learning.Linq.Graphs;
using Kalos.Learning.KNN;
using Kalos.Learning.SVM;
using Kalos.Learning.KMeans;
using Kalos.Learning.Perceptron;
using Kalos.Learning.NaiveBayes;
using Kalos.Learning.DecisionTree;
using Kalos.Learning.LinearRegression;
using Kalos.Learning.LogisticRegression;
using Kalos.Learning.Layers.ActivationLayers;

namespace Kalos.Learning.Tests
{
    public class Program
    {
        static void Main(string[] args)
        {
            //Iris_KMeans();
            //XOR_KNN();
            //Bulgarian_Economy_LinearRegression();
            // Iris_LogisticRegression();
            //Text_NaiveBayes();
            //NeuralNetwork();
            //Iris_Perceptron();
            //Iris_SVM();
            //Iris_DecisonTree();
        }

        static void Bulgarian_Economy_LinearRegression()
        {
            (double[] xValues, double[] yValues) = DataSets.BulgarianPopulationLR();

            //Graph
            Console.WriteLine("------------------");
            Console.WriteLine("Y-Axis Progression");
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(GraphService.PlotLineGraph(yValues, new Options {
                AxisLabelLeftMargin = 4,
                AxisLabelRightMargin = 0,
                Height = 10,
                Fill = '.',
                AxisLabelFormat = "0.00",
            }));
            Console.WriteLine("\n------------------");
            Console.WriteLine("X-Axis Progression");
            Console.OutputEncoding = Encoding.UTF8;
            Console.WriteLine(GraphService.PlotLineGraph(xValues, new Options
            {
                AxisLabelLeftMargin = 4,
                AxisLabelRightMargin = 0,
                Height = 10,
                Fill = '.',
                AxisLabelFormat = "0.00",
            }));

            LinearResult res = LinearRegressor.LinearRegregate(xValues, yValues);
            Console.WriteLine("\n-------------------\nEnter Year");
            int year = int.Parse(Console.ReadLine());
            Console.Beep();
            Console.WriteLine($"The Economy in {year} will be {Math.Round(Math.Abs(res.Prediction(year)))} billion");
            Console.WriteLine("\nPress Any Key to Exit");
            Console.ReadKey();
        }

        static void Iris_KMeans()
        {
            //DATA BOIZ
            Console.OutputEncoding = Encoding.UTF8;
            double[][] rawData = DataSets.IrisKMeans();

            //SHow Data in Tabular form
            Console.WriteLine("Raw unclustered data:\n");
            Console.WriteLine("    Value1 Value2");
            Console.WriteLine("-------------------");
            KMeans.ShowData(rawData, 1, true, true);

            //Show Value 1 in graph style
            Console.WriteLine("Value 1 Linear Progression");
            Console.WriteLine("-------------------");
            List<double> stuff = new List<double>();
            foreach (double[] it in rawData)
            {
                stuff.Add(it[0]);
            }
            Console.WriteLine(GraphService.PlotLineGraph(stuff.ToArray(), new Options
            {
                AxisLabelLeftMargin = 4,
                AxisLabelRightMargin = 0,
                Height = 10,
                Fill = '.',
                AxisLabelFormat = "0.00",
            }));

            //Show Value 2 in graph style
            stuff.Clear();
            Console.WriteLine("\nValue 2 Linear Progression");
            Console.WriteLine("-------------------");
            foreach (double[] it in rawData)
            {
                stuff.Add(it[1]);
            }
            Console.WriteLine(GraphService.PlotLineGraph(stuff.ToArray(), new Options
            {
                AxisLabelLeftMargin = 4,
                AxisLabelRightMargin = 0,
                Height = 10,
                Fill = '.',
                AxisLabelFormat = "0.00",
            }));

            //Get Clusters input
            Console.WriteLine("\n\nHow Many Clusters do you want the Computer to Form?");
            int numClusters = int.Parse(Console.ReadLine());

            //CLUSTER
            int[] clustering = KMeans.Cluster(rawData, numClusters);

            //SHow Internal Cluster
            Console.WriteLine("Final clustering in internal form:\n");
            KMeans.ShowVector(clustering, true);

            //Show Clustered Data
            Console.WriteLine("Raw data by cluster:\n");
            KMeans.ShowClustered(rawData, clustering, numClusters, 1);

            Console.WriteLine("\nPress Any Key To Exit\n");
            Console.ReadLine();
        }

        static void XOR_KNN()
        {
            KNN idk = new KNN();
            idk.TrainingData = DataSets.XORTable2Input();

            Console.WriteLine("Enter 1st Value");
            double val1 = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter 2nd Value");
            double val2 = double.Parse(Console.ReadLine());

            double[] input = new double[]{
                val1,
                val2
            };
            Console.WriteLine($"\n-----------------------------------------" +
                $"\nThe Prediction By the Computer is {idk.Predict(input, 2, 1)}");
            Console.ReadLine();
        }

        static void Iris_LogisticRegression()
        {
            LogisticRegressor f = new LogisticRegressor();
            var data = DataSets.Iris_2DLogisticRegression();

            Console.WriteLine("\nEnter Epochs");
            int epochs = int.Parse(Console.ReadLine());
            f.Fit(data.Item1, data.Item2, 0.01, epochs, 0.2, step_for_verbose: 100);
            
            Console.WriteLine("Enter 1st Value");
            double d1 = double.Parse(Console.ReadLine());
            Console.WriteLine("Enter 2ndValue");
            double d2 = double.Parse(Console.ReadLine());

            var prediction = f.ComputeOutput(new double[] { d1, d2 });
            Console.WriteLine($"\nThe Computer Predicts that the output is {Math.Round(prediction)}");
            Console.ReadLine();
        }

        static void Text_NaiveBayes()
        {
            BayesClassifier classifier = new BayesClassifier();
            (string[], string[]) thing = DataSets.Sentiment_NaiveBayes();

            classifier.Fit(thing.Item1, thing.Item2);
            Console.WriteLine("Enter Input to the System");
            string inputaa = Console.ReadLine();
            var prediction = classifier.Predict(inputaa);
            Console.WriteLine($"The Computer Predicts the Input as " +
                $"{prediction.Item2}, with a " +
                $"{(100 - Math.Abs(prediction.Item1)).ToString("F2")}% Confidence");

            Console.ReadKey();
        }

        static void NeuralNetwork()
        {
            Sequential model = new Sequential(3);
            model.Add(model.CreateNeuralLayer(3, new Tanh(), new Sum()));
            model.Add(model.CreateNeuralLayer(3, new Tanh(), new Sum()));
            model.Add(model.CreateNeuralLayer(3, new Tanh(), new Sum()));
            model.Add(model.CreateNeuralLayer(1, new Tanh(), new Sum()));

            double[][] outputs = new double[][]{
                new double[] { 1 },
                new double[] { 0 },
                new double[] { 1 },
                new double[] { 0 },
                new double[] { 1 },
                new double[] { 0 },
                new double[] { 1 },
                new double[] { 0 },
            };

            double[][] inputs = new double[][]
            {
                new double[] { 18 , 1 , 1 },
                new double[] { 31 , 1 , 0},
                new double[] { 41 , 0 , 0},
                new double[] { 8 , 0 , 1 },
                new double[] { 22 , 1 , 1 },
                new double[] { 28 , 1 , 0},
                new double[] { 52 , 0 , 0},
                new double[] { 60 , 1 , 0 }
            };

            model.Fit(inputs, outputs, 300, 0.56, true, true, 50);
            LinqHelper.PrintDoubleArray(model.Predict(new double[] { 8, 0, 0 }).ToArray());
            Console.ReadKey();
        }

        static void Iris_Perceptron()
        {
            (double[][], double[]) data = DataSets.Iris_Perceptrion2D();
            Perceptron per = new Perceptron();
            per.Fit(data.Item1, data.Item2, 0.1, 100, true, true, 10, true);
            Console.WriteLine("\n---------------------------------");
            Console.WriteLine("Training Complete");

            Console.WriteLine("Enter 1st Value");
            double val1 = double.Parse(Console.ReadLine());

            Console.WriteLine("Enter 2nd Value");
            double val2 = double.Parse(Console.ReadLine());

            Console.WriteLine($"Prediction : {per.Predict(new double[] { val1, val2 })}");
            Console.WriteLine("Press any Key to Exit");
            Console.ReadKey();
        }

        static void Iris_SVM()
        {
            (double[][], int[]) fdata = DataSets.Iris_2DLogisticRegression();
            SVM svm = new SVM(fdata.Item1, fdata.Item2, KernelType.RBF);
            double[][] data = new double[][]
            {
                new double[]{0.2,0.3},
                new double[]{0.5,0.6}
            };
            int[] result = svm.Predict(data);
            result.ToList().ForEach(x => Console.WriteLine(x));
            Console.ReadKey();
        }

        static void Iris_DecisonTree()
        {
            var data = DataSets.Iris_DecisionTree();
            double[][] dataX = data.Item1;
            int[] dataY = data.Item2;
            DecisionTreeClassifier dt = new DecisionTreeClassifier(7, 3);
            dt.BuildTree(dataX, dataY);
            dt.Show();

            double acc = dt.Accuracy(dataX, dataY);
            Console.WriteLine("Classification accuracy = " + acc.ToString("F4"));

            Console.WriteLine("\nEnter Sepal Length");
            double v1 = double.Parse(Console.ReadLine());

            Console.WriteLine("\nEnter Sepal Width");
            double v2 = double.Parse(Console.ReadLine());

            Console.WriteLine("\nEnter Petal Length");
            double v3 = double.Parse(Console.ReadLine());

            Console.WriteLine("\nEnter Petal Width");
            double v4 = double.Parse(Console.ReadLine());

            string[] output = new string[] { "I.Setosa", "I.Versicolor", "I.Virginica" };
            Console.WriteLine($"Prediction is {output[dt.Predict(new double[] { v1, v2, v3, v4 },false)]}");
            Console.ReadLine();
        }
    }
}
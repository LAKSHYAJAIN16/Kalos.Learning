using System;
using System.Linq;
using System.Collections.Generic;

namespace Kalos.Learning.Perceptron
{
    public class Perceptron
    {
        internal int override_num = 40;
        internal Random random { get; set; }
        internal double[] weights { get; set; }

        public Perceptron(int? seed = null)
        {
            //Initialize Random
            if (seed.HasValue){
                random = new Random(seed.Value);
            }
            else if (!seed.HasValue){
                random = new Random();
            }
        }

        public double[] Fit(double[][] X_Data, double[] Y_Data, double learning_Rate = 1, int epochs = 4, bool verbose = true, bool beep_console = true, int steps_for_verbose = 10, bool save_to_object = true)
        {
            //Define Length of Training Data
            int Length_Items = X_Data.Length;
            int Length_Features = X_Data[0].Length;

            //Initialize Weights
            double[] weights = new double[Length_Features + 1];
            double[] accWeights = new double[Length_Features + 1];
            double[] avgWeights = new double[Length_Features + 1];

            //Initialize RANDOM weights
            weights = InitWeights(weights);

            //Initialize Indicies
            int[] indicies = new int[Length_Items];

            //Define some more stuff XD
            int itter = 0;
            int numAccums = 0;
            int number_of_correct_predictions = 0;

            //While loop
            while (itter < epochs)
            {
                //Shuffle Data
                indicies = Shuffle(indicies);

                //Loop for each index
                foreach (int index in indicies)
                {
                    //Get Predicted Output
                    double output = Predict(X_Data[index], weights);

                    //Get Real Output
                    double real_output = Y_Data[index];

                    //Check whether we got it right
                    if (output != real_output)
                    {
                        //Get Delta
                        double delta = real_output - output;

                        //Correct Weights
                        for (int j = 0; j < Length_Features; j++)
                            weights[j] = weights[j] + (learning_Rate * delta * X_Data[index][j]);

                        //Correct Bias
                        weights[Length_Features] = weights[Length_Features] + (learning_Rate * delta * 1);
                    }

                    //Increment acc weights
                    for (int k = 0; k < weights.Length; k++)
                        accWeights[k] += accWeights[k];

                    //Increment Number of Accums
                    numAccums++;
                }

                //Increment Epochs
                itter++;

                //Calculate Accuracy
                double accuracy = Accuracy(X_Data, Y_Data, weights);

                //If it is 100, increment number of Correct Predictions
                if (accuracy == 100) number_of_correct_predictions++;

                //If we have predicted the correct output greater than the override number, halt loop
                if (number_of_correct_predictions > override_num){
                    if (verbose) Console.WriteLine("\nThe Computer Determined that it needed no more training because it was already so smart and trained its model with 100% accuracy");
                    break;
                }

                //Verbose Stuff
                if (verbose && itter % steps_for_verbose == 0){
                    Console.WriteLine($"Epoch {itter} completed with success.Accuracy {accuracy}%");
                    if (beep_console) Console.Beep();
                }
            }

            //Get Average Weights
            for (int l = 0; l < weights.Length; l++)
                avgWeights[l] = avgWeights[l] / numAccums;

            if (save_to_object) this.weights = avgWeights;
            return avgWeights;
        }

        public double Predict(double[] x, double[] weights)
        {
            //Some Matrix Multiplication
            double z = 0;
            for (int i = 0; i < x.Length; ++i)
                z += x[i] * weights[i];

            //Add the Bias
            z += weights[weights.Length - 1];

            //Return The Output 
            if (z < 0.0) return 0;
            else return 1;
        }
        public double Predict(double[] x)
        {
            //Some Matrix Multiplication
            double z = 0;
            for (int i = 0; i < x.Length; ++i)
                z += x[i] * weights[i];

            //Add the Bias
            z += weights[weights.Length - 1];

            //Return The Output 
            if (z < 0.0) return 0;
            else return 1;
        }

        internal int[] Shuffle(int[] indices)
        {
            int n = indices.Length;
            for (int i = 0; i < n; ++i){
                int ri = random.Next(i, n);
                int tmp = indices[ri];
                indices[ri] = indices[i];
                indices[i] = tmp;
            }

            return indices;
        }

        internal double[] InitWeights(double[] weights){
            List<double> weightsL = weights.ToList();
            weightsL.ForEach(x => x = random.NextDouble()) ;
            return weightsL.ToArray();
        }

        internal double Accuracy(double[][] xData, double[] yData, double[] weights)
        {
            int numCorrect = 0; int numWrong = 0;
            int N = xData.Length;
            for (int i = 0; i < N; i++)
            {
                double[] x = xData[i];
                double target = yData[i];
                double computed = Predict(x, weights);
                if (target == 1 && computed == 1 || target == -1 && computed == -1){
                    numCorrect++;
                }
                else{
                    numWrong++;
                }
            }
            return (1 - (1.0 * numCorrect / (numCorrect + numWrong))) * 100;
        }
    }
}

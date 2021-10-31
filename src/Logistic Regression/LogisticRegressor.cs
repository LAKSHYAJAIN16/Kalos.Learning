using System;

namespace Kalos.Learning.Utils.LogisticRegression
{
    /// <summary>
    /// Main Class For Logistic Regression
    /// </summary>
    public class LogisticRegressor
    {
        private Random random { get; set; }
        private int numFeatures { get; set; }
        private double[] alphas { get; set; }
        private double[][] TrainingX { get; set; }
        private double sigma { get; set; }
        public LogisticRegressor()
        {
            random = new Random();
            numFeatures = 2;
        }
        public LogisticRegressor(int seed)
        {
            random = new Random(seed);
            numFeatures = 2;
        }
        public LogisticRegressor(int seed, int numFeatures)
        {
            random = new Random(seed);
            this.numFeatures = numFeatures;
        }

        /// <summary>
        /// Method to Fit the Network
        /// </summary>
        /// <param name="Train_X">The X Input to the System</param>
        /// <param name="Train_Y">The Y Output to the System</param>
        /// <param name="learning_rate">How Much Weightage the system will give to the error</param>
        /// <param name="epochs">How Many Itterations the proccess will run for</param>
        /// <param name="sigma">The Sigma for the System. This is directly proportional to the values of the data. Like for example, a census for a country will have higher sigma than a flower length dataset</param>
        /// <param name="verbose">If we want to Debug the Proccess</param>
        /// <param name="save_values">If We Want to Save the values to the Object</param>
        /// <param name="beep_console">If we want to beep in the console</param>
        /// <param name="step_for_verbose">How Much the Step for visualizing the data should be</param>
        /// <returns></returns>
        public double[] Fit(double[][] Train_X, int[] Train_Y, double learning_rate = 0.01, int epochs = 500, double sigma = 1, bool verbose = true, bool save_values = true, bool beep_console = true, int step_for_verbose = 5)
        {
            //Get Length of Training Data
            int len = Train_X.Length;

            //Compute Alphas. 1 extra for Bias
            double[] alphas = new double[len + 1];

            //Create Kernel Matrix
            double[][] kernels = new double[len][];
            for (int i = 0; i < len; i++)
                kernels[i] = new double[len];

            //Fill in Matrix with Kernel Values
            for (int i = 0; i < len; i++)
                for (int j = 0; j < len; j++)
                    kernels[i][j] = kernels[j][i] = Kernel(Train_X[i], Train_X[j], sigma);

            //Create Indicees aray
            int[] indices = new int[len];  // process data in random order
            for (int i = 0; i < len; i++)
                indices[i] = i;

            //Main Training Loop
            for (int iter = 0; iter < epochs; iter++)
            {
                //Shuffle indicies
                Shuffle(indices);

                //Loop for each train item
                foreach (int t_item in indices)
                {
                    //Get Output
                    double output = Predict(Train_X[t_item], alphas, sigma, Train_X);

                    //Get REAL output
                    double real_output = Train_Y[t_item];

                    //Update each alpha by the error
                    for (int j = 0; j < alphas.Length - 1; ++j){
                        double derivative = real_output - output;
                        alphas[j] += learning_rate * derivative * kernels[t_item][j];
                    }

                    //Update the Bias using dummy input
                    alphas[alphas.Length - 1] += learning_rate * (real_output - output) * 1;
                }

                //Some Debug Stuff :L
                if (iter % step_for_verbose == 0 && verbose) {
                    double err = Error(Train_X, Train_Y, Train_X, alphas, sigma);
                    Console.WriteLine(" Epoch = " + iter.ToString() +
                      "  Error = " + err.ToString("F3") + 
                      "  Accuracy = " + ((1-err) * 100).ToString("F2") +"%");
                    if (beep_console) Console.Beep();
                }
            }

            if (save_values){
                this.TrainingX = Train_X;
                this.sigma = sigma;
                this.alphas = alphas;
            }

            return alphas;
        }
        
        public double Predict(double[] value, double[] alphas, double sigma, double[][] trainX)
        {
            //Get Length
            int n = trainX.Length;

            //Define sum
            double sum = 0.0;

            //Loop through and use the alphas
            for (int i = 0; i < n; ++i)
                sum += (alphas[i] * Kernel(value, trainX[i], sigma));

            //Add the Bias
            sum += alphas[n];
            return LogSig(sum);
        }

        public double ComputeOutput(double[] value)
        {
            //Get Length
            int n = TrainingX.Length;

            //Define sum
            double sum = 0.0;

            //Loop through and use the alphas
            for (int i = 0; i < n; ++i)
                sum += (alphas[i] * Kernel(value, TrainingX[i], sigma));

            //Add the Bias
            sum += alphas[n];
            return LogSig(sum);
        }

        public double Error(double[][] dataX, int[] dataY, double[][] trainX, double[] alphas, double sigma){
            int n = dataX.Length;
            double sum = 0.0;  // sum of squarede error
            for (int i = 0; i < n; ++i)
            {
                double p = Predict(dataX[i], alphas, sigma, trainX);  // [0.0, 1.0]
                int y = dataY[i];  // target 0 or 1
                sum += (p - y) * (p - y);
            }
            return sum / n;
        }

        public double LogSig(double x)
        {
            if (x < -10.0)
                return 0.0;
            else if (x > 10.0)
                return 1.0;
            else
                return 1.0 / (1.0 + Math.Exp(-x));
        }

        public double Kernel(double[] v1, double[] v2, double sigma)
        {
            //Define Num
            double num = 0.0;

            //Increment using Kernel Formula
            for (int i = 0; i < v1.Length; ++i)
                num += (v1[i] - v2[i]) * (v1[i] - v2[i]);

            //Create denominator
            double denom = 2.0 * sigma * sigma;

            //Find value and return it to e
            double z = num / denom;
            return Math.Exp(-z);
        }

        public void Shuffle(int[] indices)
        {
            for (int i = 0; i < indices.Length; ++i)
            {
                int ri = random.Next(i, indices.Length);
                int tmp = indices[i];
                indices[i] = indices[ri];
                indices[ri] = tmp;
            }
        }
    }
}

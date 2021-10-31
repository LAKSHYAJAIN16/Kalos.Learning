namespace Kalos.Learning.Datasets
{
    public class DataSets
    {
        public static (double[], double[]) BulgarianPopulationLR()
        {
            var xValues = new double[]
                  {
                                  1990, 1991, 1992, 1993, 1994, 1995, 1996, 1997, 1998, 1999, 2000, 2001, 2002, 2003, 2004,
                                  2005, 2006, 2007, 2008, 2009, 2069
                  };
            var yValues = new double[]
                              {
                                      20, 4, 5, 7, 9, 12, 15, 18, 21, 25, 26, 12, 14, 17, 19, 21, 26, 30,
                                      31, 25, 20
                              };

            return (xValues, yValues);
        }

        public static double[][] IrisKMeans()
        {
            double[][] rawData = new double[40][];
            rawData[0] = new double[] { 65.0, 220.0 };
            rawData[1] = new double[] { 73.0, 160.0 };
            rawData[2] = new double[] { 59.0, 110.0 };
            rawData[3] = new double[] { 61.0, 120.0 };
            rawData[4] = new double[] { 75.0, 150.0 };
            rawData[5] = new double[] { 67.0, 240.0 };
            rawData[6] = new double[] { 68.0, 230.0 };
            rawData[7] = new double[] { 70.0, 220.0 };
            rawData[8] = new double[] { 62.0, 130.0 };
            rawData[9] = new double[] { 66.0, 210.0 };
            rawData[10] = new double[] { 77.0, 190.0 };
            rawData[11] = new double[] { 75.0, 180.0 };
            rawData[12] = new double[] { 74.0, 170.0 };
            rawData[13] = new double[] { 70.0, 210.0 };
            rawData[14] = new double[] { 61.0, 110.0 };
            rawData[15] = new double[] { 58.0, 100.0 };
            rawData[16] = new double[] { 66.0, 230.0 };
            rawData[17] = new double[] { 59.0, 120.0 };
            rawData[18] = new double[] { 68.0, 210.0 };
            rawData[19] = new double[] { 61.0, 130.0 };
            rawData[20] = new double[] { 65.0, 220.0 };
            rawData[21] = new double[] { 82.0, 160.0 };
            rawData[22] = new double[] { 78.0, 110.0 };
            rawData[23] = new double[] { 61.0, 120.0 };
            rawData[24] = new double[] { 75.0, 150.0 };
            rawData[25] = new double[] { 67.0, 240.0 };
            rawData[26] = new double[] { 68.0, 230.0 };
            rawData[27] = new double[] { 70.0, 220.0 };
            rawData[28] = new double[] { 62.0, 130.0 };
            rawData[29] = new double[] { 66.0, 210.0 };
            rawData[30] = new double[] { 77.0, 190.0 };
            rawData[31] = new double[] { 75.0, 180.0 };
            rawData[32] = new double[] { 74.0, 170.0 };
            rawData[33] = new double[] { 70.0, 210.0 };
            rawData[34] = new double[] { 61.0, 110.0 };
            rawData[35] = new double[] { 58.0, 100.0 };
            rawData[36] = new double[] { 52.0, 230.0 };
            rawData[37] = new double[] { 50.0, 120.0 };
            rawData[38] = new double[] { 42.0, 300.0 };
            rawData[39] = new double[] { 69.0, 110.0 };

            return rawData;
        }

        public static double[][] XORTable2Input()
        {
            return new double[][]{
                new double[]{0,0,0},
                new double[]{0,1,1},
                new double[]{1,0,1},
                new double[]{1,1,0}
            };
        }

        public static (double[][], int[]) Iris_2DLogisticRegression()
        {
            // load training data - not lin. sep. logistic regression gives 12/21 = .57 accuracy
            double[][] trainX = new double[21][];
            trainX[0] = new double[] { 0.2, 0.3 };
            trainX[1] = new double[] { 0.1, 0.5 };
            trainX[2] = new double[] { 0.2, 0.7 };
            trainX[3] = new double[] { 0.3, 0.2 };
            trainX[4] = new double[] { 0.3, 0.8 };
            trainX[5] = new double[] { 0.4, 0.2 };
            trainX[6] = new double[] { 0.4, 0.8 };
            trainX[7] = new double[] { 0.5, 0.2 };
            trainX[8] = new double[] { 0.5, 0.8 };
            trainX[9] = new double[] { 0.6, 0.3 };
            trainX[10] = new double[] { 0.7, 0.5 };
            trainX[11] = new double[] { 0.6, 0.7 };
            trainX[12] = new double[] { 0.3, 0.4 };
            trainX[13] = new double[] { 0.3, 0.5 };
            trainX[14] = new double[] { 0.3, 0.6 };
            trainX[15] = new double[] { 0.4, 0.4 };
            trainX[16] = new double[] { 0.4, 0.5 };
            trainX[17] = new double[] { 0.4, 0.6 };
            trainX[18] = new double[] { 0.5, 0.4 };
            trainX[19] = new double[] { 0.5, 0.5 };
            trainX[20] = new double[] { 0.5, 0.6 };

            int[] trainY = new int[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            return (trainX, trainY);
        }

        public static (double[][], double[]) Iris_Perceptrion2D()
        {
            double[][] trainX = new double[21][];
            trainX[0] = new double[] { 0.2, 0.3 };
            trainX[1] = new double[] { 0.1, 0.5 };
            trainX[2] = new double[] { 0.2, 0.7 };
            trainX[3] = new double[] { 0.3, 0.2 };
            trainX[4] = new double[] { 0.3, 0.8 };
            trainX[5] = new double[] { 0.4, 0.2 };
            trainX[6] = new double[] { 0.4, 0.8 };
            trainX[7] = new double[] { 0.5, 0.2 };
            trainX[8] = new double[] { 0.5, 0.8 };
            trainX[9] = new double[] { 0.6, 0.3 };
            trainX[10] = new double[] { 0.7, 0.5 };
            trainX[11] = new double[] { 0.6, 0.7 };
            trainX[12] = new double[] { 0.3, 0.4 };
            trainX[13] = new double[] { 0.3, 0.5 };
            trainX[14] = new double[] { 0.3, 0.6 };
            trainX[15] = new double[] { 0.4, 0.4 };
            trainX[16] = new double[] { 0.4, 0.5 };
            trainX[17] = new double[] { 0.4, 0.6 };
            trainX[18] = new double[] { 0.5, 0.4 };
            trainX[19] = new double[] { 0.5, 0.5 };
            trainX[20] = new double[] { 0.5, 0.6 };

            double[] trainY = new double[21] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 1, 1 };

            return (trainX, trainY);
        }

        public static (string[], string[]) Sentiment_NaiveBayes()
        {
            string[] input = new string[]{
                "lost",
                "won",
                "died",
                "sad",
                "bad",
                "horrible",
                "happy",
                "excited"
            };

            string[] output = new string[]{
                "Happy","Happy","Sad","Sad","Sad","Sad","Happy","Happy"
            };

            return (input, output);
        }

        public static (double[][], int[]) Iris_DecisionTree()
        {
            double[][] dataX = new double[30][];
            dataX[0] = new double[] { 5.1, 3.5, 1.4, 0.2 };  // 0
            dataX[1] = new double[] { 4.9, 3.0, 1.4, 0.2 };
            dataX[2] = new double[] { 4.7, 3.2, 1.3, 0.2 };
            dataX[3] = new double[] { 4.6, 3.1, 1.5, 0.2 };
            dataX[4] = new double[] { 5.0, 3.6, 1.4, 0.2 };
            dataX[5] = new double[] { 5.4, 3.9, 1.7, 0.4 };
            dataX[6] = new double[] { 4.6, 3.4, 1.4, 0.3 };
            dataX[7] = new double[] { 5.0, 3.4, 1.5, 0.2 };
            dataX[8] = new double[] { 4.4, 2.9, 1.4, 0.2 };
            dataX[9] = new double[] { 4.9, 3.1, 1.5, 0.1 };

            dataX[10] = new double[] { 7.0, 3.2, 4.7, 1.4 };  // 1
            dataX[11] = new double[] { 6.4, 3.2, 4.5, 1.5 };
            dataX[12] = new double[] { 6.9, 3.1, 4.9, 1.5 };
            dataX[13] = new double[] { 5.5, 2.3, 4.0, 1.3 };
            dataX[14] = new double[] { 6.5, 2.8, 4.6, 1.5 };
            dataX[15] = new double[] { 5.7, 2.8, 4.5, 1.3 };
            dataX[16] = new double[] { 6.3, 3.3, 4.7, 1.6 };
            dataX[17] = new double[] { 4.9, 2.4, 3.3, 1.0 };
            dataX[18] = new double[] { 6.6, 2.9, 4.6, 1.3 };
            dataX[19] = new double[] { 5.2, 2.7, 3.9, 1.4 };

            dataX[20] = new double[] { 6.3, 3.3, 6.0, 2.5 };   // 2
            dataX[21] = new double[] { 5.8, 2.7, 5.1, 1.9 };
            dataX[22] = new double[] { 7.1, 3.0, 5.9, 2.1 };
            dataX[23] = new double[] { 6.3, 2.9, 5.6, 1.8 };
            dataX[24] = new double[] { 6.5, 3.0, 5.8, 2.2 };
            dataX[25] = new double[] { 7.6, 3.0, 6.6, 2.1 };
            dataX[26] = new double[] { 4.9, 2.5, 4.5, 1.7 };
            dataX[27] = new double[] { 7.3, 2.9, 6.3, 1.8 };
            dataX[28] = new double[] { 6.7, 2.5, 5.8, 1.8 };
            dataX[29] = new double[] { 7.2, 3.6, 6.1, 2.5 };

            int[] dataY = new int[30] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                      1, 1, 1, 1, 1, 1, 1, 1, 1, 1,
                      2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };

            return (dataX, dataY);
        }
    }
}
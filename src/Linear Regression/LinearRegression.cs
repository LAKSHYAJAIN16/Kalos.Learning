using System;
using System.Diagnostics;

namespace Kalos.Learning.Utils.LinearRegression
{
    public class LinearRegressor
    {
        public static LinearResult LinearRegregate(double[] xVals, double[] yVals)
        {
            //Check if the length of the data is the same
            if (xVals.Length != yVals.Length){
                throw new LearningException("Error : Length of Training set is not equal");
            }

            //one more check
            Debug.Assert(xVals.Length == yVals.Length);

            //Get Sum
            double sumX, sumY, sumXsq, sumYsq, sumCodev;
            SumLinearData(xVals, yVals, out sumX, out sumY, out sumXsq, out sumYsq, out sumCodev);

            //Get slope
            double ssX, ssY;
            CalculateSlope(xVals.Length, sumXsq, sumYsq, sumX, sumY, out ssX, out ssY);

            //Get RLine
            double rNum, rDen, sCo;
            CalculateRLine(xVals.Length, sumCodev, sumX, sumY, sumXsq, sumYsq, out rNum, out rDen, out sCo);

            //Calculate Mean
            double xMean, yMean, dbLr;
            CalculateMean(xVals.Length, sumX, sumY, rNum, rDen, out xMean, out yMean, out dbLr);

            //Finally calculate values
            double rSquared = dbLr * dbLr;
            double yIntercept = yMean - ((sCo / ssX) * xMean);
            double slope = sCo / ssX;

            //Return new LinearResult
            return new LinearResult(rSquared, yIntercept, slope);
        }

        public static void SumLinearData(double[] xVals, double[] yVals, out double sumX, out double sumY, out double sumXSq, out double sumYSq, out double sumCodev)
        {
            //Define Values
            sumX = 0; sumY = 0; sumXSq = 0; sumYSq = 0; sumCodev = 0;

            //Add with loop
            for (int i = 0; i < xVals.Length; i++)
            {
                double xN = xVals[i];
                double yN = yVals[i];
                sumCodev += xN * yN;
                sumX += xN;
                sumY += yN;
                sumXSq += Math.Pow(xN, 2);
                sumYSq += Math.Pow(yN, 2);
            }
        }

        public static void CalculateSlope(int dataLength, double sumXsq, double sumYsq, double sumX, double sumY,out double ssX, out double ssY)
        {
            ssX = sumXsq - ((sumX * sumX) / dataLength);
            ssY = sumYsq - ((sumY * sumY) / dataLength);
        }

        public static void CalculateRLine(int dataLength, double sumCodev, double sumX, double sumY, double sumXsq, double sumYsq, out double rNum, out double rDenom, out double sCo)
        {
            rNum = (dataLength * sumCodev) - (sumX * sumY);
            rDenom = (dataLength * sumXsq - (sumX * sumX)) * (dataLength * sumYsq - (sumY * sumY));
            sCo = sumCodev - ((sumX * sumY) / dataLength);
        }

        public static void CalculateMean(int dataLength, double sumX, double sumY, double rNum, double rDen, out double xMean, out double yMean, out double dBlr)
        {
            xMean = sumX / dataLength;
            yMean = sumY / dataLength;
            dBlr = rNum / Math.Sqrt(rDen);
        }
    }

    public struct LinearResult
    {
        public double rSquared;
        public double yIntercept;
        public double slope;

        public LinearResult(double rSq, double yInte, double slope)
        {
            this.rSquared = rSq;
            this.yIntercept = yInte;
            this.slope = slope;
        }

        public double Prediction(double input)
        {
            return (input * slope) + yIntercept;
        }
    }
}

namespace Kalos.Learning
{
    public class LearningException : Exception
    {
        public LearningException() { }
        public LearningException(string message) : base(message) { }
        public LearningException(string message, Exception inner) : base(message, inner) { }
        protected LearningException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
using System;

namespace Kalos.Learning.Utils.KNN
{
    public class KNN
    {
        public int K { get; set; }
        public double[][] TrainingData { get; set; }

        public VotingData Predict(double[] input, int numberOfUniqueOutputs, int K, bool verbose = true)
        {
            //Assign K
            this.K = K;

            //Get Length of Training Data
            int length = TrainingData.Length;

            //Create An Array of points
            Point[] points = new Point[length];

            //Loop
            for (int i = 0; i < length; i++)
            {
                //Make Point
                Point current = new Point();

                //Get Distance
                double dist = GetDistance(input, TrainingData[i]);

                //Assign ID and Distance
                current.idx = i;
                current.dist = dist;

                //Re-input the Point
                points[i] = current;
            }

            //Sort
            Array.Sort(points);
            
            //Some Debug :L
            if (verbose){
                Console.WriteLine("Nearest   Distance   Class");
                Console.WriteLine("==========================");
                for (int i = 0; i < this.K; i++)
                {
                    int c = (int)TrainingData[points[i].idx][2];
                    string dist = points[i].dist.ToString("F3");
                    Console.WriteLine("( " + TrainingData[points[i].idx][0] +
                      "," + TrainingData[points[i].idx][1] + " )  :  " +
                      dist + "        " + c);
                }
            }

            return Vote(points, numberOfUniqueOutputs);
        }

        public double GetDistance(double[] input, double[] data)
        {
            double sum = 0.0;
            for (int i = 0; i < input.Length; ++i)
                sum += (input[i] - data[i]) * (input[i] - data[i]);
            return Math.Sqrt(sum);
        }

        public VotingData Vote(Point[] points, int numberofuniqueClasses)
        {
            //Create Votes Array
            int[] votes = new int[numberofuniqueClasses];

            //Loop through
            for (int i = 0; i < K; i++)
            {   
                //Get Index
                int idx = points[i].idx;
                
                //Get Class
                int c = (int)TrainingData[idx][2];

                //Increment
                votes[c]++;
            }

            //Define Most Votes
            int mostVotes = 0;

            //Define CLASS with most votes
            int classWithMostVotes = 0;
            for (int j = 0; j < numberofuniqueClasses; j++)
            {
                if (votes[j] > mostVotes){
                    mostVotes = votes[j];
                    classWithMostVotes = j;
                }
            }

            //Get Percentage
            float t = Convert.ToSingle(votes.Length);
            float mV = Convert.ToSingle(mostVotes);
            float percent = (mV / t) * 100;

            return new VotingData(classWithMostVotes, mostVotes, percent);
        }
    }

    public class Point : IComparable<Point>
    {
        public int idx;  // Index of a training item
        public double dist;  // To unknown
                             // Need to sort these to find k closest
        public int CompareTo(Point other)
        {
            if (this.dist < other.dist) return -1;
            else if (this.dist > other.dist) return +1;
            else return 0;
        }
    }

    public class VotingData : IFormattable
    {
        public int classwithmostvotes;
        public int mostvotes;
        public float percentage;

        public VotingData(int class_withMost, int most_votes, float percentage)
        {
            this.classwithmostvotes = class_withMost;
            this.mostvotes = most_votes;
            this.percentage = percentage;
        }
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return $"\n\nPrediction : {classwithmostvotes}\nMostVotes : {mostvotes}\nPercentage : {percentage}%";
        }
    }
}
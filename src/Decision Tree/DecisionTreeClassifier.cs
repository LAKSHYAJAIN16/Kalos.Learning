using System;
using System.Collections.Generic;

namespace Kalos.Learning.Utils.DecisionTree
{
    internal class DecisionTreeClassifier
    {
        public int numNodes;
        public int numClasses;
        public List<Node> tree;

        public DecisionTreeClassifier(int numNodes, int numClasses)
        {
            this.numNodes = numNodes;
            this.numClasses = numClasses;
            this.tree = new List<Node>();
            for (int i = 0; i < numNodes; ++i)
                this.tree.Add(new Node());
        }

        public void BuildTree(double[][] dataX, int[] dataY)
        {
            // prep the list and the root node
            int n = dataX.Length;

            List<int> allRows = new List<int>();
            for (int i = 0; i < n; ++i)
                allRows.Add(i);

            this.tree[0].rows = new List<int>(allRows);

            for (int i = 0; i < this.numNodes; ++i)
            {
                this.tree[i].nodeID = i;

                SplitInfo si = GetSplitInfo(dataX, dataY, this.tree[i].rows, this.numClasses);
                tree[i].splitCol = si.splitCol;
                tree[i].splitVal = si.splitVal;

                tree[i].classCounts = ComputeClassCts(dataY, this.tree[i].rows, this.numClasses);
                tree[i].predictedClass = ArgMax(tree[i].classCounts);

                int leftChild = (2 * i) + 1;
                int rightChild = (2 * i) + 2;

                if (leftChild < numNodes)
                    tree[leftChild].rows = new List<int>(si.lessRows);
                if (rightChild < numNodes)
                    tree[rightChild].rows = new List<int>(si.greaterRows);
            }
        } 

        public void Show(){
            for (int i = 0; i < this.numNodes; ++i)
                ShowNode(i);
        }

        public void ShowNode(int nodeID)
        {
            Console.WriteLine("\n==========");
            Console.WriteLine("Node ID: " + this.tree[nodeID].nodeID);
            Console.WriteLine("Node split column: " + this.tree[nodeID].splitCol);
            Console.WriteLine("Node split value: " + this.tree[nodeID].splitVal.ToString("F2"));
            Console.Write("Node class counts: ");
            for (int c = 0; c < this.numClasses; ++c)
                Console.Write(this.tree[nodeID].classCounts[c] + "  ");
            Console.WriteLine("");
            Console.WriteLine("Node predicted class: " + ArgMax(this.tree[nodeID].classCounts));
            Console.WriteLine("==========");
        }

        public int Predict(double[] x, bool verbose = true)
        {
            int result = -1;
            int currNodeID = 0;
            string rule = "IF (*)";
            while (true)
            {
                if (tree[currNodeID].rows.Count == 0)
                    break;

                if (verbose) Console.WriteLine("\ncurr node id = " + currNodeID);

                int sc = this.tree[currNodeID].splitCol;
                double sv = this.tree[currNodeID].splitVal;
                double v = x[sc];
                if (verbose) Console.WriteLine("Comparing " + sv + " in column " + sc + " with " + v);

                if (v < sv)
                {
                    if (verbose) Console.WriteLine("attempting move left");

                    currNodeID = (2 * currNodeID) + 1;
                    if (currNodeID >= this.tree.Count)
                        break;
                    result = this.tree[currNodeID].predictedClass;
                    rule += " AND (column " + sc + " < " + sv + ")";
                }
                else
                {
                    if (verbose) Console.WriteLine("attempting move right");
                    currNodeID = (2 * currNodeID) + 2;
                    if (currNodeID >= this.tree.Count)
                        break;
                    result = this.tree[currNodeID].predictedClass;
                    rule += " AND (column " + sc + " >= " + sv + ")";
                }

                if (verbose) Console.WriteLine("new node id = " + currNodeID);
            }

            if (verbose) Console.WriteLine("\n" + rule);
            if (verbose) Console.WriteLine("Predcited class = " + result);

            return result;
        }

        public double Accuracy(double[][] dataX, int[] dataY)
        {
            int numCorrect = 0;
            int numWrong = 0;
            for (int i = 0; i < dataX.Length; ++i)
            {
                int predicted = Predict(dataX[i], verbose: false);
                int actual = dataY[i];
                if (predicted == actual)
                    ++numCorrect;
                else
                    ++numWrong;
            }
            return (numCorrect * 1.0) / (numWrong + numCorrect);
        }

        private static SplitInfo GetSplitInfo(double[][] dataX, int[] dataY, List<int> rows, int numClasses)
        {
            // given a set of parent rows, find the col and value, and less-rows and greater-rows of
            // partition that gives lowest resulting mean impurity or entropy
            int nCols = dataX[0].Length;
            SplitInfo result = new SplitInfo();

            int bestSplitCol = 0;
            double bestSplitVal = 0.0;
            double bestImpurity = double.MaxValue;
            List<int> bestLessRows = new List<int>();
            List<int> bestGreaterRows = new List<int>();  // actually >=

            foreach (int i in rows)  // traverse the specified rows of the ref data
            {
                for (int j = 0; j < nCols; ++j)
                {
                    double splitVal = dataX[i][j];  // curr value to evaluate as possible best split value
                    List<int> lessRows = new List<int>();
                    List<int> greaterRows = new List<int>();
                    foreach (int ii in rows)  // walk down curr column
                    {
                        if (dataX[ii][j] < splitVal)
                            lessRows.Add(ii);
                        else
                            greaterRows.Add(ii);
                    } // ii

                    double meanImp = MeanImpurity(dataY, lessRows, greaterRows, numClasses);

                    if (meanImp < bestImpurity)
                    {
                        bestImpurity = meanImp;
                        bestSplitCol = j;
                        bestSplitVal = splitVal;

                        bestLessRows = new List<int>(lessRows);
                        bestGreaterRows = new List<int>(greaterRows);
                    }

                }
            }

            result.splitCol = bestSplitCol;
            result.splitVal = bestSplitVal;
            result.lessRows = new List<int>(bestLessRows);
            result.greaterRows = new List<int>(bestGreaterRows);

            return result;
        }

        private static double Impurity(int[] dataY, List<int> rows, int numClasses)
        {
            if (rows.Count == 0) return 0.0;

            int[] counts = new int[numClasses];
            double[] probs = new double[numClasses];
            for (int i = 0; i < rows.Count; ++i)
            {
                int idx = rows[i];
                int c = dataY[idx];  
                ++counts[c];
            }

            for (int c = 0; c < numClasses; ++c)
                if (counts[c] == 0) probs[c] = 0.0;
                else probs[c] = (counts[c] * 1.0) / rows.Count;

            double sum = 0.0;
            for (int c = 0; c < numClasses; ++c)
                sum += probs[c] * probs[c];

            return 1.0 - sum;
        }

        private static double MeanImpurity(int[] dataY, List<int> rows1, List<int> rows2, int numClasses)
        {
            if (rows1.Count == 0 && rows2.Count == 0)
                return 0.0;

            double imp1 = Impurity(dataY, rows1, numClasses);
            double imp2 = Impurity(dataY, rows2, numClasses);
            int count1 = rows1.Count;
            int count2 = rows2.Count;
            double wt1 = (count1 * 1.0) / (count1 + count2);
            double wt2 = (count2 * 1.0) / (count1 + count2);
            double result = (wt1 * imp1) + (wt2 * imp2);
            return result;
        }

        private static int[] ComputeClassCts(int[] dataY, List<int> rows, int numClasses)
        {
            int[] result = new int[numClasses];
            foreach (int i in rows)
            {
                int c = dataY[i];
                ++result[c];
            }
            return result;
        }

        private static int ArgMax(int[] classCts)
        {
            int largeCt = 0;
            int largeIndx = 0;
            for (int i = 0; i < classCts.Length; ++i)
            {
                if (classCts[i] > largeCt)
                {
                    largeCt = classCts[i];
                    largeIndx = i;
                }
            }
            return largeIndx;
        }

        public class Node
        {
            public int nodeID;
            public List<int> rows;
            public int splitCol;
            public double splitVal;
            public int[] classCounts;
            public int predictedClass;
        }

        public class SplitInfo
        {
            public int splitCol;
            public double splitVal;
            public List<int> lessRows;
            public List<int> greaterRows;
        }
    }
}
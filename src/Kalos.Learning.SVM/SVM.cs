using System;

namespace Kalos.Learning.SVM
{
    public class SVM
    {
        //Input Values
        private double[][] X_input_values;

        //Y_Values
        private int[] Y_output_values;

        //Weights
        public double[] Weights;

        //Coefficient
        public double coefficient;

        public SVM(double[][] TrainingData, int[] Traininglabels, KernelType tp)
        {
            if (TrainingData.Length > 0 && TrainingData.Length == Traininglabels.Length)
            {
                this.X_input_values = TrainingData;
                this.Y_output_values = Traininglabels;


                SimplifiedSMO SSMO = new SimplifiedSMO(X_input_values, Y_output_values, 1e1, tp);
                Weights = InternalEvaluate(X_input_values, Y_output_values, SSMO.alphas);
                coefficient = SSMO.coefficient;
            }
            else
            {
                if (TrainingData.Length <= 0){
                    Console.WriteLine("Length of Traning Data is wrong");
                }
                if (Traininglabels.Length != TrainingData.Length){
                    Console.WriteLine("data length of labels and inputvalues are not the same");
                }
            }
        }

        public SVM(double[] Previous_W_vector, double Previous_b)
        {
            this.Weights = Previous_W_vector;
            this.coefficient = Previous_b;
        }

        public int[] Predict(double[][] input_values)
        {
            int[] result = new int[input_values.Length];
            for (int i = 0; i < input_values.Length; i++)
            {
                double res = 0;
                for (int j = 0; j < input_values[i].Length; j++){
                    res += Weights[j] * input_values[i][j];
                }
                res += coefficient;
                if (res >= 1){
                    result[i] = 1;
                }
                else if (res <= -1){
                    result[i] = -1;
                }
                else{
                    result[i] = 0;
                }
            }
            return result;
        }

        public int[] Predict(double[] input_values)
        {
            double[][] data = new double[][]{
                input_values
            };
            return Predict(input_values);
        }

        private double[] InternalEvaluate(double[][] input_values, int[] labels, double[] alphas)
        {
            double[] W = new double[input_values[0].Length];
            for (int i = 0; i < input_values.Length; i++)
            {
                for (int j = 0; j < input_values[i].Length; j++){
                    W[j] += alphas[i] * labels[i] * input_values[i][j];
                }
            }
            return W;
        }
    }
    public enum KernelType
    {
        Linear,
        RBF,
        Polynominal,
        InnerProduct
    }

    internal class SimplifiedSMO
    {
        //Alphas
        public double[] alphas;

        //Input
        private double[][] input;

        //Output
        private int[] y_output;

        //Coefficient
        public double coefficient;

        //TOlerance
        private double tolerance = 0.00001;

        //Overide Value
        private int overide_value = 100;

        //The CO, commanding var
        private double Theta = 1;

        //Type
        internal KernelType type;

        public SimplifiedSMO(double[][] input_values, int[] labels, double C, KernelType Kerneltype)
        {
            this.input = input_values;
            this.alphas = new double[input_values.Length];
            this.type = Kerneltype;
            this.y_output = labels;
            this.coefficient = 0;
            int passes = 0;
            this.Theta = C;
            Random rdn = new Random();

            while (passes < overide_value)
            {
                int num_changed_alpha = 0;
                for (int i = 0; i < input.Length; i++)
                {
                    double Ei = E_function(input[i], i);
                    if ((Ei * y_output[i] < -tolerance && alphas[i] < C) || (Ei * y_output[i] > tolerance && alphas[i] > 0))
                    {
                        int j = 0;
                        j = rdn.Next(0, input.Length - 1);
                        //Console.WriteLine(j);
                        while (j == i) j = rdn.Next(0, input.Length - 1);
                        double Ej = E_function(input[j], j);
                        //Console.WriteLine("Ej:{0}", Ej);
                        double ai = alphas[i], aj = alphas[j];
                        double L, H;
                        if (y_output[i] != y_output[j])
                        {
                            L = 0 > aj - ai ? 0 : aj - ai;
                            H = C < C + aj - ai ? C : C + aj - ai;
                        }
                        else
                        {
                            L = 0 > ai + aj - C ? 0 : ai + aj - C;
                            H = C < ai + aj ? C : ai + aj;
                        }
                        if (L == H)
                        {
                            //next i

                        }
                        else
                        {
                            double tau = 2 * InnerProduct(input[i], input[j]) - InnerProduct(input[i], input[i]) - InnerProduct(input[j], input[j]);
                            if (tau >= 0)
                            {
                                //next i
                            }
                            else
                            {
                                alphas[j] = alphas[j] - (E_function(input[i], i) - E_function(input[j], j)) * y_output[j] / tau;
                                if (alphas[j] > H) alphas[j] = H;
                                else if (alphas[j] < H && alphas[j] > L) { }
                                else alphas[j] = L;
                                if (Math.Abs(alphas[j] - aj) < tolerance)
                                {
                                    //next i
                                }
                                else
                                {
                                    alphas[i] = alphas[i] + y_output[i] * y_output[j] * (aj - alphas[j]);
                                    double b1 = coefficient - E_function(input[i], i) - y_output[i] * (alphas[i] - ai) * InnerProduct(input[i], input[i]) - y_output[j] * (alphas[j] - aj) * InnerProduct(input[i], input[j]);
                                    double b2 = coefficient - E_function(input[j], j) - y_output[i] * (alphas[i] - ai) * InnerProduct(input[i], input[j]) - y_output[j] * (alphas[j] - aj) * InnerProduct(input[j], input[j]);
                                    if (alphas[i] > 0 && alphas[i] < C) coefficient = b1;
                                    else if (alphas[j] > 0 && alphas[j] < C) coefficient = b2;
                                    else coefficient = (b1 + b2) / 2;
                                    num_changed_alpha += 1;
                                }
                            }
                        }
                    }
                }
                if (num_changed_alpha == 0)
                {
                    passes += 1;
                    // Console.WriteLine("PASSES:{0}", passes);
                }
                else passes = 0;
            }
        }

        private static double InnerProduct(double[] u, double[] v)
        {
            double res = 0.0;
            for (int i = 0; i < u.Length; i++)
            {
                res += u[i] * v[i];
            }
            return res;
        }

        private static double LinearKernel(double[] u, double[] v)
        {
            return InnerProduct(u, v);
        }

        private static double RBF_Kernel(double[] u, double[] v, double Theta)
        {
            double res = 0.0;
            for (int i = 0; i < u.Length; i++)
            {
                res += (u[i] - v[i]) * (u[i] - v[i]);
            }
            return Math.Pow(Math.E, -res * Theta);
        }

        private double E_function(double[] RowEle, int index_Label)
        {
            double res = 0.0;
            for (int i = 0; i < alphas.Length; i++)
            {
                if (type == KernelType.InnerProduct || type == KernelType.Linear)
                {
                    res += alphas[i] * y_output[i] * LinearKernel(input[i], RowEle);
                }
                else if (type == KernelType.RBF)
                {
                    res += alphas[i] * y_output[i] * RBF_Kernel(input[i], RowEle, 0.5);
                }
            }
            return res + coefficient - y_output[index_Label];
        }
    }
}
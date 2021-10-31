using System;
using System.Collections.Generic;

namespace Kalos.Learning.Preproccessing
{
    public class PreProccessingModule
    {
        public static float[][] StringToFloat(string[][] values)
        {
            //Declare Directory
            Dictionary<string, float> Directory = new Dictionary<string, float>();

            //Declare Output Array
            float[][] Output = new float[values.Length][];

            //Callback val
            float call_buffer = 0f;

            //Loop through
            for (int i = 0; i < values.Length; i++)
            {
                string[] valueCol = values[i];
                for (int j = 0; j < valueCol.Length; j++)
                {
                    string value = valueCol[j];
                    if (Directory.TryGetValue(value, out float storedValue)){
                        Output[i][j] = storedValue;
                    }
                    else{
                        Output[i][j] = call_buffer;
                        call_buffer += 1f;
                    }
                }
            }

            return Output;
        }

        public static int[][] StringToInt(string[][] values)
        {
            //Declare Directory
            Dictionary<string, int> Directory = new Dictionary<string, int>();

            //Declare Output Array
            int[][] Output = new int[values.Length][];

            //Callback val
            int call_buffer = 1;

            //Loop through
            for (int i = 0; i < values.Length; i++)
            {
                string[] valueCol = values[i];
                for (int j = 0; j < valueCol.Length; j++)
                {
                    string value = valueCol[j];
                    if (Directory.TryGetValue(value, out int storedValue))
                    {
                        Output[i][j] = storedValue;
                    }
                    else
                    {
                        Output[i][j] = call_buffer;
                        call_buffer += 1;
                    }
                }
            }

            return Output;
        }

        public static ValueCollection StringToValueCollection(string[][] values)
        {
            try
            {
                //Length
                int norm_length = values.Length;
                int nested_length = values[0].Length;

                //Declare Directory
                Dictionary<string, float> Directory = new Dictionary<string, float>();

                //Declare Output Array
                float[][] Output = new float[values.Length][];

                //Callback val
                float call_buffer = 1;

                //Loop through
                for (int i = 0; i < norm_length; i++)
                {
                    Output[i] = new float[nested_length];
                    for (int j = 0; j < nested_length; j++)
                    {
                        string value = values[i][j];
                        if (Directory.ContainsKey(value))
                        {
                            Output[i][j] = Directory[value];
                        }
                        else
                        {
                            Output[i][j] = call_buffer;
                            Directory.Add(value, call_buffer);
                            call_buffer += 1;
                        }
                    }
                }

                return new ValueCollection()
                {
                    Values = Output,
                    Labels = Directory
                };
            }


            catch(IndexOutOfRangeException e){
                throw new LearningException("Error : Length of all the input is not same", e);
            }
        }
    }

    [Serializable]
    public class ValueCollection
    {
        public float[][] Values { get; set; }
        public Dictionary<string,float> Labels { get;set; }
    }
}


namespace Kalos.Learning.Linq.Activation
{

    //Activation types
    public enum ActivationType
    {
        Tanh,
        ReLu,
        LazyReLu,
        Sigmoid,
        SoftMax,
        Binary,
        Sqrt,
        Identity
    }

    //Activation Module
    public struct Activation
    {
        public static float ActivateValue(ActivationType activation, float finalValue)
        {
            float activatedValue = 0f;

            //Tanh activation
            if (activation == ActivationType.Tanh)
                activatedValue = (float)Math.Tanh((double)finalValue);

            //ReLu activation
            else if (activation == ActivationType.ReLu)
                activatedValue = Math.Abs(finalValue);

            //LazyRelu Activation
            else if (activation == ActivationType.LazyReLu)
                activatedValue = finalValue - (finalValue * 2);

            //Identity Activation
            else if (activation == ActivationType.Identity)
                activatedValue = finalValue;

            //Sigmoid activation
            else if (activation == ActivationType.Sigmoid)
                activatedValue = (float)(1 / (1 + Math.Pow(Math.E, (double)-finalValue)));

            //Softmax activation
            else if (activation == ActivationType.SoftMax)
                activatedValue = (float)Math.Tanh(Math.E * (double)finalValue);

            //Binary activation
            else if (activation == ActivationType.Binary)
                activatedValue = (float)Math.Round(finalValue);

            //Sqrt activation
            else if (activation == ActivationType.Sqrt)
                activatedValue = (float)Math.Sqrt((double)finalValue);

            if (activatedValue == 0f)
                activatedValue = 0.1f;

            return activatedValue;
        }
    }
}

namespace Kalos.Learning.Linq.Helpers
{
    public struct LinqHelper
    {
        public static void PrintFloatArray(float[] input)
        {
            string str = "";
            foreach (float item in input)
            {
                str += item;
                str += " ";
            }

            Console.WriteLine(str);
        }

        public static void PrintDoubleArray(double[] input)
        {
            string str = "";
            foreach (double item in input)
            {
                str += item;
                str += " ";
            }

            Console.WriteLine(str);
        }

        public static void PrintCommaArray(float[,] input)
        {
            string str = "";
            foreach (float item in input)
            {
                str += item;
                str += " ";
            }

            Console.WriteLine(str);
        }
        public static void PrintNestedFloatArray(float[][] input)
        {
            string str = "";
            foreach (float[] idk in input){
                str += "{";
                foreach (float item in idk){
                    str += item;
                    str += ',';
                    str += " ";
                }
                str += "}";
            }

            Console.WriteLine(str);
        }
        public static void PrintDictionary(Dictionary<string,float> input)
        {
            string str = "";
            foreach (KeyValuePair<string,float> item in input)
            {
                str += item.Key;
                str += " : ";
                str += item.Value;
                str += ", ";
            }
            Console.WriteLine(str);
        }
    }
}
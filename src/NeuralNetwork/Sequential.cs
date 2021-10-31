using System;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;
using Kalos.Learning.Layers;
using Kalos.Learning.Layers.ActivationLayers;
using Kalos.Learning.Linq.Interfaces;
using Kalos.Learning.Neurons;

namespace Kalos.Learning.Models
{
    public class Sequential
    {
        public List<KLayer> Layers;

        public Sequential(List<KLayer> Layers){
            this.Layers = Layers;
        }
        public Sequential(int input_number){
            this.Layers = new List<KLayer>();
            CreateInputLayer(input_number);
        }

        public void Add(KLayer layer)
        {
            if (Layers.Any()){
                var LastLayer = Layers.Last();
                layer.ConnectLayers(LastLayer);
            }

            Layers.Add(layer);
        }

        private void CreateInputLayer(int numberOfInputNeurons)
        {
            KLayer inputLayer = CreateNeuralLayer(numberOfInputNeurons, new Tanh(), new Sum());
            inputLayer.Neurons.ForEach(x => x.AddInputSynapse(0));
            Add(inputLayer);
        }

        public KLayer CreateNeuralLayer(int number, KActivationFunction activation , KInputFunction input)
        {
            Dense layer = new Dense(number,activation);

            for (int i = 0; i < number; i++){
                var neuron = new Neuron(activation, input);
                layer.Neurons.Add(neuron);
            }

            return layer;
        }

        public KLayer CreateNeuralLayer(LayerTypes type, int number)
        {
            if (type == LayerTypes.Dense){
                return CreateNeuralLayer(number, new Tanh(), new Sum());
            }

            return new Dense(5,new Tanh());
        }

        public void Fit(double[][] inputs, double[][] outputs, int epochs=500, double learning_rate = 0.01, bool verbose = true, bool beep_console = true, int steps_verbose = 10)
        {
            //Define Total Error
            double error = 0;

            Stopwatch stopwatch = Stopwatch.StartNew();
            //Loop through each epoch
            for (int i = 0; i < epochs; i++)
            {
                Stopwatch e = Stopwatch.StartNew();
                for (int j = 0; j < inputs.GetLength(0); j++)
                {
                    //Push Inputs
                    PushInputValues(inputs[j]);

                    //Get Output Array
                    List<double> outputsFor = new List<double>();

                    //Cycle through and get predicted output
                    Layers.Last().Neurons.ForEach(x =>{
                        outputsFor.Add(x.CalculateOutput());
                    });

                    //Calculate Error by summing errors on all output neurons
                    error = CalculateTotalError(outputsFor, j, outputs);

                    //Back Propogate Hidden Layers
                    BackwardsPropogateHiddenLayers(learning_rate);
                    BackwardsPropogateOutputLayers(j, outputs, learning_rate);
                }

                e.Stop();
                if (verbose && i % steps_verbose == 0){
                    Console.WriteLine($"Epoch {i} completed with success. Time taken per step = {e.ElapsedTicks}");
                    if (beep_console) Console.Beep();
                }
            }
            stopwatch.Stop();
            Console.WriteLine($"Training Complete\nTarget Time : {stopwatch.ElapsedTicks}\n");
        }

        public List<double> Predict(double[] input)
        {
            PushInputValues(input);
            List<double> return_values = new List<double>();

            Layers.Last().Neurons.ForEach(neurons =>{
                return_values.Add(neurons.CalculateOutput());
            });

            return return_values;
        }

        public void PushInputValues(double[] inputs){
            Layers.First().Neurons.ForEach(x => x.PushValueOnInput(inputs[Layers.First().Neurons.IndexOf(x)]));
        }

        private double CalculateTotalError(List<double> outputs, int row, double[][] expected)
        {
            //Define Error
            double totalError = 0;

            //Loop through and use mean squared error
            outputs.ForEach(output =>
            {
                double error = Math.Pow(output - expected[row][outputs.IndexOf(output)], 2);
                totalError += error;
            });

            return totalError;
        }

        private void BackwardsPropogateHiddenLayers(double learning_rate)
        {
            for (int k = Layers.Count - 2; k > 0; k--)
            {
                Layers[k].Neurons.ForEach(neuron =>
                {
                    neuron.Inputs.ForEach(connection =>
                    {
                        //Get Output
                        double output = neuron.CalculateOutput();
                        
                        //Get Net Input
                        double netInput = connection.GetOutput();

                        //Define Partial Sum
                        double sumPartial = 0;

                        //Loop through
                        Layers[k + 1].Neurons.ForEach(outputNeuron =>
                        {
                            outputNeuron.Inputs.Where(i => i.IsFromNeuron(neuron.Id)).ToList().ForEach(outConnection =>
                            {
                                sumPartial += outConnection.PreviousWeight * outputNeuron.PreviousPartialDerivate;
                            });
                        });

                        double delta = -1 * netInput * sumPartial * output * (1 - output);
                        connection.UpdateWeight(learning_rate, delta);
                    });
                });
            }
        }

        private void BackwardsPropogateOutputLayers(int row, double[][] expected, double learning_rate)
        {
            Layers.Last().Neurons.ForEach(neuron =>
            {
                neuron.Inputs.ForEach(connection =>
                {
                    double output = neuron.CalculateOutput();
                    double netInput = connection.GetOutput();
                    double expectedOutput = expected[row][Layers.Last().Neurons.IndexOf(neuron)];
                    double node_delta = (expectedOutput - output) * output * (1 - output);
                    double delta = -1 * netInput * node_delta;
                    connection.UpdateWeight(learning_rate, delta);
                    neuron.PreviousPartialDerivate = node_delta;
                });
            });
        }
    }
}

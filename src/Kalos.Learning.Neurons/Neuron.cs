using System;
using System.Linq;
using System.Collections.Generic;
using Kalos.Learning.Linq.Interfaces;

namespace Kalos.Learning.Neurons
{
    public class Neuron : KNeuron
    {
        //Reference to our Activation Function
        public KActivationFunction ActivationFunction;

        //Reference to our input Function
        public KInputFunction InputFunction;

        //Input connections of the neuron.
        public List<KSynapse> Inputs { get; set; }

        //Output COnnections of the neuron
        public List<KSynapse> Outputs { get; set; }

        //Our ID
        public Guid Id { get; private set; }

        //Our Past Derivative for Back-Prop
        public double PreviousPartialDerivate { get; set; }

        public Neuron(KActivationFunction activationFunction, KInputFunction inputFunction)
        {
            //Initialize ID
            Id = Guid.NewGuid();
            
            //Initialize Input and Output Lists
            Inputs = new List<KSynapse>();
            Outputs = new List<KSynapse>();

            //Initialize Activation and Input Functions
            ActivationFunction = activationFunction ?? throw new ArgumentNullException(nameof(activationFunction));
            InputFunction = inputFunction ?? throw new ArgumentNullException(nameof(activationFunction));
        }

        public void AddInputNeuron(KNeuron inputNeuron)
        {
            //Create Synapse
            Synapse synapse = new Synapse(inputNeuron, this);

            //Add it
            Inputs.Add(synapse);
            inputNeuron.Outputs.Add(synapse);
        }

        public void AddInputSynapse(double inputValue)
        {
            InputSynapse inputSynapse = new InputSynapse(this, inputValue);
            Inputs.Add(inputSynapse);
        }

        public void AddOutputNeuron(KNeuron outputNeuron)
        {
            var synapse = new Synapse(this,outputNeuron);
            Outputs.Add(synapse);
            outputNeuron.Inputs.Add(synapse);
        }

        public double CalculateOutput()
        {
            return ActivationFunction.FeedForward(InputFunction.CalculateInput(Inputs));
        }

        public void PushValueOnInput(double inputValue)
        {
            ((InputSynapse)Inputs.First()).Output = inputValue;
        }
    }
}

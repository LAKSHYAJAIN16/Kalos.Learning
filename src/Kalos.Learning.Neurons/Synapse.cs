using System;
using Kalos.Learning.Linq.Interfaces;

namespace Kalos.Learning.Neurons
{
    public class Synapse : KSynapse
    {
        internal KNeuron FromNeuron;
        internal KNeuron ToNeuron;

        /// <summary>
        /// Weight of the connection.
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Weight that connection had in previous itteration.
        /// Used in training process.
        /// </summary>
        public double PreviousWeight { get; set; }

        public Synapse(KNeuron fromNeuraon, KNeuron toNeuron, double weight)
        {
            FromNeuron = fromNeuraon;
            ToNeuron = toNeuron;

            Weight = weight;
            PreviousWeight = 0;
        }

        public Synapse(KNeuron fromNeuraon, KNeuron toNeuron)
        {
            FromNeuron = fromNeuraon;
            ToNeuron = toNeuron;

            var tmpRandom = new Random();
            Weight = tmpRandom.NextDouble();
            PreviousWeight = 0;
        }

        /// <summary>
        /// Get output value of the connection.
        /// </summary>
        /// <returns>
        /// Output value of the connection.
        /// </returns>
        public double GetOutput()
        {
            return FromNeuron.CalculateOutput();
        }

        /// <summary>
        /// Checks if Neuron has a certain number as an input neuron.
        /// </summary>
        /// <param name="fromNeuronId">Neuron Id.</param>
        /// <returns>
        /// True – if the neuron is the input of the connection.
        /// False – if the neuron is not the input of the connection. 
        /// </returns>
        public bool IsFromNeuron(Guid fromNeuronId)
        {
            return FromNeuron.Id.Equals(fromNeuronId);
        }

        /// <summary>
        /// Update weight.
        /// </summary>
        /// <param name="learningRate">Chossen learning rate.</param>
        /// <param name="delta">Calculated difference for which weight of the connection needs to be modified.</param>
        public void UpdateWeight(double learningRate, double delta)
        {
            PreviousWeight = Weight;
            Weight += learningRate * delta;
        }
    }
}

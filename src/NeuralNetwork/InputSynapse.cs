using System;
using Kalos.Learning.Linq.Interfaces;

namespace Kalos.Learning.Neurons
{
    public class InputSynapse : KSynapse
    {
        internal KNeuron ToNeuron;

        public double Weight { get; set; }
        public double Output { get; set; }
        public double PreviousWeight { get; set; }

        public InputSynapse(KNeuron toNeuron)
        {
            ToNeuron = toNeuron;
            Weight = 1;
        }

        public InputSynapse(KNeuron toNeuron, double output)
        {
            ToNeuron = toNeuron;
            Output = output;
            Weight = 1;
            PreviousWeight = 1;
        }

        public double GetOutput()
        {
            return Output;
        }

        public bool IsFromNeuron(Guid fromNeuronId){
            return false;
        }

        public void UpdateWeight(double learningRate, double delta){
            throw new InvalidOperationException("It is not allowed to call this method on Input Connecion");
        }
    }
}

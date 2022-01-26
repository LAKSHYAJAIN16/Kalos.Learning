using System;

namespace Kalos.Learning.Linq.Interfaces
{
    public interface KSynapse
    {
        double Weight { get; set; }
        double PreviousWeight { get; set; }
        double GetOutput();

        bool IsFromNeuron(Guid fromNeuronId);
        void UpdateWeight(double learningRate, double delta);
    }
}

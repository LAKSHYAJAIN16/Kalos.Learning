using System;
using System.Collections.Generic;

namespace Kalos.Learning.Linq.Interfaces
{
    public interface KNeuron
    {
        Guid Id { get; }
        double PreviousPartialDerivate { get; set; }

        List<KSynapse> Inputs { get; set; }
        List<KSynapse> Outputs { get; set; }

        void AddInputNeuron(KNeuron inputNeuron);
        void AddOutputNeuron(KNeuron outputNeuron);
        double CalculateOutput();

        void AddInputSynapse(double inputValue);
        void PushValueOnInput(double inputValue);
    }
}

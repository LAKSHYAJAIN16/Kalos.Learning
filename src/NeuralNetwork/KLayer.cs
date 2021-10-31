using System.Collections.Generic;

namespace Kalos.Learning.Linq.Interfaces
{
    public interface KLayer
    {
        int NeuronCount { get; set; }
        List<KNeuron> Neurons { get; set; }

        KActivationFunction ActivationFunction { get; set; }

        KInputFunction InputFunction { get; set; }

        void ConnectLayers(KLayer inputlayer);
    }
}
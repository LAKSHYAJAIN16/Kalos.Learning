using System.Linq;
using System.Collections.Generic;
using Kalos.Learning.Linq.Interfaces;

namespace Kalos.Learning.Layers
{
    public class Dense : KLayer
    {
        public int NeuronCount { get; set; }
        public List<KNeuron> Neurons { get; set; }
        public KActivationFunction ActivationFunction { get; set; }
        public KInputFunction InputFunction { get; set; }

        public Dense(int neurons, KActivationFunction activation){
            this.NeuronCount = neurons;
            Neurons = new List<KNeuron>();
            this.ActivationFunction = activation;
            this.InputFunction = new Sum();
        }

        public void ConnectLayers(KLayer inputLayer){
            var combos = Neurons.SelectMany(neuron => inputLayer.Neurons, (neuron, input) => new { neuron, input });
            combos.ToList().ForEach(x => x.neuron.AddInputNeuron(x.input));
        }
    }
}

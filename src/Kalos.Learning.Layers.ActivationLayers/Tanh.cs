using System;
using Kalos.Learning.Linq.Interfaces;

namespace Kalos.Learning.Layers.ActivationLayers
{
    public class Tanh : KActivationFunction
    {
        public double FeedForward(double input){
            return Math.Tanh(input);
        }
    }
}

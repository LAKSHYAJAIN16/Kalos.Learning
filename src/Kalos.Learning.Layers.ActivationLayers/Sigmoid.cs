using System;
using Kalos.Learning.Linq.Interfaces;

namespace Kalos.Learning.Layers.ActivationLayers
{
    public class Sigmoid : KActivationFunction
    {
        public double FeedForward(double input)
        {
            return 1 / (1 + Math.Exp(-input));
        }
    }
}

using System;
using Kalos.Learning.Linq.Interfaces;

namespace Kalos.Learning.Layers.ActivationLayers
{
    public class Swish : KActivationFunction
    {
        public double FeedForward(double input)
        {
            return input * (1 / (1 + Math.Exp(-input)));
        }
    }
}
using System;
using Kalos.Learning.Linq.Interfaces;

namespace Kalos.Learning.Layers.ActivationLayers
{
    public class ReLu : KActivationFunction
    {
        public double FeedForward(double input)
        {
            return Math.Max(0, input);
        }
    }
}
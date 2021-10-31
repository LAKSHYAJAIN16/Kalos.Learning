using System;
using Kalos.Learning.Linq.Interfaces;

namespace Kalos.Learning.Layers.ActivationLayers
{
    public class SoftSign : KActivationFunction
    {
        public double FeedForward(double input)
        {
            return input / (Math.Abs(input) + 1);
        }
    }
}
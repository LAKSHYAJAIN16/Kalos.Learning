using System;
using Kalos.Learning.Linq.Interfaces;

namespace Kalos.Learning.Layers.ActivationLayers
{
    public class CoefficientSigmoid : KActivationFunction
    {
        public double coefficient;
        public CoefficientSigmoid(double coefficient){
            this.coefficient = coefficient;
        }

        public double FeedForward(double input){
            return 1 / (1 + Math.Exp(-input));
        }
    }
}
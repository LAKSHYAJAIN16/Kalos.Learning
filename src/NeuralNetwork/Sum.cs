using System.Collections.Generic;
using System.Linq;
using Kalos.Learning.Linq.Interfaces;

namespace Kalos.Learning.Layers
{
    public class Sum : KInputFunction
    {
        public double CalculateInput(List<KSynapse> inputs){
            return inputs.Select(x => x.Weight * x.GetOutput()).Sum();
        }
    }
}

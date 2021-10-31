using System.Collections.Generic;

namespace Kalos.Learning.Linq.Interfaces
{
    public interface KInputFunction
    {
        double CalculateInput(List<KSynapse> inputs);
    }
}

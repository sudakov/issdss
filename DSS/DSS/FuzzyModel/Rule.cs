using System;
using System.Linq;

namespace DSS.DSS.FuzzyModel
{
    class Rule
    {
        private readonly Func<double[], double> _map;
        public readonly Func<double, double> ResolutionFunc;

        public Rule(Func<double[], double> map, Func<double, double> resolutionFunc)
        {
            _map = map;
            ResolutionFunc = resolutionFunc;
        }

        public double[] Apply(double[][] u)
        {
            return u.Select(_map).ToArray();
        }
    }
}
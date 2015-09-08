using System;
using System.ComponentModel;

namespace DSS.DSS.FuzzyModel
{
    public static class ResulutionFunc
    {
        /// <summary>
        /// Безупречный
        /// </summary>
        public static double Best(double x)
        {
            return x < 1 ? 0 : 1;
        }

        /// <summary>
        /// Более, чем удовлетворяющий
        /// </summary>
        public static double VeryGood(double x)
        {
            return x*Math.Sqrt(x);
        }

        /// <summary>
        /// Очень удовлетворяющий
        /// </summary>
        public static double Good(double x)
        {
            return x*x;
        }

        /// <summary>
        /// Удовлетворяющий
        /// </summary>
        public static double Norm(double x)
        {
            return x;
        }

        /// <summary>
        /// Неудовлетворяющий
        /// </summary>
        public static double Bad(double x)
        {
            return 1 - x;
        }

        public static Func<double, double> GetFunc(ResolutionEnum @enum)
        {
            switch (@enum)
            {
            case ResolutionEnum.Best:
                return Best;
            case ResolutionEnum.VeryGood:
                return VeryGood;
            case ResolutionEnum.Good:
                return Good;
            case ResolutionEnum.Norm:
                return Norm;
            case ResolutionEnum.Bad:
                return Bad;
            }
            throw new InvalidEnumArgumentException();
        }
    }
}

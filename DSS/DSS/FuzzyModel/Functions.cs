using System;

namespace DSS.DSS.FuzzyModel
{
    public static class Functions
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
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace DSS.DSS.FuzzyModel
{
    public static class Core
    {
       static readonly double[] J = Enumerable.Range(0, 11).Select(x => x * 0.1).ToArray();
        public static readonly string[] Criteria =
        {
            "Исследовательские способности",
            "Производственный стаж",
            "Опыт преподавания технических дисциплин",
            "Опыт преподавания теории информационных систем",
            "Способность найти заказчика"
        };

        private static readonly Rule[] Rules =
        {
            new Rule(x => Math.Min(x[0], Math.Min(x[1], x[2])), ResulutionFunc.Norm),
            new Rule(x => Math.Min(Math.Min(x[0], x[1]), Math.Min(x[2], x[3])), ResulutionFunc.VeryGood),
            new Rule(x => Math.Min(Math.Min(x[0], x[1]), Math.Min(Math.Min(x[2], x[3]), x[4])), ResulutionFunc.Best),
            new Rule(x => Math.Min(Math.Min(x[0], x[1]), Math.Min(x[2], x[4])), ResulutionFunc.Good),
            new Rule(x => Math.Min(Math.Min(x[0]*x[0], 1 - x[1]), Math.Min(x[2], x[4])), ResulutionFunc.Norm),
            new Rule(x => 1 - Math.Min(x[0], x[2]), ResulutionFunc.Bad)
        };

        private static double[][] Transpose(double[,] array)
        {
            var result = new double[array.GetLength(1)][];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = new double[array.GetLength(0)];
                for (int j = 0; j < result.Length; j++)
                {
                    result[i][j] = array[j, i];
                }
            }
            return result;
        }

        public static FuzzyRankResult Compute(double[,] u)
        {
            double[][] uTransposed = Transpose(u);

            var m = Rules.Select(x => x.Apply(uTransposed)).ToArray();

            double[][,] d = GetRules(m, J);

            double[,] finalD = Intersect(d);
            Debug.WriteLine("_______");
            for (int i = 0; i < finalD.GetLength(0); i++)
            {
                for (int j = 0; j < finalD.GetLength(1); j++)
                {
                    Debug.Write(finalD[i, j].ToString("N") + " ");
                }
                Debug.WriteLine("");
            }

            var e = CalcE(finalD);

            return new FuzzyRankResult(e.Max(), e);
        }
            
        private static double[] CalcE(double[,] d)
        {
            double[] result = new double[d.GetLength(0)];
            double[] x = J;

            for (int i = 0; i < d.GetLength(0); i++)
            {
                var arr = new FuzzyNumber[d.GetLength(1)];

                for (int j = 0; j < arr.Length; j++)
                {
                    arr[j] = new FuzzyNumber(x[j], d[i, j]);
                }
                result[i] = CalcEi(arr);
            }
            return result;
        }

        private static double CalcEi(FuzzyNumber[] arr)
        {
            Debug.WriteLine("_______");
            for (int i = 0; i < arr.Length; i++)
            {
                Debug.Write(arr[i].U.ToString("N") + " ");
            }
            Debug.WriteLine("");
            for (int i = 0; i < arr.Length; i++)
            {
                Debug.Write(arr[i].X.ToString("N") + " ");
            }

            Array.Sort(arr, (a, b) => a.U.CompareTo(b.U));
            var max = arr[arr.Length - 1].U; // last element

            double s = 0;
            double[] m = new double[arr.Length];
            double[] lbd = new double[arr.Length];

            for (int i = 0; i < arr.Length; i++)
            {
                if (i == 0)
                {
                    lbd[i] = arr[0].U;
                    m[i] = CalcM(arr, 0);
                }
                else
                {
                    lbd[i] = arr[i].U - arr[i - 1].U;
                    m[i] = CalcM(arr, i);
                }
            }

            for (int i = 0; i < arr.Length; i++)
                s += lbd[i] * m[i];

            return s / max;
        }

        private static double CalcM(FuzzyNumber[] num, int first)
        {
            double s = 0;
            for (int i = first; i < num.Length; i++)
                s += num[i].X;
            return s / (num.Length - first);
        }

        private static double[,] Intersect(double[][,] d)
        {
            double[,] result = (double[,]) d[0].Clone();
            for (int k = 1; k < d.Length; k++)
            {
                for (int i = 0; i < result.GetLength(0); i++)
                {
                    for (int j = 0; j < result.GetLength(1); j++)
                    {
                        if (result[i, j] > d[k][i, j])
                            result[i, j] = d[k][i, j];
                    }
                }
            }
            return result;
        }

        public static double[][,] GetRules(double[][] m, double[] J)
        {
            var D = new double[Rules.Length][,];
            for (int i = 0; i < D.Length; i++)
            {
                var di = new double[m[0].Length, J.Length];
                for (int j = 0; j < di.GetLength(0); j++)
                {
                    for (int k = 0; k < di.GetLength(1); k++)
                    {
                        di[j, k] = Math.Min(1, 1 - m[i][j] + Rules[i].ResolutionFunc(J[k]));
                        Debug.Write(di[j, k].ToString("N") + " ");
                    }
                    Debug.WriteLine("");
                }
                Debug.WriteLine(new string('-', 40));
                D[i] = di;
            }
            return D;
        }
    }
}

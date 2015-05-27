namespace DSS.DSS.FuzzyModel
{
    public struct FuzzyRankResult
    {
        public readonly double Best;
        public readonly double[] All;

        public FuzzyRankResult(double best, double[] all)
        {
            Best = best;
            All = all;
        }
    }
}

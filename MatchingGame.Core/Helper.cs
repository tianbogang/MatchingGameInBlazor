using System;
using System.Linq;

namespace MatchingGame.Core
{
    public static class Helper
    {
        public static int[] GenerateRandomNumbers(this Random rn, int N)
        {
            return Enumerable.Range(0, N).OrderBy(_ => rn.Next()).ToArray();
        }
    }
}

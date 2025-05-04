namespace Infrastructure.Utility
{
    public static class StringHelper
    {
        public static int LevenshteinDistance(string s, string t)
        {
            if (string.IsNullOrEmpty(s))
            {
                return string.IsNullOrEmpty(t) ? 0 : t.Length;
            }

            if (string.IsNullOrEmpty(t))
            {
                return s.Length;
            }

            var d = new int[s.Length + 1, t.Length + 1];

            ///set the base for the 2d array
            ///	   ""   s	a	m	u	e	l
            ///""   0   1   2   3   4   5   6
            /// s   1
            /// a   2
            /// m   3
            for (var i = 0; i <= s.Length; d[i, 0] = i++) { }
            for (var j = 0; j <= t.Length; d[0, j] = j++) { }

            //after setting the base 2d array, continue to fill the rest of the array
            for (var i = 1; i <= s.Length; i++)
            {
                for (var j = 1; j <= t.Length; j++)
                {
                    var cost = t[j - 1] == s[i - 1] ? 0 : 1;

                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            return d[s.Length, t.Length];
        }

        public static double SimilarityPercentage(string s1, string s2)
        {
            var fName = NormalizeName(s1);
            var sName = NormalizeName(s2);
            int distance = LevenshteinDistance(fName, sName);
            int maxLen = Math.Max(s1.Length, s2.Length);

            // To avoid division by zero if both strings are empty
            if (maxLen == 0) return 100.0;

            double similarity = (1.0 - (double)distance / maxLen) * 100;
            return similarity;
        }

        public static string NormalizeName(string name)
        {
            var words = name.ToLower().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            Array.Sort(words);
            return string.Join(" ", words);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScoring.ProgrammingParadigms
{
    static class ExtensionMethods
    {
        // Accumulate return an equivalent sequence with the values accumlating
        public static IEnumerable<int> Accumulate(this IEnumerable<int> input)
        {
            int sum = 0;
            foreach (var x in input)
            {
                sum += x;
                yield return sum;
            }
        }


        public static int[] Sum(this IEnumerable<int[]> input)
        {
            int[] sum = new int[2];
            sum[0] = 0;
            sum[1] = 0;
            foreach (var x in input)
            {
                sum[0] += x[0];
                sum[1] += x[1];
            }
            return sum;
        }


        
                public static int[] AddIntArray(this int[] value1, int[] value2)
                {
                    int[] result = new int[value1.Length];
                    for (int i = 0; i < value1.Length && i < value2.Length; i++)
                    {
                        result[i]  = value1[i] + value2[i];
                    }
                    return result;
                }
        

    }
}

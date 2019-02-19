using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScoring.ProgrammingParadigms
{
    static class ExtensionMethods
    {
        /// <summary>
        /// Returns an equivalent sequence with the integer values accumlating
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static IEnumerable<int> Accumulate(this IEnumerable<int> input)
        {
            int sum = 0;
            foreach (var x in input)
            {
                sum += x;
                yield return sum;
            }
        }




        /// <summary>
        /// Returns an integer array where elements are the sum of corresponding elements in an IEnumerable of integer arrays
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
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




        /// <summary>
        /// Adds two int arrays. Corresponding elements are added. 
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <returns></returns>
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

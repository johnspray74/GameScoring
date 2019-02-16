using GameScoring.ProgrammingParadigms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScoring.DomainAbstractions
{
    /// <summary>
    /// ALA Domain Abstraction. Binds a function that gets a score (or list or array of scores) to a label. A method returns a scalar string score, using 1 or 2 indexes if the score is a list or array. 
    /// </summary>
    /// <remarks>
    /// ScoreBinding is a domain abstraction in ALA architecture.
    /// It implements the IScoreBinding interface, which scorecard or ScoreBoard types of abstractions can use.
    /// It is typically used with scorecard instances to bind locations on the scoreboard to functions that get the score for that location
    /// It is configured with a label, which a scorecard would typically use at various locations on teh card..
    /// It has a method that is called to get the score.
    /// This abstraction can handle any type useful for returning a single valued score or multi-valued scores in 1 or two dimensions.
    /// see example tenpin bowling and tennis applciations to see example usage of this domain abstraction.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    class ScoreBinding<T> : IScoreBinding
    {
        private readonly string label;
        private readonly Func<T> function;

        /// <summary>
        /// ALA Domain Abstraction. Binds a function that gets a score in one of a variety of types and associates it to an identying label (for example the label is on a Scorecard).
        /// </summary>
        public ScoreBinding(String l, Func<T> f) { label = l; function = f; }


        public string Label { get { return label; } }


        // if no index, the GetScore function can return int or string
        public string GetScore()
        {
            if (typeof(T) == typeof(int))
            {
                return Convert.ToInt32(function()).ToString();
            }
            if (typeof(T) == typeof(string))
            {
                return Convert.ToString(function());
            }
            return "";
        }




        // if one index, the GetScore function can return a list or array of type int or string
        public string GetScore(int x)
        {
            if (typeof(T) == typeof(List<int>))
            {
                List<int> list = (List<int>)Convert.ChangeType(function(),typeof(List<int>));
                if (x < list.Count) return list[x].ToString();
            }
            if (typeof(T) == typeof(int[]))
            {
                int[] array = (int[])Convert.ChangeType(function(), typeof(int[]));
                if (x < array.Length) return array[x].ToString();
            }
            if (typeof(T) == typeof(List<string>))
            {
                List<string> list = (List<string>)Convert.ChangeType(function(), typeof(List<string>));
                if (x < list.Count) return list[x];
            }
            if (typeof(T) == typeof(string[]))
            {
                string[] array = (string[])Convert.ChangeType(function(), typeof(string[]));
                if (x < array.Length) return array[x].ToString();
            }
            return "";
        }




        // if two indexes, the GetScore function can return a list of list of int or a list of array of int
        public string GetScore(int y, int x)
        {
            if (typeof(T) == typeof(List<List<int>>))
            {
                List<List<int>> list = (List<List<int>>)Convert.ChangeType(function(), typeof(List<List<int>>));
                if (y < list.Count && x < list[y].Count) return list[y][x].ToString();
            }
            if (typeof(T) == typeof(List<List<string>>))
            {
                List<List<string>> list = (List<List<string>>)Convert.ChangeType(function(), typeof(List<List<string>>));
                if (y < list.Count && x < list[y].Count) return list[y][x];
            }
            if (typeof(T) == typeof(List<int[]>))
            {
                List<int[]> list = (List<int[]>)Convert.ChangeType(function(), typeof(List<int[]>));
                if (y < list.Count && x < list[y].Length) return list[y][x].ToString();
            }
            return "";
        }

    }

}

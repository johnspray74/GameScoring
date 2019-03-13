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
    /// It is configured with a label, which a scorecard would typically use at various locations on the card.
    /// It has a method that is called to get the score, with 3 overloads for 0, 1 or 2 indexes
    /// This abstraction can handle any type useful for returning a single valued score or multi-valued scores in 1 or two dimensions.
    /// see example tenpin bowling and tennis applciations to see example usage of this domain abstraction.
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    class ScoreBinding<T> : IScoreBinding
    {
        public string Label { get; }
        private readonly Func<T> function;




        /// <summary>
        /// ALA Domain Abstraction. Binds a function that gets a score in one of a variety of types and associates it to an identying label (for example the letter on a Scorecard Template).
        /// </summary>
        public ScoreBinding(String l, Func<T> f) { Label = l; function = f; }




        // if no index, the GetScore function can return int or string
        public string GetScore()
        {
            if (typeof(T) == typeof(int))
            {
                return function().ToString();
            }
            if (typeof(T) == typeof(string))
            {
                object temp = function();
                return (string)temp;
            }
            return "";
        }




        // if one index, the GetScore function can return a list or array of type int or string
        public string GetScore(int x)
        {
            object temp = function();
            if (typeof(T) == typeof(List<int>))
            {
                List<int> list = (List<int>)temp;
                if (x < list.Count) return list[x].ToString();
            }
            if (typeof(T) == typeof(int[]))
            {
                int[] array = (int[])temp;
                if (x < array.Length) return array[x].ToString();
            }
            if (typeof(T) == typeof(List<string>))
            {
                List<string> list = (List<string>)temp;
                if (x < list.Count) return list[x];
            }
            if (typeof(T) == typeof(string[]))
            {
                string[] array = (string[])temp;
                if (x < array.Length) return array[x];
            }
            return "";
        }




        // if two indexes, the GetScore function can return a list of list or a list of array
        public string GetScore(int y, int x)
        {
            object temp = function();
            if (typeof(T) == typeof(List<List<int>>))
            {
                List<List<int>> list = (List<List<int>>)temp;
                if (y < list.Count && x < list[y].Count) return list[y][x].ToString();
            }
            if (typeof(T) == typeof(List<List<string>>))
            {
                List<List<string>> list = (List<List<string>>)temp;
                if (y < list.Count && x < list[y].Count) return list[y][x];
            }
            if (typeof(T) == typeof(List<int[]>))
            {
                List<int[]> list = (List<int[]>)temp;
                if (y < list.Count && x < list[y].Length) return list[y][x].ToString();
            }
            return "";
        }

    }

}

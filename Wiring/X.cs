using System;
using System.Collections.Generic;
using System.Linq;
using Wiring.Interface;

namespace Wiring
{
    /// <summary>
    /// this class is wired up using Wiring.WireUp extension method
    /// typically by the application code
    /// </summary>
    public class X : IC
    {
        // this 
        private List<IA> _aList;

        private IB _b;

        public void DoSomething()
        {
            foreach (var a in _aList)
            {
                a.Execute();
            }
            Console.WriteLine();
        }

        public void Execute()
        {
            _b.Execute();
        }

        
    }

}

using System;
using System.Collections.Generic;
using Wiring.Interface;

namespace Wiring
{
    /// <summary>
    /// this class is wired up using Wiring.WireUp extension method
    /// typically by the application code
    /// </summary>
    public class Y : IA, IB
    {
        private List<IC> _cList = new List<IC>();
        private IC _c;
        void IA.Execute()
        {
            _c.Execute();
            foreach (var c in _cList)
            {
                c.Execute();
            }

        }

        void IB.Execute()
        {
            Console.WriteLine("IB.Execute");
            Console.ReadLine();
        }
    }

}

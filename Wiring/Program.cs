using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wiring
{
    class Program
    {
        static void Main(string[] args)
        {
            var x = new X();
            // var y = new Y();
            x.WireTo(new Y());
            x.DoSomething();
        }
    }
}

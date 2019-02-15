using GameScoring.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScoring.Application
{
    class Start
    {
        static void Main(string[] args)
        {
            new Tennis().Run();
            // new Bowling().Run();
        }
    }
}

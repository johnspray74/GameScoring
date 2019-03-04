using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameScoring.ProgrammingParadigms
{
    interface IPullDataFlow<T>
    {
        T GetData();
    }
}

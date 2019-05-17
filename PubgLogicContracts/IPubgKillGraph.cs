using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PubgLogicContracts
{
    public interface IPubgKillGraph
    {
        void LoadJson();
        void FindPlayerDeaths();

    }
}

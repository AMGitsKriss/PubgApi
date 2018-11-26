using PubgLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace PubgListener
{
    class Program
    {
        const int SECOND = 1000;
        const int MINUTE = SECOND * 60;

        static void Main(string[] args)
        {
            while (true)
            {
                new PubgStatistics();
                Thread.Sleep(5 * MINUTE);
            }
        }
    }
}

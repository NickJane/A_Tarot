using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TarotWinServicesFramework
{
    public interface IWindowTask
    {
        void Run(params object[] parms);
    }
}

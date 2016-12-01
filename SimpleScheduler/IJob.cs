using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleScheduler
{
    public interface IJob
    {
        void Execute();
    }
}

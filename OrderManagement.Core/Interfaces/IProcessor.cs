using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Core.Interfaces
{
    public interface IProcessor
    {
        void Process(string data);
    }
}

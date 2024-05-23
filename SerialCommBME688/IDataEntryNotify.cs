using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SamplingBME688Serial
{
    public interface IDataEntryNotify
    {
        public void dataEntryNotify(bool isSuccess, string message);
    }
}

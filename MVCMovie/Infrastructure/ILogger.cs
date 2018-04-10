using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// Creating temp logger to  verify logging as dependecy injection
namespace MVCMovie.Infrastructure
{
    public interface ILogger
    {
        void log(string message);

    }
}

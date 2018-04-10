using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MVCMovie.Infrastructure
{
    public class Logger_Test_Class_1 : ILogger
    {
        public void log(string message)
        {
            Debug.Write("This is from logger_test_class_1");
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public interface ILogger
    {
        void WriteSuccessLogs(string logData);
        void WriteFailureLogs(string logData);
        void WriteInfoLogs(string logData);
        void WriteWarningLogs(string logData);
    }
}

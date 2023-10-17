using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logging
{
    public class WriteLogsToFile : ILogger
    {
        private readonly string _logFile;
        public WriteLogsToFile(string logFile)
        {
            this._logFile = logFile;

            if (File.Exists(logFile))
            {
                var createdOn = File.GetLastWriteTime(logFile);
                var fileName = Path.GetFileName(logFile);

                var newFileName = logFile.Replace(fileName, Path.GetFileNameWithoutExtension(logFile) + "_" + createdOn.ToString("HHmmss", CultureInfo.InvariantCulture) + ".txt");
                File.Move(logFile, newFileName);
            }

            Directory.CreateDirectory(Path.GetDirectoryName(this._logFile));
        }

        public void WriteSuccessLogs(string logData)
        {
            ConsoleClass.SuccessLog(logData);
            WriteToFile(logData, StatusType.SUCCESS);
        }

        public void WriteFailureLogs(string logData)
        {
            ConsoleClass.FailureLog(logData);
            WriteToFile(logData, StatusType.FAIL);
        }

        public void WriteInfoLogs(string logData)
        {
            ConsoleClass.InfoLog(logData);
            WriteToFile(logData, StatusType.INFO);
        }

        public void WriteWarningLogs(string logData)
        {
            ConsoleClass.WarningLog(logData);
            WriteToFile(logData, StatusType.WARNING);
        }

        private void WriteToFile(string logData, StatusType statusType)
        {
            int retry = 3;
            while (retry > 0)
            {
                try
                {
                    using (TextWriter textWriter = new StreamWriter(this._logFile, true))
                    {
                        textWriter.WriteLine(DateTime.Now.ToString("ddMMMyyyy_HH:mm:ss", CultureInfo.InvariantCulture) + $" {statusType} " + logData);
                    }
                    return;
                }
                catch (Exception exception)
                {
                    File.WriteAllText(Path.Combine(Path.GetDirectoryName(this._logFile), $"ErrorWhileWritingLogs_{DateTime.Now.ToString("ddMMMyyyyHHmmss", CultureInfo.InvariantCulture)}.txt"), exception.ToString());
                }

                retry--;
                Thread.Sleep(100);
            }
        }

        private enum StatusType
        {
            INFO,
            FAIL,
            WARNING,
            SUCCESS
        }
    }
}

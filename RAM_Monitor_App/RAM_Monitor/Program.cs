using Logging;
using RAM_Monitor;
using System.Configuration;
using System.Reflection;
using System.Text;

ILogger log = new WriteLogsToFile();
int delayBetweenSubsequentChecksInSecs = 1;
try
{
    delayBetweenSubsequentChecksInSecs = int.Parse(ConfigurationManager.AppSettings["DelayBetweenSubsequentChecksInSecs"]);
    if (delayBetweenSubsequentChecksInSecs <= 0)
    {
        throw new Exception();
    }
}
catch
{
    delayBetweenSubsequentChecksInSecs = 1;
    log.WriteWarningLogs("Configuration value is not a valid value, switching to default value of One second.");
}

log.WriteUtilityLogs("Started RAM Tracker utility");
var logFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "RAM_Tracker");
Directory.CreateDirectory(logFilePath);
log.WriteSuccessLogs("Logs can be found at: " + logFilePath);
var totalRAM = Helpers.Total_RAM_GB();
var testStart = DateTime.Now;
var columnHeader = $"Total RAM in machine is: {Math.Round(totalRAM, 2)} GB\nDate_Time,RAM in Use (in GB), Free RAM (in GB), Utilization";

Helpers.builder = new StringBuilder();
int intervalToLogResultsInSeconds = delayBetweenSubsequentChecksInSecs * 5;
Helpers.builder.AppendLine(columnHeader.ToString());
// Specify the performance counter for RAM usage
var currentRunningDay = 1;
var filePath = Path.Combine(logFilePath, $"RAM_Usage_Day0{currentRunningDay}_{(DateTime.Now.AddDays(0)).ToString(Helpers.DateFormat)}.csv");

while (true)
{
    try
    {
        Helpers.PerformPingOperation(intervalToLogResultsInSeconds, totalRAM, delayBetweenSubsequentChecksInSecs);

        using (TextWriter textWriter = new StreamWriter(filePath, true))
        {
            textWriter.WriteLine(Helpers.builder.ToString().TrimEnd());
            Helpers.builder.Clear();
            ConsoleClass.InfoLog(($"Last updated results at: {DateTime.Now}"));
        }

        var totalRunningDays = (DateTime.Now.Day - testStart.Day) + 1;
        if (totalRunningDays > currentRunningDay)
        {
            currentRunningDay = totalRunningDays;
            filePath = Path.Combine(logFilePath, $"RAM_Usage_Day0{currentRunningDay}_{(DateTime.Now.AddDays(0)).ToString(Helpers.DateFormat)}.csv");
            Helpers.builder.AppendLine(columnHeader.ToString());
        }
    }
    catch (Exception exception)
    {
        log.WriteFailureLogs(($"Exception thrown while executing. Reason is: {exception}"));
    }
}

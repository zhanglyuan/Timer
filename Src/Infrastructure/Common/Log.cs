using Serilog.Core;
using Serilog.Events;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Log
    {
        public static void Init(string logName)
        {
            Logger logger = new LoggerConfiguration()
              .MinimumLevel.Verbose()
              .WriteTo.Async(c => c.File(System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, $"Log\\{logName}.log"),
                 outputTemplate: @"[{Timestamp:yyyy-MM-dd HH:mm:ss.fff}][{Level}] {Message:lj}{NewLine}{Exception}",
                 rollOnFileSizeLimit: false,
                 rollingInterval: RollingInterval.Day))
             .Enrich.FromLogContext()
              .CreateLogger();

            Serilog.Log.Logger = logger;
        }

        public static void Debug(string logtxt)
        {
            Serilog.Log.Logger.Debug(logtxt);
        }

        public static void Debug(string logtxt, params object[] propertyValues)
        {
            Serilog.Log.Logger.Debug(logtxt, propertyValues);
        }

        public static void Error(string logtxt)
        {
            Serilog.Log.Logger.Error(logtxt);
        }

        public static void Info(string logtxt)
        {
            Serilog.Log.Logger.Information(logtxt);
        }

        public static void Info(string logtxt, object propertyValue)
        {
            Serilog.Log.Logger.Information(logtxt, propertyValue);
        }

        public static void Warn(string logtxt)
        {
            Serilog.Log.Logger.Warning(logtxt);
        }
    }
}
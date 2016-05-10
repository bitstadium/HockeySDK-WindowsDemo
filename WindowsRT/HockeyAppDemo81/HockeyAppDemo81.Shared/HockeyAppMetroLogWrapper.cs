using Microsoft.HockeyApp;
using MetroLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace HockeyAppDemo81
{
    public class HockeyAppMetroLogWrapper : Microsoft.HockeyApp.ILog
    {
        private ILogger Logger;
        private readonly Type _type;

        public HockeyAppMetroLogWrapper(Type type)
        {
            _type = type;
            Logger = LogManagerFactory.DefaultLogManager.GetLogger(type);
        }

        public void Error(Exception exception)
        {
            Logger.Error("ha: Exception", exception);
        }

        public void Info(string format, params object[] args)
        {
            Logger.Debug("ha: " + format, args);
        }

        public void Warn(string format, params object[] args)
        {
            Logger.Warn("ha: " + format, args);
        }
    }
}

using MetroLog.Layouts;
using MetroLog.Targets;
using System;
using System.Collections.Generic;
using System.Text;

namespace HockeyAppDemo
{
    public class InMemoryLogTarget : SyncTarget
    {

        public InMemoryLogTarget(int maxLines = 100)
            : base(new SingleLineLayout())
        {
            this.maxLines = maxLines;
        }
        public InMemoryLogTarget(Layout layout, int maxLines = 100)
            : base(layout)
        {
            this.maxLines = maxLines;
        }

        List<string> logEntries = new List<string>();
        int maxLines = 100;

        public IList<string> LogLines
        {
            get { return logEntries; }
        }

        protected override void Write(MetroLog.LogWriteContext context, MetroLog.LogEventInfo entry)
        {
            lock (this)
            {
                logEntries.Add(Layout.GetFormattedString(context, entry));
                if (logEntries.Count > maxLines)
                {
                    logEntries.RemoveAt(0);
                }
            }
        }
    }
}

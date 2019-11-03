using System;
using System.Collections.Generic;
using System.Text;

namespace YeOldePub.WPF
{
    public class MessageLogEventArgs: EventArgs
    {
        public string LogMessage { get; set; }

        public MessageLogEventArgs(string message)
        {
            LogMessage = message;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YeOldePub.WPF;
using System.Windows.Controls;

namespace YeOldePub.WPF
{
    static public class ListBoxUpdate
    {
        static public void InsertMessageLog(ListBox listBox, YeOldePub yeOldePub)
        {
            (yeOldePub.Bartender.MessageLog != null) yeOldePub.Bartender.MessageLog.TryDequeue();
        }
    }
}

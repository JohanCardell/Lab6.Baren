using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;


namespace YeOldePub.WPF
{
    
    public class DataManager
    {

        private YeOldePub YeOldePub { get; set; }
        private List<ListBox> ListBoxes { get; set; }
        //public event Action<Agent, MessageLogEventArgs> MessageLogged;

        public DataManager(ref List<ListBox> listBoxes)
        {
            ListBoxes = listBoxes;

            //yeOldePub.Bartender.MessageLogged
            //MessageLogged += RefreshList;
        }
        
        public async void OpenClosePub()   
        {
            if (YeOldePub == null)
            {
                YeOldePub = new YeOldePub(this);
            }     
            YeOldePub.
        }

        public async Task RefreshList(YeOldePub yeOldePub, Agent messageLogger, string message)
        {
            var elapsedTime = yeOldePub.stopwatch.Elapsed.Ticks;
            message += $"{elapsedTime} {message}";
            if (messageLogger is Bartender) await Task.Run (() => ListBoxes[0].Items.Insert(0, message));
            else if (messageLogger is Bouncer || messageLogger is Patron) await Task.Run(() => ListBoxes[1].Items.Insert(0, message));
            else if (messageLogger is Waitress) await Task.Run(() => ListBoxes[2].Items.Insert(0, message));
        }
    }
}

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Threading;


namespace YeOldePubSim
{
    
    public class DataManager
    {

        private YeOldePub YeOldePub { get; set; }
        private MainWindow MainWindow { get; set; }
        public DispatcherTimer _timer;
        private TimeSpan _time;
        private int counter = 0;

        public DataManager(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
            _time = TimeSpan.FromSeconds(YeOldePub.TimePubIsOpen);
            _timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                MainWindow.PubCountdown.Content = _time.ToString("c");
                if (_time == TimeSpan.Zero) _timer.Stop();
                _time = _time.Add(TimeSpan.FromSeconds(-1));
            }, Application.Current.Dispatcher);
        }
        
        public void OpenClosePub()   
        {
            if (YeOldePub == null)
            {
                YeOldePub = new YeOldePub(this);
            }
            else
            {
                YeOldePub = null;
                _timer.Stop();
            }
        }

        public void RefreshList(YeOldePub yeOldePub, Agent messageLogger, string message)
        {
            counter++;
           // var elapsedTime = yeOldePub.stopwatch.Elapsed.Ticks;
            string messageConc = counter +": "+ message;
            if (messageLogger is Bartender) MainWindow.Dispatcher.Invoke(() => MainWindow.lbBartender.Items.Insert(0,messageConc));
            else if (messageLogger is Bouncer|| messageLogger is Patron) MainWindow.Dispatcher.Invoke(() => MainWindow.lbPatrons.Items.Insert(0, messageConc));
            else if (messageLogger is Waitress) MainWindow.Dispatcher.Invoke(() => MainWindow.lbWaitress.Items.Insert(0, messageConc));
        }

        public void RefreshLabels ()
        { 
            MainWindow.Dispatcher.Invoke(() =>
            MainWindow.NumOfPatrons.Content = "Patrons: " + YeOldePub.numOfPatrons);
        }  
            
    }
}

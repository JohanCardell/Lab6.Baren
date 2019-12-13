using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;


namespace YeOldePubSim
{

    public class DataManager
    {
        private YeOldePub YeOldePub { get; set; }
        public MainWindow MainWindow { get; set; }

        public TimeSpan time;
        private DispatcherTimer timer;
        private int logCount = 0;

        public DataManager(MainWindow mainWindow)
        {
            MainWindow = mainWindow;
        }

        public void OpenPub()
        {
            YeOldePub = new YeOldePub(this);
            time = TimeSpan.FromSeconds(YeOldePub.TimePubIsOpen);
            timer = new DispatcherTimer(new TimeSpan(0, 0, 1), DispatcherPriority.Normal, delegate
            {
                MainWindow.PubCountdown.Content = time.ToString("c");
                if (time == TimeSpan.Zero && YeOldePub.currentPubState is PubState.Open)
                {
                    YeOldePub.currentPubState = PubState.Closed;
                }
                if (
                    YeOldePub.currentPubState is PubState.Closed &&
                    YeOldePub.Bartender.hasGoneHome is true &&
                    YeOldePub.Bouncer.hasGoneHome is true &&
                    YeOldePub.Waitress.hasGoneHome is true
                    ) timer.Stop();
                time = time.Add(TimeSpan.FromSeconds(-1));

                RefreshLabels();
            }, Application.Current.Dispatcher);
            YeOldePub.Run();
        }

        public void RefreshList(Agent messageLogger, string message)
        {
            logCount++;
            string messageConc = logCount + ": " + message;
            if (messageLogger is Bartender) MainWindow.Dispatcher.Invoke(() => MainWindow.lbBartender.Items.Insert(0, messageConc));
            else if (messageLogger is Bouncer || messageLogger is Patron) MainWindow.Dispatcher.Invoke(() => MainWindow.lbPatrons.Items.Insert(0, messageConc));
            else if (messageLogger is Waitress) MainWindow.Dispatcher.Invoke(() => MainWindow.lbWaitress.Items.Insert(0, messageConc));
        }

        public void RefreshLabels()
        {
            MainWindow.Dispatcher.Invoke(() => MainWindow.NumOfPints.Content = "Pints on shelves: " + YeOldePub.Shelves.Count);
            MainWindow.Dispatcher.Invoke(() => MainWindow.NumOfPatrons.Content = "Patrons: " + YeOldePub.Patrons.Count);
            MainWindow.Dispatcher.Invoke(() => MainWindow.NumOfChairs.Content = "Available chairs: " + (from chair in YeOldePub.Chairs where chair.IsAvailable select chair).Count());
        }
    }
}

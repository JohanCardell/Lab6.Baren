using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace YeOldePubSim
{
    public enum PubState { Open, Closed }
    public class YeOldePub
    {
        public DataManager DataManager { get; set; }
        private const int NumOfPintGLasses = 8;
        private const int NumOfChairs = 9;
        public const int TimePubIsOpen = 120;
        public Bartender Bartender { get; set; }
        public Bouncer Bouncer { get; set; }
        public Waitress Waitress { get; set; }
        public BlockingCollection<PintGlass> Tables;
        public BlockingCollection<PintGlass> Shelves;
        public BlockingCollection<Chair> Chairs;
        public ConcurrentDictionary<String,Patron> Patrons;
        public ConcurrentBag<Agent> Agents;
        public ConcurrentQueue<Patron> PatronsWaitingForBeer;
        public ConcurrentQueue<Patron> PatronsWaitingForChair;
        public System.Diagnostics.Stopwatch stopwatch;
        public ConcurrentBag<Task> tasks;
        public bool partyBusMode = true;

        public Enum currentPubState { get; set; }

        public YeOldePub(DataManager dataManager)
        {
            DataManager = dataManager;
            currentPubState = PubState.Open;
            PatronsWaitingForBeer = new ConcurrentQueue<Patron>();
            PatronsWaitingForChair = new ConcurrentQueue<Patron>();
            Patrons = new ConcurrentDictionary<String,Patron>();
            Shelves = new BlockingCollection<PintGlass>();
            Tables = new BlockingCollection<PintGlass>();
            for (int i = 0; i < NumOfPintGLasses; i++) Shelves.Add(new PintGlass());
            Chairs = new BlockingCollection<Chair>();
            for (int i = 0; i < NumOfChairs; i++) Chairs.Add(new Chair());
            Bartender = new Bartender(this);
            Bouncer = new Bouncer(this);
            Waitress = new Waitress(this);
            Run();
        }

        private void Run()
        {
            //MainWindow._timer.Start();
            Bartender.Run(this);
            Bouncer.Run(this);
            Waitress.Run(this);
            Task.Run(() => IsOpen());
        }

        private void IsOpen()
        {
            Thread.Sleep(TimePubIsOpen * 1000);
            currentPubState = PubState.Closed;
        }
        //private async Task Run()
        //{
            
        //    stopwatch.Start();
        //    foreach (var item in Agents)
        //    {
        //        tasks.Add(Task.Run(() => item.AgentCycle(this)));
        //    }
        //     await Task.WhenAll(tasks);
        //}
    }
}

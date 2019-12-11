using System;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace YeOldePubSim
{
    public enum PubState { Open, Closed }
    public class YeOldePub
    {
        //private YeOldePub _yeOldePub;
        public DataManager DataManager { get; set; }
        public int numOfPatrons = 0;
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
        //public DateTime timeStamp;
        public System.Diagnostics.Stopwatch stopwatch;
        public ConcurrentBag<Task> tasks;

        public Enum currentPubState { get; set; }

        public YeOldePub(DataManager dataManager)
        {
            DataManager = dataManager;
            currentPubState = PubState.Open;
            //stopwatch = new System.Diagnostics.Stopwatch();
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
            //Agents = new ConcurrentBag<Agent>();
            //Agents.Add(Bartender = new Bartender(this));
            //Agents.Add(Bouncer = new Bouncer(this));
            //Agents.Add(Waitress = new Waitress(this));
            //tasks = new ConcurrentBag<Task>();
            //Run();
            
        }

        private void Run()
        {
            DataManager._timer.Start();
            Bartender.Run(this);
            Bouncer.Run(this);
            Waitress.Run(this);
            Task.Run(() => IsOpen());
        }

        private void IsOpen()
        {
            Thread.Sleep(120*1000);
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

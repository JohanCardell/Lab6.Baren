using System;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace YeOldePub.WPF
{
    public enum PubState { Open, Closed }
    public class YeOldePub
    {
        //private YeOldePub _yeOldePub;
        public DataManager DataManager { get; set; }
        private const int NumOfPintGLasses = 8;
        private const int NumOfChairs = 9;
        private const int TimePubIsOpen = 2*60*1000;
        public Bartender Bartender { get; set; }
        public Bouncer Bouncer { get; set; }
        public Waitress Waitress { get; set; }
        public BlockingCollection<PintGlass> Tables;
        public BlockingCollection<PintGlass> Shelves;
        public BlockingCollection<Chair> Chairs;
        public ConcurrentDictionary<string, Patron> Patrons;
        public ConcurrentBag<Agent> Agents;
        
        public ConcurrentQueue<Patron> PatronsWaitingForBeer;
        public ConcurrentQueue<Patron> PatronsWaitingForChair;
        public DateTime timeStamp;
        public System.Diagnostics.Stopwatch stopwatch;
        public ConcurrentBag<Task> tasks;

        public Enum currentPubState { get; set; }

        //public YeOldePub YyOldePub()
        //{
        //    if (_yeOldePub is null) _yeOldePub = new YeOldePub();
        //    else _yeOldePub = null;
        //    return _yeOldePub;
            
        //}
        public YeOldePub(DataManager dataManager)
        {
            DataManager = dataManager;
            currentPubState = PubState.Open;
            Task taskYeOldPub = new Task(OpenPub);
            Tasks.Add(taskYeOldPub);
            taskYeOldPub.Start();
            stopwatch = new System.Diagnostics.Stopwatch();
            PatronsWaitingForBeer = new ConcurrentQueue<Patron>();
            PatronsWaitingForChair = new ConcurrentQueue<Patron>();
            Patrons = new ConcurrentDictionary<string, Patron>();
            Shelves = new BlockingCollection<PintGlass>();
            for (int i = 0; i < NumOfPintGLasses; i++) Shelves.Add(new PintGlass());
            Tables = new BlockingCollection<PintGlass>();
            for (int i = 0; i < NumOfChairs; i++) Chairs.Add(new Chair());
            Chairs = new BlockingCollection<Chair>();
            Agents = new ConcurrentBag<Agent>();
            Agents.Add(Bartender = new Bartender(this));
            Agents.Add(Bouncer = new Bouncer(this));
            Agents.Add(Waitress = new Waitress(this));
            List<Task> tasks = new List<Task>();
        }
        private async Task OpenPub()
        {
            
            stopwatch.Start();
            foreach (var item in Agents)
            {
                //Task task = Task.Run(() => item.AgentCycle(yeOldePub));
                tasks.Add(Task.Run(() => item.AgentCycle(this)));
            }
            await Task.WhenAll(tasks);

        }

        public ()
        {

        }
        public async Task<Task> StartTask(Agent a)
        {
           Task taskedAgent = Task.Run(() => a.AgentCycle(this));
           return taskedAgent;
        }
        public async Task StopTask(Task t)
        {
            
            foreach (Task item in tasks)
            {
                 await Task.Delay(-1);
            }
            await Task.WhenAll(tasks);
        }
        public async Task StopTask (Bartender b)
        {
            b.pa
        }


    }
}

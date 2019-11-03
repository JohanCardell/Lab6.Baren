using System;
using System.Collections.Concurrent;
using System.ComponentModel.Design;
using System.Threading;
using System.Threading.Tasks;

namespace YeOldePub.WPF
{
    public enum PubState { Open, Closed }
    public class YeOldePub
    {
        //private YeOldePub _yeOldePub;
        private const int NumOfPintGLasses = 8;
        private const int NumOfChairs = 9;
        private const int TimePubIsOpen = 2*60*1000;
        public Bartender Bartender;
        public Bouncer Bouncer;
        public Waitress Waitress;
        public BlockingCollection<PintGlass> Tables;
        public BlockingCollection<PintGlass> Shelves;
        public BlockingCollection<Chair> Chairs;
        public ConcurrentDictionary<string, Patron> Patrons;
        public BlockingCollection<Agent> Agents;
        public ConcurrentQueue<Patron> PatronsWaitingForBeer;
        public ConcurrentQueue<Patron> PatronsWaitingForChair;
        public ConcurrentBag<Task> Tasks;
        public DateTime timeStamp;

        public Enum currentPubState { get; set; }

        //public YeOldePub YyOldePub()
        //{
        //    if (_yeOldePub is null) _yeOldePub = new YeOldePub();
        //    else _yeOldePub = null;
        //    return _yeOldePub;
            
        //}
        public YeOldePub()
        {
            currentPubState = PubState.Open;
            Task taskYeOldPub = new Task(OpenPub);
            Tasks.Add(taskYeOldPub);
            taskYeOldPub.Start();
            PatronsWaitingForBeer = new ConcurrentQueue<Patron>();
            PatronsWaitingForChair = new ConcurrentQueue<Patron>();
            Patrons = new ConcurrentDictionary<string, Patron>();
            Shelves = new BlockingCollection<PintGlass>();
            for (int i = 0; i < NumOfPintGLasses; i++) Shelves.Add(new PintGlass());
            Tables = new BlockingCollection<PintGlass>();
            for (int i = 0; i < NumOfChairs; i++) Chairs.Add(new Chair());
            Chairs = new BlockingCollection<Chair>();
            Agents = new BlockingCollection<Agent>();
            Agents.Add(Bartender = new Bartender(this));
            Agents.Add(Bouncer = new Bouncer(this));
            Agents.Add(Waitress = new Waitress(this));
        }
        private void OpenPub()
        {
            Thread.Sleep (TimePubIsOpen);
            currentPubState = PubState.Closed;
        }

    }
}

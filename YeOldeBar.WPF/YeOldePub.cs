using System;
using System.Collections.Concurrent;

namespace YeOldePubSim
{
    public enum PubState { Open, Closed }
    public class YeOldePub
    {
        private const int NumOfPintGLasses = 8;
        private const int NumOfChairs = 9;
        public const int TimePubIsOpen = 120;
        public DataManager DataManager { get; set; }
        public Bartender Bartender { get; set; }
        public Bouncer Bouncer { get; set; }
        public Waitress Waitress { get; set; }
        public BlockingCollection<PintGlass> Tables;
        public BlockingCollection<PintGlass> Shelves;
        public BlockingCollection<Chair> Chairs;
        public ConcurrentDictionary<String,Patron> Patrons;
        public ConcurrentQueue<Patron> PatronsWaitingForBeer;
        public ConcurrentQueue<Patron> PatronsWaitingForChair;
       

        public Enum currentPubState { get; set; }

        public YeOldePub(DataManager dataManager)
        {
            DataManager = dataManager;
            currentPubState = PubState.Open;
            PatronsWaitingForBeer = new ConcurrentQueue<Patron>();
            PatronsWaitingForChair = new ConcurrentQueue<Patron>();
            Patrons = new ConcurrentDictionary<String,Patron>();
            Tables = new BlockingCollection<PintGlass>();
            Shelves = new BlockingCollection<PintGlass>();
            for (int i = 0; i < NumOfPintGLasses; i++) Shelves.Add(new PintGlass());
            Chairs = new BlockingCollection<Chair>();
            for (int i = 0; i < NumOfChairs; i++) Chairs.Add(new Chair());
            Bartender = new Bartender(this);
            Bouncer = new Bouncer(this);
            Waitress = new Waitress(this);
        }

        public void Run()
        {
            Bartender.Run(this);
            Bouncer.Run(this);
            Waitress.Run(this);
        }
    }
}

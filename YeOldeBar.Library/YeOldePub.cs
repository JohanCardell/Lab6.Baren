using System;
using System.Collections.Concurrent;
using System.ComponentModel.Design;

namespace YeOldePub.Library
{
    public enum PubState { Open, Closed }
    public class YeOldePub
    {
        //STARTING VALUES
        //Pub is open for 120 sec
        //8 glasses
        //9 Chairs 
        private YeOldePub _yeOldePub;
        private const int NumOfPintGLasses = 8;
        private const int NumOfChairs = 9;
        private const int TimePubIsOpen = 300;
        private Bartender _bartender;
        private Bouncer _bouncer;
        private Waitress _waitress;
        public BlockingCollection<PintGlass> Tables;
        public BlockingCollection<PintGlass> Shelves;
        public BlockingCollection<Chair> Chairs;
        public ConcurrentDictionary<string, Patron> Patrons;
        public BlockingCollection<IAgent> Agents;
        public ConcurrentQueue<Patron> PatronsWaitingForBeer;
        public ConcurrentQueue<Patron> PatronsWaitingForChair;

        public Enum PubState { get; set; }

        //public YeOldePub YyOldePub()
        //{
        //    if (_yeOldePub is null) _yeOldePub = new YeOldePub();
        //    else _yeOldePub = null;
        //    return _yeOldePub;
            
        //}
        public YeOldePub()
        {
            PubState = Library.PubState.Open;
            _bartender = new Bartender(this);
            _bouncer = new Bouncer(this);
            _waitress = new Waitress(this);
            Tables = new BlockingCollection<PintGlass>();
            Shelves = new BlockingCollection<PintGlass>();
            for (int i = 0; i < NumOfPintGLasses; i++) Shelves.Add(new PintGlass());
            Chairs = new BlockingCollection<Chair>();
            for (int i = 0; i < NumOfChairs; i++) Chairs.Add(new Chair());
            Patrons = new ConcurrentDictionary<string, Patron>();
            Agents = new BlockingCollection<IAgent>();
            PatronsWaitingForBeer = new ConcurrentQueue<Patron>();
            PatronsWaitingForChair = new ConcurrentQueue<Patron>();
        }
    }
}

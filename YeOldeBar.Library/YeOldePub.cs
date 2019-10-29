using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace YeOldePub.Library
{
    enum State { Open, Closed}
    public class YeOldePub
    {
        //STARTING VALUES
        //Pub is open for 120 sec
        //8 glasses
        //9 chairs 
        private const int NumOfPintGLasses  = 8;
        private const int NumOfChairs = 9;
        private const int TimePubIsOpen = 300;

        public Enum PubState { get; set; }
        public YeOldePub()
        {
            PubState = State.Open;
        }
            
        BlockingCollection<IYeOldePubObject> yeOldePub = new BlockingCollection<IYeOldePubObject>();
        BlockingCollection<IAgent> agents = new BlockingCollection<IAgent>();
        BlockingCollection<PintGlass> tables = new BlockingCollection<PintGlass>(); 
        BlockingCollection<PintGlass> shelves = new BlockingCollection<PintGlass>();
        ConcurrentQueue<IAgent> chairs = new ConcurrentQueue<IAgent>();
        ConcurrentQueue<IAgent> beerQueue = new ConcurrentQueue<IAgent>();
        ConcurrentQueue<IAgent> chairQueue = new ConcurrentQueue<IAgent>();
        
    }
}

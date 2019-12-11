using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace YeOldePubSim     
{
    public enum RunState {
        Idling,
        Working,
        WalkingToBar,
        WaitingForBeer,
        WaitingForChair,
        WalkingToChair,
        DrinkingBeer,
        LeavingThePub
    }

    public abstract class Agent
    {
        public DataManager DataManager { get; set; }
        public YeOldePub YeOldePub { get; set; }
        public abstract void AgentCycle(YeOldePub yeOldePub);
        public void Run(YeOldePub yeOldePub)
        {
            Task.Run(() => AgentCycle(yeOldePub));
        }
        //public abstract RunState CheckState(YeOldePub yeOldePub);
    }
}
using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;

namespace YeOldePub.WPF
{
    public enum RunState {
        Idle,
        Work,
        WalkingToBar,
        WaitingForBeer,
        WaitingForChair,
        GoingToChair,
        DrinkingBeer,
        LeavingThePub
    }

    public abstract class Agent
    {
        static public event Action<string> LogMessage;

        public ConcurrentQueue<string> MessageLog;
        public virtual void Activate(YeOldePub yeOldePub) { }
        //RunState CheckState(YeOldePub yeOldePub);
    }
}
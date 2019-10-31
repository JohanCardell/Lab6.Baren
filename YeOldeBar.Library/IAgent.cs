using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace YeOldePub.Library
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

    public interface IAgent
    {

        void Activate(YeOldePub yeOldePub);
        //RunState CheckState(YeOldePub yeOldePub);
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace YeOldePub.Library
{
    public class Bouncer : IYeOldePubObject, IAgent
    {
        //FIRST add random Patron to YeOldePub. Thread.Sleep(3000-10000)
        //THEN check ID. Update YeOldePub with Patron.Name.
        //IF YeOldePub is closed => CurrentState = GoingHome;
        public Enum CurrentState { get; set; }
        private const int NumOfPatronsToLetInside = 1;

        public Bouncer(Enum currentState)
        {
            CurrentState = currentState;
        }

        public void PerformWork()
        {
            throw new NotImplementedException();

        }
    }
}

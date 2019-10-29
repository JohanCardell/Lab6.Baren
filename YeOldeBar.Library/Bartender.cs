using System;
using System.Collections.Generic;
using System.Text;

namespace YeOldePub.Library
{
    public class Bartender : IYeOldePubObject, IAgent
    {
        // wait for patron
        // then go to shelves and get glass (3 sec)
        // if no glass available, wait for clean glass, then get
        // fill glass with beer (3 sec)
        // give glass to patron
        // wait for next patron
        // when no more patrons in pub, go home

        public Enum CurrentState { get; set; }

        public Bartender(Enum currentState) => CurrentState = currentState;

        
        public void PerformWork()
        {
            throw new NotImplementedException();

        }
    }
}

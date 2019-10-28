using System;
using System.Collections.Generic;
using System.Text;

namespace Baren.Library
{
    class Bouncer: IAgent
    {
        //Generate patrons
        public Enum CurrentState { get; set; }

        public Bouncer(Enum currentState)
        {
            CurrentState = currentState;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Baren.Library
{
    class Bartender: IAgent
    {
        public Enum CurrentState { get; set; }

        public Bartender(Enum currentState)
        {
            CurrentState = currentState;
        }
    }
}

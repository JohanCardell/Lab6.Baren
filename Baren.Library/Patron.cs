using System;
using System.Collections.Generic;
using System.Text;

namespace Baren.Library
{
    class Patron: IAgent
    {
        public Enum CurrentState { get; set; }

        public Patron(Enum currentState)
        {
            CurrentState = currentState;
        }
    }
}

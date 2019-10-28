using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Baren.Library
{
    class Waitress: IAgent
    {
        public Enum CurrentState { get; set; }

        public Waitress(Enum currentState)
        {
            CurrentState = currentState;
        }
    }
}

using System;
using System.Diagnostics;

namespace YeOldePub.Library
{
    internal interface IAgent
    {
        Enum CurrentState { get; set; }

        void PerformWork();

    }
}
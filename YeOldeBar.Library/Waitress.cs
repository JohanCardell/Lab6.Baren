using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace YeOldePub.Library
{
    public class Waitress: IYeOldePubObject, IAgent
    {
        //FIRST pick up empty glasses on Tables. Thread.Sleep(10000)
        //THEN wash up glasses. Thread.Sleep(15000)
        //IF no Patrons in YeOldePub => CurrentState = GoingHome
        public List<PintGlass> tray = new List<PintGlass>();
        private const int TimeToCollectGlass = 10000;
        private const int TimeToWash = 15000;
        public Enum CurrentState { get; set; }

        public Waitress(Enum currentState) => CurrentState = currentState;
        public void PerformWork()
        {
            throw new NotImplementedException();
        }
    }
}

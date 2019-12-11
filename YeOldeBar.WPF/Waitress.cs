using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace YeOldePubSim
{
    public class Waitress : Agent
    {
        //FIRST pick up empty glasses on Tables. Thread.Sleep(10000)
        //THEN wash up glasses. Thread.Sleep(15000)
        //IF no Patrons in YeOldePub => CurrentState = GoingHome
        private ConcurrentBag<PintGlass> tray;
        private bool hasGoneHome;
        private const int TimeSpentCollectingPintGlass = 10000;
        private const int TimeSpentWashingPintGlass = 15000;
        private const int TimeSpentIdling = 1000;
        static public ConcurrentQueue<string> messageLog;
        public Enum CurrentState { get; set; }

        //Constructor
        public Waitress(YeOldePub yeOldePub)
        {
            YeOldePub = yeOldePub;
            DataManager = yeOldePub.DataManager;
            CurrentState = RunState.Idling;
            tray = new ConcurrentBag<PintGlass>();
            hasGoneHome = false;
        }

        public override void AgentCycle(YeOldePub yeOldePub)
        {
            while (hasGoneHome is false)
            {
                switch (CheckState(yeOldePub))
                {
                    case RunState.Idling:
                        Thread.Sleep(TimeSpentIdling);
                        break;
                    case RunState.Working:
                        //Gather empty pints from Tables
                        
                            DataManager.RefreshList(yeOldePub, this, "Gathering dirty pints from tables");
                            foreach (var pintGlass in yeOldePub.Tables.Where(g => g.HasBeer is false && g.IsClean is false))
                            {
                                PintGlass gatheredPintGlass = null;
                                while (gatheredPintGlass is null) yeOldePub.Tables.TryTake(out gatheredPintGlass);
                                tray.Add(gatheredPintGlass);
                            }
                            Thread.Sleep(TimeSpentCollectingPintGlass);

                            //Clean glass and place on Shelves
                            DataManager.RefreshList(yeOldePub, this, $"Cleaning {tray.Count} pint(s)");
                            foreach (var pintGlass in tray)
                            {
                                pintGlass.IsClean = true;
                                if (yeOldePub.Shelves.TryAdd(pintGlass)) tray.TryTake(out PintGlass glass);
                            }
                            Thread.Sleep(TimeSpentWashingPintGlass);
                            DataManager.RefreshList(yeOldePub, this, "Finished placing clean pints on the shelves");
                        break;
                    case RunState.LeavingThePub:
                        DataManager.RefreshList(yeOldePub, this, "Going home");
                        hasGoneHome = true;
                        break;
                }
            }
        }
        public RunState CheckState(YeOldePub yeOldePub)
        {
            Thread.Sleep(500);
            if (yeOldePub.Patrons is null && yeOldePub.currentPubState is PubState.Closed) return RunState.LeavingThePub;
            if (yeOldePub.Tables.Count >= 1) return RunState.Working;
            return RunState.Idling;
        }

        //private async Task EmptyTray()
        //{
        //    foreach (var pintGlass in tray)
        //    {
        //        pintGlass.IsClean = true;
        //        while (this.YeOldePub.Shelves.TryAdd(pintGlass)) ;
        //        tray.Remove(pintGlass);
        //    }
        //}
    }
}

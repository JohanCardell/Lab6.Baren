using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace YeOldePub
{
    public class Waitress : Agent
    {
        //FIRST pick up empty glasses on Tables. Thread.Sleep(10000)
        //THEN wash up glasses. Thread.Sleep(15000)
        //IF no Patrons in YeOldePub => CurrentState = GoingHome
        private List<PintGlass> tray;
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
            tray = new List<PintGlass>();
            hasGoneHome = false;
        }

        public override void AgentCycle(YeOldePub yeOldePub)
        {
            while (hasGoneHome is false)
            {
                switch (CheckState(yeOldePub))
                {
                    case RunState.Idling:
                        DataManager.RefreshList(yeOldePub, this, "Is idling");
                        Thread.Sleep(TimeSpentIdling);
                        break;
                    case RunState.Working:
                        //Gather empty pints from Tables
                        if (yeOldePub.Tables != null)
                        {
                            DataManager.RefreshList(yeOldePub, this, "Gathering dirty pints from tables");
                            foreach (var pintGlass in yeOldePub.Tables.Where(g => g.HasBeer is false && g.IsClean is false))
                            {
                                PintGlass gatheredPintGlass = null;
                                while (gatheredPintGlass is null) yeOldePub.Tables.TryTake(out gatheredPintGlass);
                                tray.Add(gatheredPintGlass);
                            }
                            Thread.Sleep(TimeSpentCollectingPintGlass);

                            //Clean glass and place on Shelves
                            DataManager.RefreshList(yeOldePub, this, "Cleaning {tray.Count} pint(s)");
                            foreach (var pintGlass in tray)
                            {
                                pintGlass.IsClean = true;
                                while (yeOldePub.Shelves.TryAdd(pintGlass)) tray.Remove(pintGlass);
                            }
                            Thread.Sleep(TimeSpentWashingPintGlass);
                            DataManager.RefreshList(yeOldePub, this, "Finished placing clean pints on the shelves");
                        }
                        break;
                    case RunState.LeavingThePub:
                        DataManager.RefreshList(yeOldePub, this, "Going home");
                        hasGoneHome = true;
                        break;
                }
            }
        }
        public override RunState CheckState(YeOldePub yeOldePub)
        {
            if (yeOldePub.Patrons is null && yeOldePub.currentPubState is PubState.Closed) return RunState.LeavingThePub;
            if (yeOldePub.Tables != null) return RunState.Working;
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

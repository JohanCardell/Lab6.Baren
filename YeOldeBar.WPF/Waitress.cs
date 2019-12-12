using System;
using System.Collections.Concurrent;
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
        private const int TimeSpentCollectingPintGlass = 10000;
        private const int TimeSpentWashingPintGlass = 15000;
        private const int TimeSpentIdling = 100;
        private bool hasBeenProductive = true;
        public Enum CurrentState { get; set; }

        //Constructor
        public Waitress(YeOldePub yeOldePub)
        {
            YeOldePub = yeOldePub;
            DataManager = yeOldePub.DataManager;
            CurrentState = RunState.Idling;
            tray = new ConcurrentBag<PintGlass>();
        }

        public override void AgentCycle(YeOldePub yeOldePub)
        {
            while (hasGoneHome is false)
            {
                switch (CheckState(yeOldePub))
                {
                    case RunState.Idling:
                        Thread.Sleep(TimeSpentIdling);
                        if (hasBeenProductive is true) DataManager.RefreshList(this, "Is currently idling");
                        hasBeenProductive = false;
                        break;
                    case RunState.Working:
                        //Gather empty pints from Tables
                        DataManager.RefreshList(this, "Gathering dirty pints from tables");
                        foreach (var pintGlass in yeOldePub.Tables.Where(g => g.HasBeer is false && g.IsClean is false))
                        {
                            PintGlass gatheredPintGlass = null;
                            while (gatheredPintGlass is null) yeOldePub.Tables.TryTake(out gatheredPintGlass);
                            tray.Add(gatheredPintGlass);
                        }
                        Thread.Sleep(TimeSpentCollectingPintGlass);

                        //Clean glass and place on Shelves
                        DataManager.RefreshList(this, $"Cleaning {tray.Count} pint(s)");
                        Thread.Sleep(TimeSpentWashingPintGlass);
                        DataManager.RefreshList(this, "Placing clean pints on the shelves");
                        foreach (var pintGlass in tray)
                        {
                            pintGlass.IsClean = true;
                            if (yeOldePub.Shelves.TryAdd(pintGlass)) tray.TryTake(out PintGlass glass);
                        }
                        hasBeenProductive = true;
                        break;
                    case RunState.LeavingThePub:
                        DataManager.RefreshList(this, "Going home");
                        hasGoneHome = true;
                        break;
                }
            }
        }
        public RunState CheckState(YeOldePub yeOldePub)
        {
            if (yeOldePub.Patrons.IsEmpty && yeOldePub.Tables.Count is 0 && yeOldePub.currentPubState is PubState.Closed) return RunState.LeavingThePub;
            if (yeOldePub.Tables.Count > 0) return RunState.Working;
            return RunState.Idling;
        }
    }
}

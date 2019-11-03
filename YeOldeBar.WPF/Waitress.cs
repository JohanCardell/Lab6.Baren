using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace YeOldePub.WPF
{
    public class Waitress : Agent
    {
        //FIRST pick up empty glasses on Tables. Thread.Sleep(10000)
        //THEN wash up glasses. Thread.Sleep(15000)
        //IF no Patrons in YeOldePub => CurrentState = GoingHome
        private List<PintGlass> tray;
        private bool _hasGoneHome;
        private const int TimeSpentCollectingPintGlass = 10000;
        private const int TimeSpentWashingPintGlass = 15000;
        private const int TimeSpentIdling = 1000;
        static public ConcurrentQueue<string> messageLog;
        public Enum CurrentState { get; set; }

        //Constructor
        public Waitress(YeOldePub yeOldePub)
        {
            CurrentState = RunState.Idle;
            tray = new List<PintGlass>();
            _hasGoneHome = false;
            var taskWaitress = Task.Factory.StartNew(() => Activate(yeOldePub));
        }

        public override void Activate(YeOldePub yeOldePub)
        {
            while (_hasGoneHome is false)
            {
                switch (CheckState(yeOldePub))
                {
                    case RunState.Idle:
                        messageLog.Enqueue($"{DateTime.UtcNow}: Is idling");
                        Thread.Sleep(TimeSpentIdling);
                        break;
                    case RunState.Work:
                        //Gather empty pints from Tables
                        if (yeOldePub.Tables != null)
                        {
                            messageLog.Enqueue($"{DateTime.UtcNow}: Gathering dirty pints from tables");
                            foreach (var pintGlass in yeOldePub.Tables.Where(g => g.HasBeer is false && g.IsClean is false))
                            {
                                PintGlass gatheredPintGlass = null;
                                while (gatheredPintGlass is null) _ = yeOldePub.Tables.TryTake(out gatheredPintGlass);
                                tray.Add(gatheredPintGlass);
                            }
                            Thread.Sleep(TimeSpentCollectingPintGlass);

                            //Clean glass and place on Shelves
                            messageLog.Enqueue($"{DateTime.UtcNow}: Cleaning {tray.Count} pint(s)");
                            foreach (var pintGlass in tray)
                            {
                                pintGlass.IsClean = true;
                                var shelved = false;
                                while (shelved is false) shelved = yeOldePub.Shelves.TryAdd(pintGlass);
                                tray.Remove(pintGlass);
                                Thread.Sleep(TimeSpentWashingPintGlass);
                            }
                            messageLog.Enqueue($"{DateTime.UtcNow}: Finished placing clean pints on the shelves");
                        }
                        break;
                    case RunState.LeavingThePub:
                        DataManager.waitressLog.Add($"{DateTime.UtcNow}: Going home");
                        _hasGoneHome = true;
                        break;
                }
            }
        }
        private RunState CheckState(YeOldePub yeOldePub)
        {
            if (yeOldePub.Patrons is null && yeOldePub.currentPubState is PubState.Closed) return RunState.LeavingThePub;
            if (yeOldePub.Tables != null) return RunState.Work;
            return RunState.Idle;
        }
    }
}

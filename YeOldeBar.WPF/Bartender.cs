using System.Threading;
using System.Threading.Tasks;
using System;

namespace YeOldePub.WPF
{

    public class Bartender : Agent, IMessageLogger
    {
        // wait for patron
        // then go to Shelves and get glass (3 sec)
        // if no glass available, wait for clean glass, then get
        // fill glass with beer (3 sec)
        // give glass to patron
        // wait for next patron
        // when no more Patrons in pub, go home

        private bool _hasGoneHome;
        private PintGlass _pintGlass;
        private const int TimeSpentGettingGlass = 3000;
        private const int TimeSpentFillingGlassWithBeer = 3000;

        //public event EventHandler MessageLogged;

        //Constructor
        public Bartender(YeOldePub yeOldePub)
        {
            _hasGoneHome = false;
            var taskBartender = Task.Factory.StartNew(() => Activate(yeOldePub));
        }

        public override void Activate(YeOldePub yeOldePub)
        {
            while (_hasGoneHome is false)
            {
                switch (CheckState(yeOldePub))
                {
                    case RunState.Idle:
                        //Wait before checking for new patron
                        Thread.Sleep(1000);
                        MessageLog.Enqueue($"{DateTime.UtcNow}: Waiting for a patron");
                        //OnMessageLogged(new MessageLogEventArgs($"{DateTime.UtcNow}: Waiting for a patron"));
                        break;
                    case RunState.Work:
                        //Identify patron in first in queue
                        Patron patronBeingServed = null;
                        while (patronBeingServed is null)
                        {
                            _ = yeOldePub.PatronsWaitingForBeer.TryPeek(out patronBeingServed);
                        }
                        MessageLog.Enqueue($"{DateTime.UtcNow}: Taking order from {patronBeingServed}");

                        //Get clean glass from Shelves
                        while (_pintGlass is null)
                        {
                            _ = yeOldePub.Shelves.TryTake(out _pintGlass);
                        }
                        Thread.Sleep(TimeSpentGettingGlass);
                        MessageLog.Enqueue($"{DateTime.UtcNow}: Getting a glass from the shelves");

                        //Fill glass with beer
                        _pintGlass.HasBeer = true;
                        _pintGlass.IsClean = false;
                        Thread.Sleep(TimeSpentFillingGlassWithBeer);
                        MessageLog.Enqueue($"{DateTime.UtcNow}: Filling glass with beer");

                        //Give glass to customer
                        patronBeingServed.PintGlass = _pintGlass;
                        _pintGlass = null;
                        MessageLog.Enqueue($"{DateTime.UtcNow}: Giving beer to {patronBeingServed}");
                        break;
                    case RunState.LeavingThePub:
                        MessageLog.Enqueue($"{DateTime.UtcNow}: Going home");
                        _hasGoneHome = true;
                        break;
                }
            }
        }

        private RunState CheckState(YeOldePub yeOldePub)
        {
            //Check to see if bartender should work or go home
            if (yeOldePub.Patrons is null && yeOldePub.currentPubState is PubState.Closed) return RunState.LeavingThePub;
            if (yeOldePub.PatronsWaitingForBeer.IsEmpty == false && yeOldePub.Shelves.Count > 0) return RunState.Work;
            return RunState.Idle;
        }
        //protected virtual void OnMessageLogged(MessageLogEventArgs message)
        //{
        //    MessageLogged?.Invoke(this, message);
        //}
    }
}

using System.Threading;
using System.Threading.Tasks;

namespace YeOldePub.Library
{
    public class Bartender : IAgent
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

        //Constructor
        public Bartender(YeOldePub yeOldePub)
        {
            _hasGoneHome = false;
            var task = Task.Factory.StartNew(() => Activate(yeOldePub));
        }

        public void Activate(YeOldePub yeOldePub)
        {
            while (_hasGoneHome is false)
            {
                switch (CheckState(yeOldePub))
                {
                    case RunState.Idle:
                        //Wait before checking for new patron
                        Thread.Sleep(1000);
                        break;
                    case RunState.Work:
                        //Identify patron in first in queue
                        Patron patronBeingServed = null;
                        while (patronBeingServed is null)
                        {
                            _ = yeOldePub.PatronsWaitingForBeer.TryPeek(out patronBeingServed);
                        }

                        //Get clean glass from Shelves
                        while (_pintGlass is null)
                        {
                            _ = yeOldePub.Shelves.TryTake(out _pintGlass);
                        }
                        Thread.Sleep(TimeSpentGettingGlass);

                        //Fill glass with beer
                        _pintGlass.HasBeer = true;
                        _pintGlass.IsClean = false;
                        Thread.Sleep(TimeSpentFillingGlassWithBeer);

                        //Give glass to customer
                        patronBeingServed.PintGlass = _pintGlass;
                        _pintGlass = null;
                        break;
                    case RunState.LeavingThePub:
                        _hasGoneHome = true;
                        break;
                }
            }
        }

        private RunState CheckState(YeOldePub yeOldePub)
        {
            //Check to see if bartender should work or go home
            if (yeOldePub.Patrons is null && yeOldePub.PubState is PubState.Closed) return RunState.LeavingThePub;
            if (yeOldePub.PatronsWaitingForBeer.IsEmpty == false && yeOldePub.Shelves.Count > 0) return RunState.Work;
            return RunState.Idle;
        }
    }
}

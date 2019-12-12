using System.Threading;

namespace YeOldePubSim
{

    public class Bartender : Agent
    {
        // wait for patron
        // then go to Shelves and get glass (3 sec)
        // if no glass available, wait for clean glass, then get
        // fill glass with beer (3 sec)
        // give glass to patron
        // wait for next patron
        // when no more Patrons in pub, go home

        private PintGlass pintGlass;
        private const int TimeSpentGettingGlass = 3000;
        private const int TimeSpentFillingGlassWithBeer = 3000;
        private bool hasBeenProductive = true;

        //Constructor
        public Bartender(YeOldePub yeOldePub)
        {

            YeOldePub = yeOldePub;
            DataManager = yeOldePub.DataManager;
        }

        public override void AgentCycle(YeOldePub yeOldePub)
        {
            while (hasGoneHome is false)
            {
                switch (CheckState(yeOldePub))
                {
                    case RunState.Idling:
                        if (hasBeenProductive is true)
                        {
                            if (yeOldePub.PatronsWaitingForBeer.IsEmpty) DataManager.RefreshList(this, "Waiting for a patron");
                            else if (yeOldePub.Shelves.Count is 0) DataManager.RefreshList(this, "Waiting for clean pints");
                        }
                        hasBeenProductive = false;
                        Thread.Sleep(100);
                        break;
                    case RunState.Working:
                        //Identify patron in first in queue
                        Patron patronBeingServed = null;
                        while (patronBeingServed is null)
                        {
                            yeOldePub.PatronsWaitingForBeer.TryPeek(out patronBeingServed);
                        }
                        if (patronBeingServed.pintGlass is null)
                        {
                            DataManager.RefreshList(this, $"Taking order from {patronBeingServed.Name}");

                            //Get clean glass from Shelves
                            while (pintGlass is null) yeOldePub.Shelves.TryTake(out pintGlass);
                            DataManager.RefreshList(this, "Getting a glass from the shelves");
                            Thread.Sleep(TimeSpentGettingGlass);

                            //Fill glass with beer
                            pintGlass.HasBeer = true;
                            pintGlass.IsClean = false;
                            DataManager.RefreshList(this, "Filling glass with beer");
                            Thread.Sleep(TimeSpentFillingGlassWithBeer);

                            //Give glass to customer
                            patronBeingServed.pintGlass = pintGlass;
                            DataManager.RefreshList(this, $"Giving beer to {patronBeingServed.Name}");
                            pintGlass = null;
                            hasBeenProductive = true;
                        }
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
            //Check to see if bartender should work or go home
            if (yeOldePub.Patrons.IsEmpty && yeOldePub.currentPubState is PubState.Closed) return RunState.LeavingThePub;
            else if (yeOldePub.PatronsWaitingForBeer.IsEmpty == false && yeOldePub.Shelves.Count > 0) return RunState.Working;
            else return RunState.Idling;
        }
    }
}

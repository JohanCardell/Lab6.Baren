using System.Threading;
using System.Threading.Tasks;

namespace YeOldePub.WPF
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

        private bool hasGoneHome;
        private PintGlass pintGlass;
        private const int TimeSpentGettingGlass = 3000;
        private const int TimeSpentFillingGlassWithBeer = 3000;

        //Constructor
        public Bartender(YeOldePub yeOldePub)
        {

            YeOldePub = yeOldePub;
            DataManager = yeOldePub.DataManager;
            AgentTask = new Task(async () => await AgentCycle(yeOldePub));
            hasGoneHome = false;
        }
      
        public override async Task AgentCycle(YeOldePub yeOldePub)
        {
            while (hasGoneHome is false)
            {
                switch (CheckState(yeOldePub))
                {
                    case RunState.Idling:
                        //Wait before checking for new patron
                        Thread.Sleep(1000);
                        await DataManager.RefreshList(yeOldePub, this, "Waiting for a patron");
                        break;
                    case RunState.Working:
                        //Identify patron in first in queue
                        Patron patronBeingServed = null;
                        while (patronBeingServed is null)
                        {
                            _ = yeOldePub.PatronsWaitingForBeer.TryPeek(out patronBeingServed);
                        }
                        await DataManager.RefreshList(yeOldePub, this, $"Taking order from {patronBeingServed}");

                        //Get clean glass from Shelves
                        while (pintGlass is null)
                        {
                            _ = yeOldePub.Shelves.TryTake(out pintGlass);
                        }
                        Thread.Sleep(TimeSpentGettingGlass);
                        await DataManager.RefreshList(yeOldePub, this, "Getting a glass from the shelves");

                        //Fill glass with beer
                        pintGlass.HasBeer = true;
                        pintGlass.IsClean = false;
                        Thread.Sleep(TimeSpentFillingGlassWithBeer);
                        await DataManager.RefreshList(yeOldePub, this, "Filling glass with beer");

                        //Give glass to customer
                        patronBeingServed.pintGlass = pintGlass;
                        pintGlass = null;
                        await DataManager.RefreshList(yeOldePub, this, $"Giving beer to {patronBeingServed}");
                        break;
                    case RunState.LeavingThePub:
                        await DataManager.RefreshList(yeOldePub, this, "Going home");
                        hasGoneHome = true;
                        break;
                }
            }
        }

        public override RunState CheckState(YeOldePub yeOldePub)
        {
            //Check to see if bartender should work or go home
            if (yeOldePub.Patrons is null && yeOldePub.currentPubState is PubState.Closed) return RunState.LeavingThePub;
            if (yeOldePub.PatronsWaitingForBeer.IsEmpty == false && yeOldePub.Shelves.Count > 0) return RunState.Working;
            return RunState.Idling;
        }
    }
}

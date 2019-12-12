using System;
using System.Threading;

namespace YeOldePubSim
{
    public class Bouncer : Agent
    {
        //FIRST add random Patron to YeOldePub. Thread.Sleep(3000-10000)
        //THEN check ID. Update YeOldePub with Patron.Name.
        //IF YeOldePub is closed => CurrentState = GoingHome;
        private int NumOfPatronsToLetInside = 1;
        //Constructor
        public Bouncer(YeOldePub yeOldePub)
        {
            YeOldePub = yeOldePub;
            DataManager = yeOldePub.DataManager;
        }

        private static int GetLeadTime() //Bouncer takes different amount of time to let inside each patron
        {
            var rnd = new Random();
            var milliseconds = 1000 * (rnd.Next(3, 11));
            return milliseconds;
        }

        public override void AgentCycle(YeOldePub yeOldePub)
        {
            while (hasGoneHome is false)
            {
                switch (CheckState(yeOldePub))
                {
                    case RunState.Working:
                        Thread.Sleep(GetLeadTime());
                        NumOfPatronsToLetInside = 1;
                        if (YeOldePub.partyBusMode is true && DataManager.time <= TimeSpan.FromSeconds(YeOldePub.TimePubIsOpen - 20.0))
                        {
                            NumOfPatronsToLetInside = 20;
                            yeOldePub.partyBusMode = false;
                        }
                        for (int patron = 0; patron < NumOfPatronsToLetInside; patron++)
                        {
                            var newPatron = new Patron(yeOldePub);
                            yeOldePub.Patrons.TryAdd(newPatron.Name, newPatron);
                        }
                        break;
                    case RunState.LeavingThePub:
                        DataManager.RefreshList(this, "Bouncer is going home");
                        hasGoneHome = true;
                        break;
                }
            }
        }

        public RunState CheckState(YeOldePub yeOldePub)
        {
            if (yeOldePub.currentPubState is PubState.Closed) return RunState.LeavingThePub;
            return RunState.Working;
        }
    }
}

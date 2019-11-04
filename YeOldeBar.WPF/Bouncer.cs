using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace YeOldePub.WPF
{
    public class Bouncer : Agent
    {
        //FIRST add random Patron to YeOldePub. Thread.Sleep(3000-10000)
        //THEN check ID. Update YeOldePub with Patron.Name.
        //IF YeOldePub is closed => CurrentState = GoingHome;
        private const int NumOfPatronsToLetInside = 1;
        private bool hasGoneHome;

        //Constructor
        public Bouncer(YeOldePub yeOldePub)
        {
            YeOldePub = yeOldePub;
            DataManager = yeOldePub.DataManager;
            hasGoneHome = false;
            var taskBouncer = Task.Factory.StartNew(() => AgentCycle(yeOldePub));
        }
              
        private static int GetLeadTime() //Bouncer takes different amount of time to let inside each patron
        {
            var rnd = new Random();
            var milliseconds = 1000 * (rnd.Next(3, 11));
            return milliseconds;
        }

        public override async Task AgentCycle(YeOldePub yeOldePub)
        {
            while (hasGoneHome is false)
            {
                switch (CheckState(yeOldePub))
                {
                    case RunState.Working:
                        for (int patron = 0; patron < NumOfPatronsToLetInside; patron++)
                        {
                            var newPatron = new Patron(yeOldePub);
                            while (!(yeOldePub.Patrons.TryAdd(newPatron.Name, newPatron)));
                            newPatron.DoWork(yeOldePub);
                        }
                        Thread.Sleep(GetLeadTime());
                        break;
                    case RunState.LeavingThePub:
                        hasGoneHome = true;
                        break;
                }
            }
        }

        public override RunState CheckState(YeOldePub yeOldePub)
        {
            if (yeOldePub.currentPubState is PubState.Closed) return RunState.LeavingThePub;
            return RunState.Working;
        }
    }
}

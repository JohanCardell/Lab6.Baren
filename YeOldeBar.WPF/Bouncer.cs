using System;
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
        private bool _hasGoneHome;

        //Constructor
        public Bouncer(YeOldePub yeOldePub)
        {
            _hasGoneHome = false;
            var taskBouncer = Task.Factory.StartNew(() => Activate(yeOldePub));
        }

        private static int GetLeadTime() //Bouncer takes different amount of time to let inside each patron
        {
            var rnd = new Random();
            var milliseconds = 1000 * (rnd.Next(3, 11));
            return milliseconds;
        }

        public override void Activate(YeOldePub yeOldePub)
        {
            while (_hasGoneHome is false)
            {
                switch (CheckState(yeOldePub))
                {
                    case RunState.Work:
                        for (int patron = 0; patron < NumOfPatronsToLetInside; patron++)
                        {
                            var newPatron = new Patron(yeOldePub);
                            while (!(yeOldePub.Patrons.TryAdd(newPatron.Name, newPatron))) ;
                        }
                        Thread.Sleep(GetLeadTime());
                        break;
                    case RunState.LeavingThePub:
                        _hasGoneHome = true;
                        break;
                }
            }
        }

        private RunState CheckState(YeOldePub yeOldePub)
        {
            if (yeOldePub.currentPubState is PubState.Closed) return RunState.LeavingThePub;
            return RunState.Work;
        }
    }
}

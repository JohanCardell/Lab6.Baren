using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace YeOldePub.Library
{
    //public enum RunState
    //{
    //    WalkingToBar,
    //    WaitingForBeer,
    //    WaitingForChair,
    //    GoingToChair,
    //    DrinkingBeer,
    //    LeavingThePub
    //}

    public class Patron : IAgent
    {
        //FIRST go to bar (1 sec)
        //THEN wait for bartender to serve beer
        //THEN search and go to empty chair (4 sec)
        //IF no empty chair, IDLE
        //WHEN seated, drink beer (20-30 se)
        //WHEN beer is finished, LEAVE chair and pub
        private static readonly List<string> PatronNames = new List<string>()
        {
            "Adam", "Bob", "Caesar", "David",
            "Eric", "Francis", "George", "Harold",
            "Ivan", "Joe", "Ken", "Leroy", "Mandrake",
            "Newman", "Otto", "Peter", "Quinn", "Rick",
            "Steven", "Ty", "Urban", "Victor", "Willy"
        };
        public Enum CurrentState { get; set; }
        public string Name { get; set; }
        private int _timeSpentDrinkingBeer;
        public int TimeSpentDrinkingBeer
        {
            get { return _timeSpentDrinkingBeer; }
            set
            {
                Random rnd = new Random();
                int time = 1000 * (rnd.Next(20, 31));
                _timeSpentDrinkingBeer = time;
            }
        }
        private const int TimeSpentWalkingToChair = 4000;
        private const int TimeSpentWaiting = 3000;
        private bool _hasGoneHome;
        //private bool _isThirsty;
        //private bool _isSitting;
        public Chair Chair { get; set; }
        public PintGlass PintGlass { get; set; }
        private ConcurrentQueue<Patron> CurrentQueue { get; set; }

        //Constructor
        public Patron(YeOldePub yeOldePub)
        {
            Name = SetRandomPatronName();
            CurrentState = RunState.WalkingToBar;
            _hasGoneHome = false;
            //_isThirsty = true;
            //_isSitting = false;
            var patronTask = Task.Factory.StartNew(() => Activate(yeOldePub));
        }

        private static string SetRandomPatronName()
        {
            Random rnd = new Random();
            int randomIndex = rnd.Next(0, PatronNames.Count + 1);
            string randomPatronName = PatronNames.ElementAt(randomIndex);
            PatronNames.RemoveAt(randomIndex);
            return randomPatronName;
        }

        public void Activate(YeOldePub yeOldePub)
        {
            while (_hasGoneHome is false)
            {
                switch (CurrentState)
                {
                    case RunState.WalkingToBar:
                        Thread.Sleep(1000);
                        yeOldePub.PatronsWaitingForBeer.Enqueue(this);
                        CurrentState = RunState.WaitingForBeer;
                        break;
                    case RunState.WaitingForBeer:
                        if (PintGlass is null)
                            Thread.Sleep(TimeSpentWaiting); // Give the bartender som time before trying again
                        else
                        {
                            DequeuePatron(CurrentQueue, this);
                            CurrentState = RunState.WaitingForChair;
                        }

                        break;
                    case RunState.WaitingForChair:
                        yeOldePub.PatronsWaitingForChair.Enqueue(this);

                        //Check to see if patron is first in line
                        var isFirstInQueue = false;
                        while (isFirstInQueue is false)
                        {
                            //Spend time checking
                            Thread.Sleep(TimeSpentWaiting);
                            isFirstInQueue = yeOldePub.PatronsWaitingForChair.TryPeek(out var result);
                            if (this != result) continue;
                            foreach (var chair in yeOldePub.Chairs)
                            {
                                if (!(chair.IsAvailable)) continue;
                                this.Chair = chair; //Dibs on available chair
                                chair.IsAvailable = false;
                                DequeuePatron(CurrentQueue, this);
                                CurrentState = RunState.GoingToChair;
                            }
                        }

                        break;
                    case RunState.GoingToChair:
                        Thread.Sleep(TimeSpentWalkingToChair);
                        //_isSitting = true;
                        CurrentState = RunState.DrinkingBeer;
                        break;
                    case RunState.DrinkingBeer:
                        //Drink beer
                        Thread.Sleep(TimeSpentDrinkingBeer);
                        PintGlass.HasBeer = false;
                        //_isThirsty = false;

                        //Place empty glass on table
                        yeOldePub.Tables.Add(PintGlass);
                        //_isSitting = false;
                        Chair.IsAvailable = true;
                        CurrentState = RunState.LeavingThePub;
                        break;
                    case RunState.LeavingThePub:
                        //Remove patron from pub
                        while (_hasGoneHome is false) _hasGoneHome = yeOldePub.Patrons.TryRemove(this.Name, out _);
                        _hasGoneHome = true;
                        break;
                }
                //CheckState(yeOldePub);
            }
        }

        public Patron DequeuePatron(ConcurrentQueue<Patron> currentQueue, Patron patronTryingToLeave)
        {
            Patron patron = null;
            while (patron is null) _ = currentQueue.TryDequeue(out patron);
            return patron;
        }

        //public RunState CheckState(YeOldePub yeOldePub)
        //{

        //    if (PintGlass is null && _isThirsty is true && Chair is null && CurrentQueue is null) return RunState.WalkingToBar;
        //    if (PintGlass is null && _isThirsty is true && Chair is null && CurrentQueue != null) return RunState.WaitingForBeer;
        //    if (PintGlass != null && _isThirsty is true && Chair is null && CurrentQueue != null) return RunState.WaitingForChair;
        //    if (PintGlass != null && _isThirsty is true && Chair != null && CurrentQueue is null) return RunState.GoingToChair;
        //    if (PintGlass != null && _isThirsty is true && Chair != null && _isSitting is true) return RunState.DrinkingBeer;
        //    if (PintGlass != null && _isThirsty is false && Chair != null && _isSitting is true) return RunState.LeavingThePub;
        //    return RunState.Idle;
        //}
        public override string ToString()
        {
            return this.Name;
        }
    }

}

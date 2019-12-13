using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace YeOldePubSim
{
    public class Patron : Agent
    {
        //FIRST go to bar (1 sec)
        //THEN wait for bartender to serve beer
        //THEN search and go to empty chair (4 sec)
        //IF no empty chair, IDLE
        //WHEN seated, drink beer (20-30 se)
        //WHEN beer is finished, LEAVE chair and pub
        private const int TimeSpentWalkingToChair = 4000;
        private const int TimeSpentWaiting = 100;
        private static List<string> PatronNames = new List<string>()
        {
            "Adam", "Angela", "Aby", "Bob" ,"Barbara", "Bill", "Caesar", "Christine", "Charlie", "Charles", "David", "Diane", "Dave",
            "Eric", "Ellen", "Ewan", "Emma", "Erin", "Francis", "Fran", "Frank", "Filippe", "George", "Gill", "Harold", "Henry", "Haley", "Holly",
            "Ivan", "Irene", "Joe", "Jack", "Jill", "Jenny", "Jen", "Jenifer", "Ken", "Kendrik", "Kelly", "Leroy", "Laura", "Mandrake", "Molly",
            "Newman", "Naomi", "Otto", "Ophelia", "Peter", "Pauleen"
        };
        public string Name { get; set; }
        public Enum CurrentState { get; set; }
        public int TimeSpentDrinkingBeer { get; set; }
        public Chair chair;
        public PintGlass pintGlass;
        private ConcurrentQueue<Patron> CurrentQueue { get; set; }

        //Constructor
        public Patron(YeOldePub yeOldePub)
        {
            YeOldePub = yeOldePub;
            DataManager = yeOldePub.DataManager;
            Random rnd = new Random();
            Name = GetRandomPatronName(rnd);
            TimeSpentDrinkingBeer = 1000 * (rnd.Next(20, 31));
            CurrentState = RunState.WalkingToBar;
            
            Run(yeOldePub);
        }

        public static string GetRandomPatronName(Random rnd)
        {
            int randomIndex = rnd.Next(0, PatronNames.Count);
            string randomName = PatronNames.ElementAt(randomIndex);
            PatronNames.RemoveAt(randomIndex);
            return randomName;
        }

        public override void AgentCycle(YeOldePub yeOldePub)
        {
            while (hasGoneHome is false)
            {
                switch (CurrentState)
                {
                    case RunState.WalkingToBar:
                        DataManager.RefreshList(this, $"{Name} entered and is walking to the bar");
                        yeOldePub.PatronsWaitingForBeer.Enqueue(this);
                        CurrentQueue = yeOldePub.PatronsWaitingForBeer;
                        Thread.Sleep(1000);
                        CurrentState = RunState.WaitingForBeer;
                        break;
                    case RunState.WaitingForBeer:
                        DataManager.RefreshList(this, $"{Name} is waiting for a pint of beer");
                        while (pintGlass is null)
                        {
                            Thread.Sleep(TimeSpentWaiting); // Give the bartender som time before trying again
                        }
                        DataManager.RefreshList(this, $"{Name} got a pint of beer");
                        DequeuePatron(CurrentQueue, this);
                        CurrentState = RunState.WaitingForChair;
                        break;
                    case RunState.WaitingForChair:
                        yeOldePub.PatronsWaitingForChair.Enqueue(this);
                        CurrentQueue = yeOldePub.PatronsWaitingForChair;
                        DataManager.RefreshList(this, $"{Name} is waiting for an available chair");
                        //Check to see if patron is first in line
                        var isFirstInQueue = false;
                        while (isFirstInQueue is false)
                        {
                            //Spend time checking
                            Thread.Sleep(TimeSpentWaiting);
                            yeOldePub.PatronsWaitingForChair.TryPeek(out var result);
                            if (this.Equals(result))
                            {
                                isFirstInQueue = true;
                                while (chair is null)
                                {
                                    foreach (var chair in yeOldePub.Chairs)
                                    {
                                        if (!(chair.IsAvailable)) continue;
                                        this.chair = chair; //Dibs on available chair
                                        chair.IsAvailable = false;
                                        DequeuePatron(CurrentQueue, this);
                                        DataManager.RefreshLabels();
                                        break;
                                    }
                                }
                            }
                        }
                        CurrentState = RunState.WalkingToChair;
                        break;
                    case RunState.WalkingToChair:
                        DataManager.RefreshList(this, $"{Name} is walking to a chair");
                        Thread.Sleep(TimeSpentWalkingToChair);
                        CurrentState = RunState.DrinkingBeer;
                        break;
                    case RunState.DrinkingBeer:
                        //Drink beer
                        DataManager.RefreshList(this, $"{Name} is drinking a pint of beer");
                        Thread.Sleep(TimeSpentDrinkingBeer);
                        pintGlass.HasBeer = false;

                        //Place empty glass on table
                        yeOldePub.Tables.Add(pintGlass);
                        chair.IsAvailable = true;
                        CurrentState = RunState.LeavingThePub;
                        break;
                    case RunState.LeavingThePub:
                        //Remove patron from pub
                        DataManager.RefreshList(this, $"{Name} finished the beer and is going home");
                        while (hasGoneHome is false) hasGoneHome = yeOldePub.Patrons.TryRemove(Name, out _);
                        hasGoneHome = true;
                        break;
                }
            }
        }

        public Patron DequeuePatron(ConcurrentQueue<Patron> currentQueue, Patron patronTryingToLeave)
        {
            Patron patron = null;
            while (patron is null) _ = currentQueue.TryDequeue(out patron);
            return patron;
        }
    }
}

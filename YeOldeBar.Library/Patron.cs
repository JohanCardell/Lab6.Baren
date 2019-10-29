using System;
using System.Collections.Generic;
using System.Text;

namespace YeOldePub.Library
{
    public class Patron: IYeOldePubObject, IAgent
    {
        //FIRST go to bar (1 sec)
        //THEN wait for bartender to serve beer
        //THEN search and go to empty chair (4 sec)
        //IF no empty chair, IDLE
        //WHEN seated, drink beer (10-20 se)
        //WHEN beer is finished, LEAVE chair and pub
        public static List<string> PatronNames = new List<string>()
        {
            "Adam","Bob","Caesar", "David",
            "Eric", "Francis", "George", "Harold",
            "Ivan", "Joe", "Ken", "Leroy", "Mandrake",
            "Newman", "Otto", "Peter", "Quinn", "Rick",
            "Steven", "Ty", "Urban", "Victor", "Willy"
        };
        public Enum CurrentState { get; set; }
        public string Name { get; set; }
        private const int TimeSpentDrinkingBeer = 10000;

        public Patron(Enum currentState) => CurrentState = currentState;
        
        public void PerformWork()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}

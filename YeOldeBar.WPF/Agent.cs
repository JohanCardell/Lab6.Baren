using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;

namespace YeOldePub.WPF
{
    public enum RunState {
        Idling,
        Working,
        WalkingToBar,
        WaitingForBeer,
        WaitingForChair,
        WalkingToChair,
        DrinkingBeer,
        LeavingThePub
    }

    public abstract class Agent
    {
        public DataManager DataManager { get; set; }
        public YeOldePub YeOldePub { get; set; }
        public Task AgentTask { get; set; }
        //public event Action<Agent, MessageLogEventArgs> MessageLogged;
        //private protected void OnMessageLogged(Agent sender, MessageLogEventArgs message)
        //{
        //    MessageLogged?.Invoke(this, message);
        //}
        public virtual async Task AgentCycle(YeOldePub yeOldePub) { }
        public async Task Activate(YeOldePub yeOldePub)
        {
            Task.Run(() => AgentCycle(yeOldePub));
        }
        public async Task StartTask()
        {
          
        }
        public async void PauseTask()
        {
            var result = AgentTask.Wait(-1);
        }
        public abstract RunState CheckState(YeOldePub yeOldePub);
    }
}
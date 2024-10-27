using System.Collections.Generic;
using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using UnityEngine;

namespace States
{
    public class ScrollFsmInitializer : MonoBehaviourExt
    {
        private readonly List<IUnsubscribable> unsubscribableStates = new();

        [OnAwake]
        private void CreateFsm()
        {
            Settings.Fsm = new FSM();

            var states = new List<FSMState>
            {
                new InitState(),
                new ScrollState(),
                new StopState()
            };

            foreach (var state in states)
            {
                Settings.Fsm.Add(state);
                AddUnsubscribableState(state);
            }
        }

        [OnStart]
        private void StartFsm()
        {
            Settings.Fsm.Start("InitState");
        }

        [OnUpdate]
        public void UpdateFsm()
        {
            Settings.Fsm.Update(Time.deltaTime);
        }

        private void AddUnsubscribableState(FSMState state)
        {
            if (state is IUnsubscribable unsubscribableState)
            {
                unsubscribableStates.Add(unsubscribableState);
            }
        }

        [OnDestroy]
        private void Destroy()
        {
            foreach (var unsubscribableState in unsubscribableStates)
            {
                unsubscribableState.Unsubscribe();
            }
        }
    }
}
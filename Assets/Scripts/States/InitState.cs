using System;
using AxGrid.FSM;
using UnityEngine;

namespace States
{
    [State("InitState")]
    public class InitState : FSMState, IUnsubscribable
    {
        public InitState()
        {
            Model.EventManager.AddAction("OnStartScrollButtonClick", OnStartScroll);
        }

        [Enter]
        void Enter()
        {
            Model.Set("BtnStartScrollButtonEnable", true);
        }

        private void OnStartScroll()
        {
            Parent.Change("ScrollState");
        }

        public void Unsubscribe()
        {
            Model.EventManager.RemoveAction("OnStartScrollButtonClick", OnStartScroll);
        }
    }
}
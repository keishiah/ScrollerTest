using AxGrid;
using AxGrid.Base;
using AxGrid.FSM;
using UnityEngine;

namespace States
{
    [State("ScrollState")]
    public class ScrollState : FSMState, IUnsubscribable
    {
        public ScrollState()
        {
            Model.EventManager.AddAction("OnStopScrollButtonClick", OnStopScroll);
        }

        [Enter]
        public void Enter()
        {
            Model.Set("BtnStartScrollButtonEnable", false);
            Model.EventManager.Invoke("StartScrolling");
        }
        
        [One(3f)]
        private void EnableStopButton()
        {
            Model.Set("BtnStopScrollButtonEnable", true);
        }


        public void Exit()
        {
        }

        private void OnStopScroll()
        {
            Parent.Change("StopState");
        }

        public void Unsubscribe()
        {
            Model.EventManager.RemoveAction("OnStopScrollButtonClick", OnStopScroll);
        }
    }
}
using AxGrid.FSM;
using UnityEngine;

namespace States
{
    [State("StopState")]
    public class StopState : FSMState, IUnsubscribable
    {
        public StopState()
        {
            Model.EventManager.AddAction("OnStopScrollingAnimationComplete", OnStopAnimationComplete);
        }

        [Enter]
        public void Enter()
        {
            Model.EventManager.Invoke("StopScrolling");
            Model.Set("BtnStopScrollButtonEnable", false);
        }

        private void OnStopAnimationComplete()
        {
            Parent.Change("InitState");
        }

        public void Unsubscribe()
        {
            Model.EventManager.RemoveAction("OnStopScrollingAnimationComplete", OnStopAnimationComplete);
        }
    }
}
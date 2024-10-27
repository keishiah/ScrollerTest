using AxGrid.Base;
using AxGrid.Tools.Binders;

public class ButtonsBinder : MonoBehaviourExt

{
    public UIButtonDataBind startScrollButtonBinder;
    public UIButtonDataBind stopScrollButtonBinder;

    [OnAwake]
    public void Init()
    {
        if (startScrollButtonBinder != null)
        {
            startScrollButtonBinder.buttonName = "StartScrollButton";
        }

        if (stopScrollButtonBinder != null)
        {
            stopScrollButtonBinder.buttonName = "StopScrollButton";
        }
    }
}
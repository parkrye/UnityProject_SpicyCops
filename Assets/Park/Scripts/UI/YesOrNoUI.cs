using UnityEngine.Events;

public class YesOrNoUI : SceneUI
{
    public UnityEvent yesEvent, noEvent;

    public void AddEvent(UnityAction yesAction = null, UnityAction noAction = null)
    {
        yesEvent = new UnityEvent();
        noEvent = new UnityEvent();

        if(yesAction != null)
            yesEvent.AddListener(yesAction);
        if (noAction != null)
            noEvent.AddListener(noAction);
    }

    public void OnYesButtonClicked()
    {
        yesEvent?.Invoke();
    }

    public void OnNoButtonClicked()
    {
        noEvent?.Invoke();
    }
}

using UnityEngine;
using System.Collections;

public abstract class Scroller : MonoBehaviour
{
    public enum ScrollType { Vertical, Horizontal, };
    public ScrollType type;
    public float scrollSpeed = 1f;
    public float startOffset = 0f;
    public bool scrolling { get; set; }

    public delegate void PeriodOverHandler(Scroller scroller);
    public event PeriodOverHandler PeriodOver;

    protected virtual void OnPeriodOver()
    {
        // Make a temporary copy of the event to avoid possibility of
        // a race condition if the last subscriber unsubscribes
        // immediately after the null check and before the event is raised.
        PeriodOverHandler handler = PeriodOver;
        if (handler != null)
            handler(this);
    }

    public abstract void InitScroller();
    public abstract void UpdateScroller();
    public abstract void ResetScroller();
}

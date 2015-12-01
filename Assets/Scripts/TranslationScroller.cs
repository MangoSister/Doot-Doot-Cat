using UnityEngine;
using System.Collections;

public class TranslationScroller : Scroller
{
    public float tileSize;
    private Vector3 startPosition;
    private float currOffset;

    // Use this for initialization
    public override void InitScroller ()
    {
        startOffset = Mathf.Clamp01(startOffset);
        currOffset = startOffset;
        startPosition = transform.position;
        scrolling = true;
    }

    // Update is called once per frame
    public override void UpdateScroller()
    {
        if (!scrolling)
            return;

        float scrollAmount = Time.deltaTime * scrollSpeed;

        if (type == ScrollType.Horizontal)
            transform.position += Vector3.right * scrollAmount;
        else
            transform.position += Vector3.down * scrollAmount;

        currOffset += scrollAmount / tileSize;
        if (currOffset >= 1f)
        {
            OnPeriodOver();

            currOffset = 0f;
            if (type == ScrollType.Horizontal)
                transform.position = startPosition + Vector3.left * startOffset * tileSize;
            else
                transform.position += startPosition + Vector3.up * startOffset * tileSize;
        }
    }

    public override void ResetScroller()
    {
        transform.position = startPosition;
    }
}

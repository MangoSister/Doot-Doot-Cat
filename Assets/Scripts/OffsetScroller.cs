using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OffsetScroller : Scroller
{
    public Vector2 CurrOffset { get { return mat.GetTextureOffset(mainTexName); } }

    private Material mat;
    private const string mainTexName = "_MainTex";

    public override void InitScroller()
    {
        mat = GetComponent<MeshRenderer>().material;
        startOffset = Mathf.Clamp01(startOffset);
        ResetScroller();
        scrolling = true;
    }

    public override void UpdateScroller()
    {
        if (!scrolling)
            return;

        float rawOffset = Time.time * scrollSpeed + startOffset;
        float lastRawOffset = rawOffset - Time.deltaTime * scrollSpeed;

        float offset = Mathf.Repeat(rawOffset, 1f);

        if(type == ScrollType.Vertical)
            mat.SetTextureOffset(mainTexName, new Vector2(0, startOffset) + Vector2.up * offset);
        else
            mat.SetTextureOffset(mainTexName, new Vector2(startOffset, 0) + Vector2.right * offset);

        if (Mathf.FloorToInt(rawOffset) != Mathf.FloorToInt(lastRawOffset))
            OnPeriodOver();
    }

    public override void ResetScroller()
    {
        if (type == ScrollType.Vertical)
            mat.SetTextureOffset(mainTexName, new Vector2(0, startOffset));
        else
            mat.SetTextureOffset(mainTexName, new Vector2(startOffset, 0));
    }

    public void ChangeTex(Texture2D tex)
    {
        mat.SetTexture(mainTexName, tex);
    }
}

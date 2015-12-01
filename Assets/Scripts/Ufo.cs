using UnityEngine;
using System.Collections;

public class Ufo : MonoBehaviour
{
    public bool direction; //true: right, false: left
    public float flyDist;
    public float flySpeed;
    public float waitTime;

    public float circularMoveRadius;
    public float circularMoveFreq;

    // Use this for initialization
    private void Start()
    {
        if (direction)
            transform.rotation = Quaternion.AngleAxis(180f, Vector3.up);
        else transform.rotation = Quaternion.identity;
        StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        yield return new WaitForSeconds(waitTime);
        float dist = 0f;
        float startTime = Time.time;
        Vector3 startPos = transform.position;
        Vector3 dir = direction ? Vector3.right : Vector3.left;
        while (dist < flyDist)
        {
            float circularOffsetX = circularMoveRadius * (1 - Mathf.Cos(circularMoveFreq * (Time.time - startTime)));
            float circularOffsetY = circularMoveRadius * (Mathf.Sin(circularMoveFreq * (Time.time - startTime)));
            float dirOffset = flySpeed * (Time.time - startTime);
            transform.position = startPos + dir * (dirOffset + circularOffsetX) + Vector3.up * circularOffsetY;
            dist = (dirOffset + circularOffsetX);
            yield return null;
        }

        Destroy(gameObject);
    }
}


public class UfoParam
{
    public float _posX;

    public float _posY;

    public bool _direction; //true: right, false: left

    public float _flyDist;

    public float _flySpeed;

    public float _waitTime;

    public float _circularMoveRadius;

    public float _circularMoveFreq;
}

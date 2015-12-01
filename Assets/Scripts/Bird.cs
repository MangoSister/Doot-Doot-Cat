using UnityEngine;
using System.Collections;

public class Bird : MonoBehaviour
{
    private Rect boundary { get { return LevelController.Instance.screenBoundary; } }

    public bool direction; //true: right, false: left
    public float flyDist;
    public float flySpeed;
    public float waitTime;

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
        Vector3 dir = direction ? Vector3.right : Vector3.left;
        while (dist < flyDist)
        {
            dist += Time.deltaTime * flySpeed;
            transform.position += dir * Time.deltaTime * flySpeed;
            yield return null;
        }
        Destroy(gameObject);
    }
}

public class BirdParam
{
    public float _posX;

    public float _posY;

    public bool _direction; //true: right, false: left

    public float _flyDist;

    public float _flySpeed;

    public float _waitTime;
}
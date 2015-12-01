using UnityEngine;
using System.Collections;

public class AeroPlane : MonoBehaviour
{
    private Rect boundary { get { return LevelController.Instance.screenBoundary; } }

    public bool direction; //true: right, false: left
    public float flyDist;
    public float flySpeed;
    public float waitTime;
   
    // Use this for initialization
	private void Start ()
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ShittyCat>() != null)
        {
            Vector3 relativeDir = (other.gameObject.transform.position - transform.position).normalized;
            if (Vector3.Dot(relativeDir, Vector3.up) >= 0.707f)
                Destroy(gameObject);
        }
        else if (other.GetComponent<Shockwave>() != null)
        {
            if (transform.position.y < boundary.yMax)
                Destroy(gameObject);
        }

    }

    private void OnDrawGizmos()
    {
        float xOffset = transform.localScale.x * GetComponent<BoxCollider2D>().size.x * 0.5f;
        float yOffset = transform.localScale.y * GetComponent<BoxCollider2D>().size.y * 0.5f;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(transform.position + new Vector3(xOffset, yOffset, 0),
                        transform.position + new Vector3(-xOffset, yOffset, 0));

        Gizmos.DrawLine(transform.position + new Vector3(-xOffset, yOffset, 0),
                transform.position + new Vector3(-xOffset, -yOffset, 0));

        Gizmos.DrawLine(transform.position + new Vector3(-xOffset, -yOffset, 0),
                transform.position + new Vector3(xOffset, -yOffset, 0));

        Gizmos.DrawLine(transform.position + new Vector3(xOffset, -yOffset, 0),
                transform.position + new Vector3(xOffset, yOffset, 0));
    }
}

public class AeroPlaneParam
{
    public float _posX;

    public float _posY;

    public bool _direction; //true: right, false: left

    public float _flyDist;

    public float _flySpeed;

    public float _waitTime;
}

using UnityEngine;
using System.Collections;

public class Shockwave : MonoBehaviour
{
    public float maxRadius;
    private float currRadius;
    public float spreadSpeed;
    public LayerMask enemyLayer;

    public GameObject shockwaveFXPrefab;
    private GameObject shockwaveFX;
    private CircleCollider2D circleCollider;
	// Use this for initialization
	void Start ()
    {
        currRadius = 0f;
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.radius = currRadius;
        Instantiate(shockwaveFXPrefab, transform.position, Quaternion.identity);
    }
	
	// Update is called once per frame
	void Update ()
    {
        currRadius += spreadSpeed * Time.deltaTime;
        circleCollider.radius = currRadius;
        if (currRadius > maxRadius)
        {
            Destroy(shockwaveFX);
            Destroy(gameObject);
        }
	}


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, currRadius);
    }

    private static int ToLayerNumber(LayerMask mask)
    {
        for (int i = 0; i < 32; i++)
        {
            if ((1 << i) == mask.value)
                return i;
        }
        return -1;
    }
}

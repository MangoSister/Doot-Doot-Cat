using UnityEngine;
using System.Collections;

public class PowerUp : MonoBehaviour
{
	public AudioManager am; 
    public Shockwave shockWavePrefab;

	void Start(){
		if(am == null) am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
	}

    //should be cat only
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ShittyCat>() == null)
            return;

		am.PlayPowerUp();

        //trigger shockwave ring, every enemy get touched is destroyed
        Instantiate(shockWavePrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        float xOffset = transform.localScale.x * GetComponent<BoxCollider2D>().size.x * 0.5f;
        float yOffset = transform.localScale.y * GetComponent<BoxCollider2D>().size.y * 0.5f;

        Gizmos.color = Color.green;
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

public class PowerUpParam
{
    public float _posX;

    public float _posY;
}

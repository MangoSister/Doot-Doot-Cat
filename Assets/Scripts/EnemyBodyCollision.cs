using UnityEngine;
using System.Collections;

public class EnemyBodyCollision : MonoBehaviour
{
    private EnemyDog owner;

    private void Start()
    {
        owner = transform.parent.gameObject.GetComponent<EnemyDog>();
        Debug.Assert(owner != null, "cannot find owner");
    }


    //should be cat only
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ShittyCat>() == null &&
            other.GetComponent<Shockwave>() == null)
            return;

        //uh maybe some effect?
    }
}

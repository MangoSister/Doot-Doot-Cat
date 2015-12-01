using UnityEngine;
using System.Collections;

public class EnemyBalloonCollision : MonoBehaviour
{
    private EnemyDog owner;
    private Rect boundary { get { return LevelController.Instance.screenBoundary; } }
    private void Start()
    {
        owner = transform.parent.gameObject.GetComponent<EnemyDog>();
        Debug.Assert(owner != null, "cannot find owner");
    }

    //should be cat or shockwave
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<ShittyCat>() != null)
        {
            //dog dies
            owner.SetLifeState(false);
        }
        else if (other.GetComponent<Shockwave>() != null)
        {
            if(owner.transform.position.y < boundary.yMax)
                //dog dies
                owner.SetLifeState(false);
        }
    }
}

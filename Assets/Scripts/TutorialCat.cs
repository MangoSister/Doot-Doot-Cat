using UnityEngine;
using System.Collections;

public class TutorialCat : MonoBehaviour, CatInterface
{
    public float dashPower;
    public float boostPower;
    public float bouncePower;

    public Rect boundary;

    public Vector2 constAcc;
    public float constFallingSpeed;

    private Vector2 currVelocity;

    private BoxCollider2D boxCollider;

    public bool myoOrKeyboard;
    private MyoCharacterController myoCtrl;
    private KeyboardController keyboardCtrl;

    // Use this for initialization
    private void Start ()
    {
        myoCtrl = GetComponent<MyoCharacterController>();
        keyboardCtrl = GetComponent<KeyboardController>();
        boxCollider = GetComponent<BoxCollider2D>();
        currVelocity = Vector2.zero;

        //EnableCtrl(true);
    }

    private void FixedUpdate()
    {
        currVelocity.x -= Mathf.Sign(currVelocity.x) * Mathf.Abs(constAcc.x) * Time.fixedDeltaTime;
        //truncate
        if (Mathf.Abs(currVelocity.x) < 0.05)
            currVelocity.x = 0;
        currVelocity.y += constAcc.y * Time.fixedDeltaTime;
        currVelocity.y = Mathf.Max(constFallingSpeed, currVelocity.y);
        transform.position += new Vector3(currVelocity.x, currVelocity.y, 0f) * Time.fixedDeltaTime;

        if (OutsideBoundary(BoundaryType.Left))
        {
            Bounce(true);
        }
        else if (OutsideBoundary(BoundaryType.Right))
        {
            Bounce(false);
        }
        else if (OutsideBoundary(BoundaryType.Top) || OutsideBoundary(BoundaryType.Bottom))
        {
            transform.position += Vector3.down * currVelocity.y * Time.fixedDeltaTime;
        }        

    }

    private void OnDrawGizmos()
    {
        Vector3 origin = transform.position + new Vector3(GetComponent<BoxCollider2D>().offset.x, GetComponent<BoxCollider2D>().offset.y, 0);
        float xOffset = transform.localScale.x * GetComponent<BoxCollider2D>().size.x * 0.5f;// + GetComponent<BoxCollider2D>().offset.x;
        float yOffset = transform.localScale.y * GetComponent<BoxCollider2D>().size.y * 0.5f;// + GetComponent<BoxCollider2D>().offset.y;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(origin + new Vector3(xOffset, yOffset, 0),
                        origin + new Vector3(-xOffset, yOffset, 0));

        Gizmos.DrawLine(origin + new Vector3(-xOffset, yOffset, 0),
                origin + new Vector3(-xOffset, -yOffset, 0));

        Gizmos.DrawLine(origin + new Vector3(-xOffset, -yOffset, 0),
                origin + new Vector3(xOffset, -yOffset, 0));

        Gizmos.DrawLine(origin + new Vector3(xOffset, -yOffset, 0),
               origin + new Vector3(xOffset, yOffset, 0));
    }

    public void Dash(Vector2 dir)
    {
        currVelocity.x = dir.x * dashPower;
        currVelocity.y = dir.y * dashPower;
    }

    public void Boost(Vector2 dir)
    {
        currVelocity.x = dir.x * boostPower;
        currVelocity.y = dir.y * boostPower;
    }

    public void Bounce(bool horiDir)
    {
        if (horiDir)
            currVelocity.x = bouncePower;
        else currVelocity.x = -bouncePower;
    }

    public bool OutsideBoundary(BoundaryType type)
    {
        if (type == BoundaryType.Top)
        {
            float topMost = transform.position.y + boxCollider.size.y * 0.5f + boxCollider.offset.y;
            return topMost > boundary.yMax;
        }
        else if (type == BoundaryType.Bottom)
        {
            float bottomMost = transform.position.y - boxCollider.size.y * 0.5f + boxCollider.offset.y;
            return bottomMost < boundary.yMin;
        }
        else if (type == BoundaryType.Left)
        {
            float leftMost = transform.position.x - boxCollider.size.x * 0.5f + boxCollider.offset.x;
            return leftMost < boundary.xMin;
        }
        else //type == BoundaryType.Right
        {
            float rightMost = transform.position.x + boxCollider.size.x * 0.5f + boxCollider.offset.x;
            return rightMost > boundary.xMax;
        }
    }

    public void EnableCtrl(bool enable)
    {
        myoCtrl = GetComponent<MyoCharacterController>();
        keyboardCtrl = GetComponent<KeyboardController>();
        if (enable && myoOrKeyboard)
        {
            myoCtrl.Active = true;
            keyboardCtrl.Active = false;
        }
        else if (enable && !myoOrKeyboard)
        {
            myoCtrl.Active = false;
            keyboardCtrl.Active = true;
        }
        else
        {
            myoCtrl.Active = false;
            keyboardCtrl.Active = false;
        }
    }

}

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShittyCat : CoroutineFsm<ShittyCatState>, CatInterface
{
    private static LevelController levelController { get { return LevelController.Instance; } }
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

	public AudioManager am;

    public delegate void CenteringHandler();
    public event CenteringHandler PeakTouched;
    public event CenteringHandler CenteringEnd;

    private float droneSpeed;

    private void Start()
    {
		if(am == null) am = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        myoCtrl = GetComponent<MyoCharacterController>();
        keyboardCtrl = GetComponent<KeyboardController>();
        boxCollider = GetComponent<BoxCollider2D>();
        currVelocity = Vector2.zero;

        state = new ShittyCatState();
        SetState(CatStateType.Normal);
        fsmActive = true;
        StartFsm();

        //if (myoOrKeyboard)
        //{
        //    levelController.myo._CalibrationComplete += () => { levelController.Pause = false; };
        //    levelController.Pause = true;
        //}
    }

    private void FixedUpdate()
    {
        if (state.stateName != CatStateType.Drone.ToString())
        {
            currVelocity.x -= Mathf.Sign(currVelocity.x) * Mathf.Abs(constAcc.x) * Time.fixedDeltaTime;
            //truncate
            if (Mathf.Abs(currVelocity.x) < 0.05)
                currVelocity.x = 0;
            currVelocity.y += constAcc.y * Time.fixedDeltaTime;
            currVelocity.y = Mathf.Max(constFallingSpeed, currVelocity.y);
            transform.position += new Vector3(currVelocity.x, currVelocity.y, 0f) * Time.fixedDeltaTime;
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

    public void EnableCtrl(bool enable)
    {
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

    private void SetState(CatStateType type)
    {
        state.stateName = type.ToString();
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

    public void ActivateDrone(float speed)
    {
        state.stateName = CatStateType.Drone.ToString();
        droneSpeed = speed;
    }

    public void DeactivateDrone()
    {
        state.stateName = CatStateType.Normal.ToString();
    }

    private IEnumerator NormalCoroutine()
    {
        //start (optional)
        EnableCtrl(true);
        yield return null;

        while (state.stateName == CatStateType.Normal.ToString())
        {
            //do nothing since being controller by player
            if (OutsideBoundary(BoundaryType.Bottom))
                SetState(CatStateType.Dead);
            else if (OutsideBoundary(BoundaryType.Top))
            {
                if (PeakTouched != null)
                    PeakTouched();
                SetState(CatStateType.Centering);
            }
            else if (OutsideBoundary(BoundaryType.Left))
            {
                SetState(CatStateType.BouncingRight);
            }
            else if (OutsideBoundary(BoundaryType.Right))
            {
                SetState(CatStateType.BouncingLeft);
            }
            yield return null;
        }

        //end (optional)   
        EnableCtrl(false);
    }
  
    private IEnumerator CenteringCoroutine()
    {
        //dunno what to do exactly when game is over
        currVelocity.x = 0f;
        //float oldVeloY = currVelocity.y;
        float oldFallingSpeed = constFallingSpeed;
        constFallingSpeed = -1f * Mathf.Abs(levelController.centeringScrollSpeed);
        while (transform.position.y > 0f)
        {
            currVelocity.y = constFallingSpeed;
            yield return null;
        }

        currVelocity.y = 0;
        constFallingSpeed = oldFallingSpeed;
        if (CenteringEnd != null)
            CenteringEnd();
        SetState(CatStateType.Normal);
    }

    private IEnumerator BouncingLeftCoroutine()
    {
        currVelocity.x = -bouncePower;
        yield return new WaitForSeconds(0.5f);

        SetState(CatStateType.Normal);
    }

    private IEnumerator BouncingRightCoroutine()
    {
        currVelocity.x = bouncePower;
        yield return new WaitForSeconds(0.5f);

        SetState(CatStateType.Normal);
    }

    private IEnumerator DeadCoroutine()
    {
        //dunno what to do exactly when game is over
        fsmActive = false;
        Debug.Log("CAT DIEEEEEEEEEEEEEEEEEEEEE");
		am.PlayCatDieSound();
		am.StopMusic();
		transform.GetChild(0).GetComponent<Animator>().SetBool("isDead", true);
        currVelocity.x = 0f;
        while(transform.position.y > -10f)
            yield return null;
        Destroy(gameObject);
        SceneManager.Instance.TransitScene(SceneManager.SceneType.Lose);
    }

    private IEnumerator DroneCoroutine()
    {
        while (state.stateName == CatStateType.Drone.ToString() &&
            Mathf.Abs(transform.position.x) > 0.05f)
        {
            
            Vector2 currPos2d = Vector2.MoveTowards(new Vector2(transform.position.x, transform.position.y), new Vector2(0, 0), droneSpeed * Time.deltaTime);
            transform.position = new Vector3(currPos2d.x, currPos2d.y, transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(0, 0, transform.position.z);

        //while (state.stateName == CatStateType.Drone.ToString())
        //{
        //    transform.position += Vector3.up * droneSpeed * Time.deltaTime;
        //    yield return null;
        //}
        //state.stateName = CatStateType.Normal.ToString();
    }

    //should be enemy only
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<EnemyBodyCollision>() != null)
        {
            //cat dies
            boxCollider.enabled = false;
            SetState(CatStateType.Dead);
        }
        else if (other.GetComponent<EnemyBalloonCollision>() != null)
        {
            //get a boost!!!
            Boost(Vector2.up.normalized);
        }
        else if (other.GetComponent<AeroPlane>() != null)
        {
            Vector3 relativeDir = (transform.position - other.gameObject.transform.position ).normalized;
            if (Vector3.Dot(relativeDir, Vector3.up) < 0.707f)
                SetState(CatStateType.Dead);
        }
    }

    private enum CatStateType
    {
        Normal, Dead, Centering, BouncingLeft, BouncingRight, Drone,
    }
}


public class ShittyCatState : FsmState
{
    private static readonly Dictionary<string, string> statesDict = new Dictionary<string, string>
        {
            { "Normal", "NormalCoroutine"},
            { "Dead", "DeadCoroutine"},
            { "Centering", "CenteringCoroutine" },
            { "BouncingLeft", "BouncingLeftCoroutine"},
            { "BouncingRight", "BouncingRightCoroutine"},
            { "Drone", "DroneCoroutine"},
        };

    private string name;

    public string stateName
    {
        get { return name; }
        set
        {
            if (statesDict.ContainsKey(value))
                name = value;
            else throw new UnityException("no such state name");
        }
    }

    public string stateCoroutine
    {
        get { return statesDict[name]; }
    }
}
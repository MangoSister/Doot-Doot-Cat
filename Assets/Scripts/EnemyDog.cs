using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

public class EnemyDog : CoroutineFsm<EnemyDogState>
{
    //only when cat steps on the head of dog can cat kills dog
    //otherwise cat dies
    public float wanderRange;
    public float wanderFreq;
    public float wanderOffset;

    public float fallingAcc;
    public float maxFallingSpeed;

    private float startTime;

	public AudioManager am;

    public BoxCollider2D bodyCollider;
    public CircleCollider2D balloonColldier;

    private void OnDrawGizmos()
    {
        Vector3 origin = bodyCollider.transform.position + new Vector3(bodyCollider.offset.x, bodyCollider.offset.y, 0);
        float xOffset = bodyCollider.transform.localScale.x * bodyCollider.size.x * 0.5f;// + GetComponent<BoxCollider2D>().offset.x;
        float yOffset = bodyCollider.transform.localScale.y * bodyCollider.size.y * 0.5f;// + GetComponent<BoxCollider2D>().offset.y;

        Gizmos.color = Color.black;
        Gizmos.DrawLine(origin + new Vector3(xOffset, yOffset, 0),
                        origin + new Vector3(-xOffset, yOffset, 0));

        Gizmos.DrawLine(origin + new Vector3(-xOffset, yOffset, 0),
                origin + new Vector3(-xOffset, -yOffset, 0));

        Gizmos.DrawLine(origin + new Vector3(-xOffset, -yOffset, 0),
                origin + new Vector3(xOffset, -yOffset, 0));

        Gizmos.DrawLine(origin + new Vector3(xOffset, -yOffset, 0),
               origin + new Vector3(xOffset, yOffset, 0));

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(balloonColldier.gameObject.transform.position, balloonColldier.radius);
    }

    private void Start()
    {
		if(am == null) am = GameObject.Find("AudioManager").GetComponent<AudioManager>();

		startTime = Time.time;

        state = new EnemyDogState();
        SetLifeState(true);
        fsmActive = true;
        StartFsm();
    }

    public void SetLifeState(bool alive)
    {
        if (alive)
            state.stateName = "Alive";
        else state.stateName = "Dead";
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.P))
    //        SetLifeState(false);
    //}

    private IEnumerator AliveCoroutine()
    {
        //start (optional)
        //yield return null;

        //update
        while (state.stateName == "Alive")
        {
            //wandering around (horizontally?)
            //sinusoidal movement
            //sin(x+y) = sinx * cosy + cosx * siny
            float lastPhase = (Time.time - startTime - Time.deltaTime) * wanderFreq + wanderOffset;
            float deltaPhase = Time.deltaTime * wanderFreq;
            float deltaMove = wanderRange * (Mathf.Sin(lastPhase) * (Mathf.Cos(deltaPhase) - 1f) + Mathf.Cos(lastPhase) * Mathf.Sin(deltaPhase));

            transform.position += Vector3.right * deltaMove;
            yield return null;
        }
        
        //end (optional)     
    }

    private IEnumerator DeadCoroutine()
    {
		am.PlayDogDieSound();

        fsmActive = false;
        //start (optional)
        balloonColldier.enabled = false;
        bodyCollider.enabled = false;
        float currFallingSpeed = 0f;
		transform.GetChild(2).GetComponent<Animator>().SetBool("isDead", true);

        //update
        while (transform.position.y > -10f)
        {
            currFallingSpeed += fallingAcc;
            currFallingSpeed = Mathf.Clamp(currFallingSpeed, currFallingSpeed, maxFallingSpeed);
            transform.position += Vector3.down * Time.deltaTime * currFallingSpeed;
            yield return null;
        }

        //end (optional)
        Destroy(gameObject);
    }
}

public class EnemyDogState : FsmState
{
    private static readonly Dictionary<string, string> statesDict = new Dictionary<string, string>
        {
            { "Alive", "AliveCoroutine"},
            { "Dead", "DeadCoroutine"}
        };

    private string name;

    public string stateName
    {
        get { return name; }
        set
        {
            if(statesDict.ContainsKey(value))
                name = value;
            else throw new UnityException("no such state name");
        }
    }

    public string stateCoroutine
    {
        get { return statesDict[name]; }
    }
}


public class EnemyDogParam
{
    public float _posX;

    public float _posY;

    //[XmlAttribute("range")]
    public float _wanderRange;

    //[XmlAttribute("freq")]
    public float _wanderFreq;

    //[XmlAttribute("offset")]
    public float _wanderOffset;

    //[XmlAttribute("fallacc")]
    public float _fallingAcc;

    //[XmlAttribute("maxfallspeed")]
    public float _maxFallingSpeed;
}

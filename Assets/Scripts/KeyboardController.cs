using UnityEngine;
using System.Collections;

public class KeyboardController : MonoBehaviour
{
    private CatInterface cat;
    public bool Active { get; set; }
	// Use this for initialization
	void Start ()
    {
        cat = GetComponent<CatInterface>();
	}

    // Update is called once per frame
    void Update()
    {
        if (!Active)
            return;

        if (Input.GetKeyDown(KeyCode.W))
            cat.Dash(Vector2.up.normalized);
        else if (Input.GetKeyDown(KeyCode.A))
            cat.Dash(new Vector2(-1f,1f).normalized);
        else if (Input.GetKeyDown(KeyCode.D))
            cat.Dash(Vector2.one.normalized);
    }
}

using UnityEngine;
using System.Collections;

public class MyoCharacterController : MonoBehaviour
{
    private static MyoController myoCtrl
    {
        get
        { return MyoController.Instance; }
    }

    private CatInterface cat;

    public bool Active { get; set; }

	public AudioManager am;

    // Use this for initialization
    private void Start()
    {
		if(am == null) am = GameObject.Find("AudioManager").GetComponent<AudioManager>();

        cat = GetComponent<ShittyCat>();
        cat = GetComponent<CatInterface>();
        myoCtrl._WaveLeft += OnWaveLeft;
        myoCtrl._WaveCenter += OnWaveCenter;
        myoCtrl._WaveRight += OnWaveRight;
    }



    private void OnWaveLeft()
    {
        if (!Active)
            return;
        cat.Dash(new Vector2(-1f, 1f).normalized);

		am.PlayWhoosh();
    }

    private void OnWaveCenter()
    {
        if (!Active)
            return;
        cat.Dash(Vector2.up.normalized);

		am.PlayWhoosh();
    }

    private void OnWaveRight()
    {
        if (!Active)
            return;
        cat.Dash(Vector2.one.normalized);

		am.PlayWhoosh();
    }

}

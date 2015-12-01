using UnityEngine;
using System.Collections;

public class CreditsManager : MonoBehaviour {

	public AudioManager am;

	IEnumerator ResetScene(){
		yield return new WaitForSeconds(.5f);
        am.StopMusic();
		am.PlayEndingMusic(); 
	}

	// Use this for initialization
	void Start () {
		if(am == null) am = GameObject.Find("AudioManager").GetComponent<AudioManager>();

		StartCoroutine(ResetScene());

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

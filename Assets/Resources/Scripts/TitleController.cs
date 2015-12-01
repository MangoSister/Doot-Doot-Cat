using UnityEngine;
using System.Collections;

public class TitleController : MonoBehaviour {
	public AudioManager am;

	IEnumerator PlayTitleScene(){
		yield return new WaitForSeconds(.5f);
		am.PlayTitle();
		yield return new WaitForSeconds(3f);
		Debug.Log("Done");
	}

	// Use this for initialization
	void Start () {
		if(am == null) am = GameObject.Find("AudioManager").GetComponent<AudioManager>();

		StartCoroutine(PlayTitleScene());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

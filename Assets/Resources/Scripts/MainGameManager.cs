using UnityEngine;
using System.Collections;

public class MainGameManager : MonoBehaviour {
	public AudioManager am;
	public LevelController lc;
	bool firstSwitch = false;
	bool secondSwitch = false;

	IEnumerator ResetStage(){
		yield return new WaitForSeconds(.5f);
		am.ResetMusic();
	}

	// Use this for initialization
	void Start () {
		if(am == null) am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
		if(lc == null) lc = GameObject.Find("LevelController").GetComponent<LevelController>();

		StartCoroutine(ResetStage());
	}
	
	// Update is called once per frame
	void Update () {
		if(!firstSwitch && (lc.currBlock > lc.totalBlock/3f)){
			Debug.Log(lc.currBlock);
			am.FadeTogetherMusic1And2();
			firstSwitch = true;
		}
		else if(!secondSwitch && (lc.currBlock > lc.totalBlock/(3f/2f))){
			Debug.Log(lc.currBlock);
			am.FadeTogetherMusic2And3();
			secondSwitch = true;
		}
	}
}

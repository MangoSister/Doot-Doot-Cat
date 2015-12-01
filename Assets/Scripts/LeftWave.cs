using UnityEngine;
using System.Collections;

public class LeftWave : MonoBehaviour {
	MyoController myo;

	void Start(){
		myo = MyoController.Instance;
	}

	void OnTriggerEnter(Collider col){
		myo.DetectWaveFinal(0);
		//Debug.Log("Left");
	}
}

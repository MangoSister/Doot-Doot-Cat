using UnityEngine;
using System.Collections;

public class RightWave : MonoBehaviour {
	MyoController myo;
	
	void Start(){
		myo = MyoController.Instance;
	}

	void OnTriggerEnter(Collider col){
		myo.DetectWaveFinal(2);
		//Debug.Log("Right");
	}
}

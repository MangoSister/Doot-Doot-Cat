using UnityEngine;
using System.Collections;

public class CenterWave : MonoBehaviour {
	MyoController myo;
	
	void Start(){
		myo = MyoController.Instance;
	}

	void OnTriggerEnter(Collider col){
		myo.DetectWaveFinal(1);
		//Debug.Log("Center");
	}
}

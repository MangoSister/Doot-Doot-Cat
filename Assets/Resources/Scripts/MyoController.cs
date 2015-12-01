/*
 * MyoController.cs
 * 
 * Notes: Extremely finicky. Only works accurate with very small movements.
 * 
 */

using UnityEngine;
using System.Collections;

public class MyoController : GenericSingleton<MyoController> {
	// Myo game object to connect with.
	// This object must have a ThalmicMyo script attached.
	#region Myo Events
	public GameObject myo = null;
	private ThalmicMyo _myoTM;
	
	public delegate void _MyoGestureHandler();
	public event _MyoGestureHandler _WaveLeft;
	public event _MyoGestureHandler _WaveCenter;
	public event _MyoGestureHandler _WaveRight;
	public event _MyoGestureHandler _CalibrationComplete;
	#endregion
	
	#region Myo Detection
	enum Region{Left, Right, Center, None};
	
	private float detectionInterval = .3f;
	private Vector3 _previousArmVector = Vector3.zero;
	private Vector3 _nextArmVector = Vector3.zero;
	private float _xBoundLeft = -50f;
	private float _xBoundRight = 50f;
	public float _yBound = 80f;
	
	float _xAngleBoundLeft = 300;
	float _xAngleBoundCenter = 10;
	float _xAngleBoundRight = 60;
	float _zUpwardsBound = 40;
	
	private bool _detectionLock = false;
	private float _detectionLockWait = 0f;
	private float _detectionLockWaitTime = .5f;
	
	public bool vibrateOnSuccess = true;

	public int recentWave = -1;
	#endregion
	
	#region Myo Calibration
	bool currentlyCalibrating = false;
	bool calibrated = false;

	public int phase = 0;
	#endregion
	
	private void CollectData(){
		_previousArmVector = _nextArmVector;
		_nextArmVector = _myoTM.gyroscope;
	}
	
	void Start () {
		_myoTM = myo.GetComponent<ThalmicMyo>();
		
		//TO DO: Activate this only on sync.
		//Though, might not need it? It still tracks position well I think.
		InvokeRepeating("CollectData", 0, detectionInterval);
	}
	
	private float mod(float x, int m){
		float r = x;
		while(r < 0){r += m;}
		while(r >= m){r -= m;}
		return r;
	}
	
	//detects whether the angle is left, right, or between the bounds of the z angle bounds.
	private Region getRegion(float xAngle, float xLeft, float xRight, float xCenter){
		Debug.Log("Bounds: " + xLeft + ", " + xCenter + ", " + xRight);
		
		if(xAngle > xRight && xAngle < xLeft){
			return Region.Center;
		}
		else if(xLeft < xRight && 
		        (xAngle > xRight || xAngle < xLeft)){
			return Region.Center;
		}
		else if(xAngle > xLeft && xAngle < (xLeft + 90)){
			return Region.Left;
		}
		else if(mod(xLeft + 90, 360) < xLeft && 
		        (xAngle > xLeft || xAngle < mod(xLeft + 90, 360))){
			return Region.Left;
		}
		else if(xAngle < xRight && xAngle > (xRight - 90)){
			return Region.Right;
		}
		else if(mod(xRight - 90, 360) > xRight && 
		        (xAngle < xRight || xAngle > mod(xRight - 90, 360))){
			return Region.Right;
		}
		return Region.None;
	}
	
	//Calibrates the ranges of the waves.
	public IEnumerator BetterCalibrateWave(){
		Debug.Log(_myoTM.arm);

		float offset = .5f;
		Transform colliders = transform.parent.Find("Colliders");
		//Set the positions of the colliders.
		if (_myoTM.arm == Thalmic.Myo.Arm.Left){
			Debug.Log("Left!");
			colliders.localPosition = new Vector3(offset, 0, 0);
		}
		else if (_myoTM.arm == Thalmic.Myo.Arm.Right){
			Debug.Log("Right!");
			colliders.localPosition = new Vector3(-offset, 0, 0);
		}

		phase = 1;

		Debug.Log("Please point your arm forward.");
		yield return new WaitForSeconds(5f);
		gameObject.GetComponent<JointOrientation>().updateReference = true;
		
		Debug.Log("Calibrating wave.");

		phase = 2;

		Debug.Log("Wave left.");
		//while(_nextArmVector.y <= _yBound){
		while(recentWave != 0){
			yield return new WaitForFixedUpdate();
		}
		_xAngleBoundLeft = transform.eulerAngles.z;
		Debug.Log("Left Wave: " + _xAngleBoundLeft);
		
		yield return new WaitForSeconds(1f);

		phase = 3; 

		Debug.Log("Wave right.");
		//while(_nextArmVector.y <= _yBound){
		while(recentWave != 2){
			yield return new WaitForFixedUpdate();
		}
		_xAngleBoundRight = transform.eulerAngles.z;
		Debug.Log("Right Wave: " + _xAngleBoundRight);
		
		yield return new WaitForSeconds(1f);

		phase = 4;

		Debug.Log("Wave center.");
		//while(_nextArmVector.y <= _yBound){
		while(recentWave != 1){
			yield return new WaitForFixedUpdate();
		}
		_xAngleBoundCenter = transform.eulerAngles.z;
		Debug.Log("Center Wave: " + _xAngleBoundCenter);

		phase = 0;
		Debug.Log("Calibration complete!");
		calibrated = true;
		currentlyCalibrating = false;
		if(_CalibrationComplete != null) _CalibrationComplete();
	}


	//0: left, 1: center, 2: right
	public void DetectWaveFinal(int dir){
		if(_detectionLock){
			//Check if you should unlock it yet.
			if(Time.time - _detectionLockWait > _detectionLockWaitTime){
				_detectionLock = false;
			}
		}
		
		//If it's been unlocked, proceed.
		if(!_detectionLock){
			//if (_nextArmVector.y > _yBound) {

				switch(dir){
				case 0:
					Debug.Log ("Wave left");
					if(_WaveLeft != null) _WaveLeft();
					if(vibrateOnSuccess) _myoTM.Vibrate(Thalmic.Myo.VibrationType.Short);
					_detectionLock = true;
					_detectionLockWait = Time.time;
					recentWave = 0;
					break;
				case 1:
					Debug.Log("Wave center");
					if(_WaveCenter != null) _WaveCenter();
					if(vibrateOnSuccess) _myoTM.Vibrate(Thalmic.Myo.VibrationType.Short);
					_detectionLock = true;
					_detectionLockWait = Time.time;
					recentWave = 1;
					break;
				case 2:
					Debug.Log ("Wave right");
					if(_WaveRight != null) _WaveRight();
					if(vibrateOnSuccess) _myoTM.Vibrate(Thalmic.Myo.VibrationType.Short);
					_detectionLock = true;
					_detectionLockWait = Time.time;
					recentWave = 2;
					break;
				default:
					//recentWave = -1;
					break;
				}
			//}
		}
	}
	
	void Update () {
		if(Input.GetKeyDown("space") && !currentlyCalibrating){
			StartCoroutine(BetterCalibrateWave());
		}
	}
}

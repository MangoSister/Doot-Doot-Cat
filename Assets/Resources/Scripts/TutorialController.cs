using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour {
	public GameObject myo = null;
	private ThalmicMyo _myoTM;
	private MyoController mc;
	
	private GameObject HoldArmOut, LeftArmSet, RightArmSet;
	private GameObject mainArm;
	private GameObject Text;
	private GameObject TutorialCat;

	public AudioManager am;
	public bool startGame = false;

	void ResetScene(){
		if(myo == null) myo = GameObject.Find("Myo");
		_myoTM = myo.GetComponent<ThalmicMyo>();
		//mc = GameObject.Find("Joint").GetComponent<MyoController>();
        mc = MyoController.Instance;
		HoldArmOut = GameObject.Find("HoldArmOut");
		LeftArmSet = GameObject.Find("LeftArm");
		RightArmSet = GameObject.Find("RightArm");
		Text = GameObject.Find("Text");
		TutorialCat = GameObject.Find("TutorialCat");
		TutorialCat.SetActive(false);
		
		if(am == null) am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
	}

	// Use this for initialization
	void Start () {
		ResetScene();
		am.PlayTutorialMusic();

		StartCoroutine(TutorialScript());
	}

	private void OnWaveCenter()
	{
		startGame = true;
		Debug.Log("Game start!");
	}

	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator TutorialScript(){
		startGame = false;
		float gameStartCountdown = 0f;

		yield return new WaitForSeconds(2f);
		StartCoroutine(mc.BetterCalibrateWave());

		//Instructions.
		//Cat meows.
		am.PlayMeowSpeech1();

		while(mc.phase < 1){
			yield return null;
		}

		am.PlayMeowSpeech2();

		//Check if left or right arm.
		//If left, display left set.
		if (_myoTM.arm == Thalmic.Myo.Arm.Left) {
			LeftArmSet.SetActive(true);
			mainArm = LeftArmSet;
			
		}
		//Else, display right set.
		else {
			RightArmSet.SetActive(true);
			mainArm = RightArmSet;
		}

		if (_myoTM.arm == Thalmic.Myo.Arm.Right) HoldArmOut.transform.eulerAngles = new Vector3(0, 180, 0);
		HoldArmOut.GetComponent<SpriteRenderer>().enabled = true;

		Text.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().enabled = false;
		Text.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;


		while(mc.phase < 2){
			yield return null;
		}

		am.PlayMeowSpeech2();
		Text.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
		Text.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;

		//Left arm.
		HoldArmOut.GetComponent<SpriteRenderer>().enabled = false;
		mainArm.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = true;

		while(mc.phase < 3){
			yield return null;
		}

		am.PlayMeowSpeech2();

		Text.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
		Text.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().enabled = true;

		//Right arm.
		mainArm.transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>().enabled = false;
		mainArm.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = true;

		while(mc.phase < 4){
			yield return null;
		}

		am.PlayMeowSpeech2();

		Text.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().enabled = false;
		Text.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().enabled = true;

		//Center arm.
		mainArm.transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().enabled = false;
		mainArm.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().enabled = true;

		while(mc.phase != 0){
			yield return null;
		}

		am.PlayMeowSpeech1();

		Text.transform.GetChild(3).gameObject.GetComponent<SpriteRenderer>().enabled = false;
		Text.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().enabled = true;

		mainArm.transform.GetChild(2).gameObject.GetComponent<SpriteRenderer>().enabled = false;

		//Cat drops on screen for calibration purposes.
		TutorialCat.transform.position = Vector3.zero;
		TutorialCat.SetActive(true);
        TutorialCat.GetComponent<TutorialCat>().EnableCtrl(true);
		//Wait 5 seconds
		yield return new WaitForSeconds(10f);

		am.PlayMeowSpeech2();

		Text.transform.GetChild(4).gameObject.GetComponent<SpriteRenderer>().enabled = false;
		Text.transform.GetChild(5).gameObject.GetComponent<SpriteRenderer>().enabled = true;

		//Start countdown.
		gameStartCountdown = Time.time;

		mc._WaveCenter += OnWaveCenter;

		while(Time.time - gameStartCountdown < 5f && !startGame){
			yield return null;
		}

		mc._WaveCenter -= OnWaveCenter;

		if(startGame){
			//If the player swipes up, game starts.
			SceneManager.Instance.TransitScene(SceneManager.SceneType.Main);
		}
		else{
			//If the player waits 5 seconds, send cat back and start calibration over.
			ResetScene();
			StartCoroutine(TutorialScript());

		}
	}
}

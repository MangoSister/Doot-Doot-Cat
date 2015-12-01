using UnityEngine;
using System.Collections;

public class AudioManager : GenericSingleton<AudioManager>, AudioManagerInterface 
{
	public static AudioManager instance = null;     //Allows other scripts to call functions from the AudioManager.

	#region Music Audio Sources
	public AudioSource MusicPlayer1;
	public AudioSource MusicPlayer2;
	public AudioSource MusicPlayer3;
	public AudioSource MusicPlayerOther;
	public AudioSource SoundPlayerWind;
	public AudioSource SoundPlayerCat;
	public AudioSource SoundPlayerDog;
	public AudioSource SoundPlayerAirplane;
	public AudioSource SoundPlayerOther;
	#endregion

	#region Audio Clips
	public AudioClip Music1;
	public AudioClip Music2;
	public AudioClip Music3;
	public AudioClip TutorialMusic;
	public AudioClip DeathMusic;
	public AudioClip EndingMusic;

	public AudioClip TitleDialogue;

	public AudioClip[] WhooshSound;

	public AudioClip MeowSpeech1;
	public AudioClip MeowSpeech2;
	public AudioClip[] MeowSound;
	public AudioClip CatDieSound;

	public AudioClip DogSound;
	public AudioClip DogDieSound;

	public AudioClip BalloonPopSound;
	public AudioClip AirplaneSound;

	public AudioClip PowerUpSound;
	#endregion
	
	void Awake (){
		//Check if there is already an instance of SoundManager
		if (instance == null)
			//if not, set it to this.
			instance = this;
		//If instance already exists:
		else if (instance != this)
			//Destroy this, this enforces our singleton pattern so there can only be one instance of SoundManager.
			Destroy (gameObject);
		
		//Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad (gameObject);
	}

	void Start(){
		if(MusicPlayer1 == null) MusicPlayer1 = GameObject.Find("MusicPlayer1").GetComponent<AudioSource>();
		if(MusicPlayer2 == null) MusicPlayer2 = GameObject.Find("MusicPlayer2").GetComponent<AudioSource>();
		if(MusicPlayer3 == null) MusicPlayer3 = GameObject.Find("MusicPlayer3").GetComponent<AudioSource>();
		if(MusicPlayerOther == null) MusicPlayerOther = GameObject.Find("MusicPlayerOther").GetComponent<AudioSource>();

		if(SoundPlayerWind == null) SoundPlayerWind = GameObject.Find("SoundPlayerWind").GetComponent<AudioSource>();
		if(SoundPlayerCat == null) SoundPlayerCat = GameObject.Find("SoundPlayerCat").GetComponent<AudioSource>();
		if(SoundPlayerDog == null) SoundPlayerDog = GameObject.Find("SoundPlayerDog").GetComponent<AudioSource>();
		if(SoundPlayerAirplane == null) SoundPlayerAirplane = GameObject.Find("SoundPlayerAirplane").GetComponent<AudioSource>();
		if(SoundPlayerOther == null) SoundPlayerOther = GameObject.Find("SoundPlayerOther").GetComponent<AudioSource>();

		if(Music1 == null) Music1 = Resources.Load<AudioClip>("Audio/Music/Music1");
		if(Music2 == null) Music2 = Resources.Load<AudioClip>("Audio/Music/Music2");
		if(Music3 == null) Music3 = Resources.Load<AudioClip>("Audio/Music/Music3");
		if(TutorialMusic == null) TutorialMusic = Resources.Load<AudioClip>("Audio/Music/TutorialMusic");
		if(DeathMusic == null) DeathMusic = Resources.Load<AudioClip>("Audio/Music/DeathMusic");
		if(EndingMusic == null) EndingMusic = Resources.Load<AudioClip>("Audio/Music/Ending");

		if(TitleDialogue == null) TitleDialogue = Resources.Load<AudioClip>("Audio/Other/Title");

		if(MeowSpeech1 == null) MeowSpeech1 = Resources.Load<AudioClip>("Audio/Cat/MeowSpeech1");
		if(MeowSpeech2 == null) MeowSpeech2 = Resources.Load<AudioClip>("Audio/Cat/MeowSpeech2");
		MeowSound = Resources.LoadAll<AudioClip>("Audio/Cat/Meow");
		if(CatDieSound == null) CatDieSound = Resources.Load<AudioClip>("Audio/Other/CatDeath2");

		if(DogSound == null) DogSound = Resources.Load<AudioClip>("Audio/Dog/DogWoof");
		if(DogDieSound == null) DogDieSound = Resources.Load<AudioClip>("Audio/Other/DogDie2");

		WhooshSound = Resources.LoadAll<AudioClip>("Audio/SoundEffects/Wind");

		if(BalloonPopSound == null) BalloonPopSound = Resources.Load<AudioClip>("Audio/Other/BalloonPop1"); 
		if(AirplaneSound == null) AirplaneSound = Resources.Load<AudioClip>("Audio/Other/Airplane");
		if(PowerUpSound == null) PowerUpSound = Resources.Load<AudioClip>("Audio/Other/Powerup");

		//TESTING AREA
		//PlayStupidRecorderCatBase();
		//InvokeRepeating("PlayWhoosh", 0, 1f);
		//ResetMusic();
	}

	#region General
	private void PlaySoundOnce(AudioSource audSource, AudioClip audClip){
		if(audSource != null){
			audSource.clip = audClip;
			audSource.Play();
		}
		else Debug.Log ("Error: AudioSource not available.");
	}
	#endregion

	#region Music
	public void PlayStupidRecorderCat1(){
		if(MusicPlayer1 != null){
			MusicPlayer1.clip = Music1;
			MusicPlayer1.Play();
		}
		else Debug.Log ("Error: MusicPlayer1 not available.");
	}

	public void PlayStupidRecorderCat2(){
		if(MusicPlayer2 != null){
			MusicPlayer2.clip = Music2;
			MusicPlayer2.Play();
		}
		else Debug.Log ("Error: MusicPlayer2 not available.");
	}

	public void PlayStupidRecorderCat3(){
		if(MusicPlayer3 != null){
			MusicPlayer3.clip = Music3;
			MusicPlayer3.Play();
		}
		else Debug.Log ("Error: MusicPlayer3 not available.");
	}

	public void PlayTutorialMusic(){
		MusicPlayerOther.loop = true;
		PlaySoundOnce(MusicPlayerOther, TutorialMusic);
	}
	public void PlayDeathMusic(){
		MusicPlayerOther.loop = true;
		PlaySoundOnce(MusicPlayerOther, DeathMusic);
	}
	public void PlayEndingMusic(){
		MusicPlayerOther.loop = false;
		PlaySoundOnce(MusicPlayerOther, EndingMusic);
	}

	public void ResetMusic(){
		MusicPlayer1.volume = .6f;
		MusicPlayer2.volume = 0f;
		MusicPlayer3.volume = 0f;

		PlayStupidRecorderCat1();
		PlayStupidRecorderCat2();
		PlayStupidRecorderCat3();
	}

	public void StopMusic(){
		MusicPlayer1.Stop();
		MusicPlayer2.Stop();
		MusicPlayer3.Stop();
	}

	private IEnumerator FadeOutMusic(AudioSource fadeOutSource){
		while(fadeOutSource.volume > 0f){
			fadeOutSource.volume -= .005f;
			yield return new WaitForFixedUpdate();
		}
	}

	private IEnumerator FadeInMusic(AudioSource fadeInSource){
		while(fadeInSource.volume < .6f){
			fadeInSource.volume += .005f;
			yield return new WaitForFixedUpdate();
		}
	}

	//Both the Base and Full must be playing.
	public void FadeTogetherMusic1And2(){
		if(!MusicPlayer1.isPlaying || !MusicPlayer2.isPlaying){
			Debug.Log("Error: One or more music players are not currently playing!");
		}
		else{
			StartCoroutine(FadeOutMusic(MusicPlayer1));
			StartCoroutine(FadeInMusic(MusicPlayer2));
		}
	}

	public void FadeTogetherMusic2And3(){
		if(!MusicPlayer2.isPlaying || !MusicPlayer3.isPlaying){
			Debug.Log("Error: One or more music players are not currently playing!");
		}
		else{
			StartCoroutine(FadeOutMusic(MusicPlayer2));
			StartCoroutine(FadeInMusic(MusicPlayer3));
		}
	}
	#endregion

	#region Cat Sounds
	public void PlayMeowSpeech1(){
		SoundPlayerCat.volume = .8f;
		PlaySoundOnce(SoundPlayerCat, MeowSpeech1);
	}
	public void PlayMeowSpeech2(){
		SoundPlayerCat.volume = .8f;
		PlaySoundOnce(SoundPlayerCat, MeowSpeech2);
	}
	public void PlayMeowSound(){
		int index = Random.Range(0, 1);
		SoundPlayerCat.clip = MeowSound[index];
		
		SoundPlayerCat.Play();
	}
	public void PlayCatDieSound(){PlaySoundOnce(SoundPlayerCat, CatDieSound);}
	#endregion

	#region Dog Sounds
	public void PlayDogSound(){PlaySoundOnce(SoundPlayerDog, DogSound);}
	public void PlayDogDieSound(){PlaySoundOnce(SoundPlayerDog, DogDieSound);}
	#endregion

	#region Sound Effects and Other
	public void PlayTitle(){
		SoundPlayerOther.volume = .8f; 
		PlaySoundOnce(SoundPlayerOther, TitleDialogue);
	}

	//Plays a random whoosh sound. Very low chance it's Kristian.
	public void PlayWhoosh(){
		float chanceOfDumbWhoosh = .1f;

		//Play Kristian's whoosh.
		if(Random.value < chanceOfDumbWhoosh){
			SoundPlayerWind.clip = WhooshSound[3];
		}
		//Play normal whoosh.
		else{
			int index = Random.Range(0, 3);
			SoundPlayerWind.clip = WhooshSound[index];
		}

		SoundPlayerWind.Play();
	}

	public void PlayAirplane(){PlaySoundOnce(SoundPlayerAirplane, AirplaneSound);}
	public void PlayPowerUp(){PlaySoundOnce(SoundPlayerOther, PowerUpSound);}
	#endregion

	void Update(){
		if(Input.GetKeyDown("space")){
			FadeTogetherMusic1And2();
		}
	}
}
using UnityEngine;
using System.Collections;

public interface AudioManagerInterface {
	#region Music Players
	void PlayStupidRecorderCat1();
	void PlayStupidRecorderCat2();
	void PlayStupidRecorderCat3();
	void PlayTutorialMusic();
	void PlayDeathMusic();
	void PlayEndingMusic();

	void ResetMusic(); //Resets the music, like at the beginning of gameplay.
	void FadeTogetherMusic1And2(); //Fades from 1st track to 2nd track.
	void FadeTogetherMusic2And3(); //Fades from 2nd track to 3rd track.
	#endregion

	#region Cat Sound Players
	void PlayMeowSpeech1(); //One version of the cat talking (for tutorial).
	void PlayMeowSpeech2(); //Another version of the cat talking (for tutorial).
	void PlayMeowSound(); //Plays random meow. (There's only 2 though lol)
	void PlayCatDieSound();
	#endregion

	#region Dog Sound Players
	void PlayDogSound();
	void PlayDogDieSound();
	#endregion

	#region Other Sound Players
	void PlayTitle();
	void PlayWhoosh();
	void PlayAirplane();
	#endregion
}

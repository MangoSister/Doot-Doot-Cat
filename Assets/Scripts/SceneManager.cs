using UnityEngine;
using System.Collections;

public class SceneManager : GenericSingleton<SceneManager>
{
    private void OnLevelWasLoaded(int level)
    {
        FadeSwitch();
    }

    public SceneType CurrScene
    {
        get { return (SceneType)Application.loadedLevel; }
    }

    public void TransitScene(SceneType next)
    {
        StartCoroutine(TransitSceneCoroutine(next));
    }

    private IEnumerator TransitSceneCoroutine(SceneType next)
    {
        yield return new WaitForSeconds(FadeSwitch());
        Application.LoadLevel((int)next);
    }

    private float FadeSwitch()
    {
        ScreenFader fader = GetComponent<ScreenFader>();
        fader.fadeIn = !fader.fadeIn;
        return fader.fadeTime;
    }

    public enum SceneType
    {
        Start, Tutorial, Main, Credit, Lose,
    }
}

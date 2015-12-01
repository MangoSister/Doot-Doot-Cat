using UnityEngine;
using System.Collections;

public class TitleTimer : MonoBehaviour
{
    public float time;
	
	private void Start ()
    {
        StartCoroutine(TimerCoroutine());
	}

    private IEnumerator TimerCoroutine()
    {
        yield return new WaitForSeconds(time);
        SceneManager.Instance.TransitScene(SceneManager.SceneType.Tutorial);
    }
}

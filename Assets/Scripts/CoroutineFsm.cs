using UnityEngine;
using System.Collections;

public abstract class CoroutineFsm<T> : MonoBehaviour where T : FsmState
{
    protected T state;
    protected bool fsmActive;

	protected void StartFsm ()
    {
        StartCoroutine(MainCoroutine());	
	}

    private IEnumerator MainCoroutine()
    {
        while (true)
        {
            if (fsmActive)
                yield return StartCoroutine(state.stateCoroutine);
            else yield return null;
        }
    }
}

public interface FsmState
{
    string stateName { get; set; }
    string stateCoroutine { get; }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneTransition : MonoBehaviour
{
    public CirclesRow[] rows;

    private const int OFFSCREEN_X = 2000;
    public static SceneTransition instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(transform.parent);
        }
        else
        {
            Destroy(transform.parent.gameObject);
        }
    }

    public void Call(string scene)
    {
        Time.timeScale = 1;
        StopAllCoroutines();
        StartCoroutine(TransitionToScene(scene));
    }

    private IEnumerator TransitionToScene(string scene)
    {
        yield return TransitionIn();

        yield return new WaitForEndOfFrame();
        AsyncOperation async = SceneManager.LoadSceneAsync(scene);
        async.allowSceneActivation = false;
        while (async.progress < .9f)
        {
            yield return null;
        }
        async.allowSceneActivation = true;
        //EventSystem.current.enabled = false;

        yield return new WaitForSeconds(.2f);
        yield return TransitionOut();
        //EventSystem.current.enabled = true;
    }

    IEnumerator TransitionIn()
    {
        float time = .2f;
        yield return new WaitForSecondsRealtime(.2f);
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i].ShowRow(time);
            yield return new WaitForSeconds(time/2);
        }
    }

    IEnumerator TransitionOut()
    {
        float time = .2f;
        yield return new WaitForSecondsRealtime(.2f);
        for (int i = 0; i < rows.Length; i++)
        {
            rows[i].HideRow(time);
            yield return new WaitForSeconds(time/2);
        }
    }

    public static void CallScene(string scene)
    {
        SceneManager.LoadScene(scene);
    }
}

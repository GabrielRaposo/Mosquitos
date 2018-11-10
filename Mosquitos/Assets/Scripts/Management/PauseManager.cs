using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class PauseManager : MonoBehaviour {

    [Header("Base components")]
    public RawImage backgroundFade;
    public TextMeshProUGUI label;
    public GameObject buttonsPanel;
    public TextMeshProUGUI pauseButtonLabel;

    [System.Serializable]
    public struct ButtonLayout
    {
        public GameObject button;
        public TextMeshProUGUI description;
    }

    [Header("Buttons")]
    public ButtonLayout pauseButton;
    public ButtonLayout restartButton;
    public ButtonLayout exitButton;

    private SceneTransition transition;
    private EventSystem eventSystem;
    private bool isPaused;
	
	void Start ()
    {
        eventSystem = EventSystem.current;
        transition = SceneTransition.instance;
        AbsoluteToggle(false);
    }

    private void AbsoluteToggle(bool value)
    {
        label.enabled = value;
        Color color = backgroundFade.color;
        color.a = (value) ? .5f : 0f;
        backgroundFade.color = color;
        buttonsPanel.SetActive(value);
    }

    public void ToggleIsPaused()
    {
        isPaused = !isPaused;

        StopAllCoroutines();
        if (isPaused) {
            StartCoroutine(TransitionIn());
        } else {
            StartCoroutine(TransitionOut());
        }
    }

    private IEnumerator TransitionIn()
    {
        eventSystem.enabled = false;
        yield return new WaitForSecondsRealtime(.2f);

        RectTransform pauseRT = pauseButton.button.GetComponent<RectTransform>();
        RectTransform restartRT = restartButton.button.GetComponent<RectTransform>();
        RectTransform exitRT = exitButton.button.GetComponent<RectTransform>();

        restartRT.anchoredPosition = exitRT.anchoredPosition = pauseRT.anchoredPosition;
        buttonsPanel.SetActive(true);
        pauseButton.description.enabled = true;
        StartCoroutine(FadeIn());

        float movement = 10f;
        while(restartRT.anchoredPosition.y < 90)
        {
            yield return new WaitForEndOfFrame();
            restartRT.anchoredPosition += Vector2.up * movement;
            exitRT.anchoredPosition += Vector2.up * movement;
        }
        restartButton.description.enabled = true;

        while (exitRT.anchoredPosition.y < 170)
        {
            yield return new WaitForEndOfFrame();
            exitRT.anchoredPosition += Vector2.up * movement;
        }
        exitButton.description.enabled = true;
        pauseButtonLabel.text = "<";

        eventSystem.enabled = true;
    }

    private IEnumerator FadeIn()
    {
        float targetAlpha = .7f;
        Color color = backgroundFade.color;
        color.a = 0;
        backgroundFade.color = color;
        backgroundFade.enabled = true;

        while (backgroundFade.color.a < targetAlpha)
        {
            yield return new WaitForEndOfFrame();
            color.a += .2f;
            backgroundFade.color = color;
        }

        label.enabled = true;
        Time.timeScale = 0;
    }

    private IEnumerator TransitionOut()
    {
        eventSystem.enabled = false;
        yield return new WaitForSecondsRealtime(.1f);

        RectTransform pauseRT = pauseButton.button.GetComponent<RectTransform>();
        RectTransform restartRT = restartButton.button.GetComponent<RectTransform>();
        RectTransform exitRT = exitButton.button.GetComponent<RectTransform>();

        StartCoroutine(FadeOut());

        float movement = 10f;
        float targetPosition = pauseRT.anchoredPosition.y;

        exitButton.description.enabled = false;
        restartButton.description.enabled = false;
        pauseButton.description.enabled = false;

        while(restartRT.anchoredPosition.y > targetPosition)
        {
            yield return new WaitForEndOfFrame();
            restartRT.anchoredPosition += Vector2.down * movement;
            exitRT.anchoredPosition += Vector2.down * movement;
        }

        while (exitRT.anchoredPosition.y > targetPosition)
        {
            yield return new WaitForEndOfFrame();
            exitRT.anchoredPosition += Vector2.down * movement;
        }

        restartRT.anchoredPosition = exitRT.anchoredPosition = pauseRT.anchoredPosition;
        buttonsPanel.SetActive(false);
        pauseButtonLabel.text = "=";

        eventSystem.enabled = true;
    }

    private IEnumerator FadeOut()
    {
        label.enabled = false;

        float targetAlpha = 0f;
        Color color = backgroundFade.color;
        backgroundFade.color = color;

        while (backgroundFade.color.a > targetAlpha)
        {
            yield return new WaitForEndOfFrame();
            color.a -= .2f;
            backgroundFade.color = color;
        }

        backgroundFade.enabled = false;
        Time.timeScale = 1;
    }

    public void LoadScene(string scene)
    {
        if (transition) transition.Call(scene);
        else  SceneTransition.CallScene(scene);
    }
}

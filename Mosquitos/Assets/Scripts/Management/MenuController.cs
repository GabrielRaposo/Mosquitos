using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;

public class MenuController : MonoBehaviour
{
    [Header("References")]

    [SerializeField] EventSystem eventSystem;
    GameObject lastSelectedObject;

    [System.Serializable]
    private struct Menu
    {
        public RectTransform menuTransform;
        public string previousMenu;
    }

    [SerializeField] Menu currentMenu;
    int menuIndex = 0;

    [Header("Menus")]
    [SerializeField] private Menu titleMenu;
    [SerializeField] private Menu ageCheckMenu;
    [SerializeField] private Menu gameplayMenu;

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SwitchMenu(string menu)
    {
        int previousMenuIndex = menuIndex;
        switch (menu)
        {
            default:
            case "title":
                menuIndex = 0;
                StartCoroutine(TransitionPanels(
                    currentMenu.menuTransform, 
                    titleMenu.menuTransform, 
                    (menuIndex < previousMenuIndex) ? Vector3.down : Vector3.up));
                currentMenu = titleMenu;
                break;
            case "ageCheck":
                menuIndex = 1;
                StartCoroutine(TransitionPanels(
                    currentMenu.menuTransform, 
                    ageCheckMenu.menuTransform,
                    (menuIndex < previousMenuIndex) ? Vector3.down : Vector3.up));
                currentMenu = ageCheckMenu;
                break;
            case "gameplay":
                menuIndex = 99;
                StartCoroutine(TransitionPanels(
                    currentMenu.menuTransform,
                    gameplayMenu.menuTransform,
                    (menuIndex < previousMenuIndex) ? Vector3.down : Vector3.up));
                currentMenu = gameplayMenu;
                SceneTransition transition = SceneTransition.instance;
                if (transition)
                {
                    transition.Call("GameScene");
                }
                break;
        }
    }

    private IEnumerator TransitionPanels(RectTransform panelToHide, RectTransform panelToShow, Vector3 direction)
    {
        eventSystem.enabled = false;
        yield return new WaitForSeconds(.2f);

        float hiddenPosition = 500;
        panelToShow.localPosition = direction * hiddenPosition * -1;
        panelToShow.gameObject.SetActive(true);
        int maxSteps = 12;
        for(int i = 0; i < maxSteps; i++)
        {
            yield return new WaitForEndOfFrame();
            panelToHide.localPosition += direction * (hiddenPosition / maxSteps);
            panelToShow.localPosition += direction * (hiddenPosition / maxSteps);
        }
        panelToShow.localPosition = Vector2.zero;
        panelToHide.localPosition = direction * hiddenPosition;

        panelToHide.gameObject.SetActive(false);
        yield return new WaitForSeconds(.2f);
        eventSystem.enabled = true;
    }

    public void CallScene(string scene)
    {
        switch (scene)
        {
            default:
            case "play":
                SceneManager.LoadScene(scene);
                break;
        }
    }
}

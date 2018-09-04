using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    [Header("Managers Reference")]
    public Spawner spawner;

    [Header("UI")]
    public GameObject introLayout;

    enum State { Intro, Playing, Paused }
    State state;

	void Start ()
    {
        SetIntroState();
	}
	
	void SetIntroState()
    {
        if(introLayout) introLayout.SetActive(true);
        Time.timeScale = 0;
        state = State.Intro;
    }

    void SetPlayingState()
    {
        if (introLayout) introLayout.SetActive(false);
        Time.timeScale = 1;
        state = State.Playing;
    }

    void SetPausedState()
    {
        if (introLayout) introLayout.SetActive(true);
        Time.timeScale = 0;
        state = State.Paused;
    }

    public void IsGameplayEnabled(bool value)
    {
        switch (state)
        {
            case State.Intro:
                if (value)
                {
                    spawner.StartCoroutine(spawner.SpawnCicle());
                    SetPlayingState();
                }
            break;

            case State.Playing:
                if (!value) SetPausedState();    
            break;

            case State.Paused:
                if (value) SetPlayingState();
            break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    enum State { Intro, Playing, Paused }

    [Header("UI")]
    public GameObject introLayout;

    [Header("Gameplay Info")]
    public Spawner cicleManager;
    public Pattern[] stages;

    int stageIndex;
    Player player;
    State state;

	void Start ()
    {
        player = Player.instance;
        SetState(State.Intro);
	}
	
	void SetState(State state)
    {
        this.state = state;
        switch (state)
        {
            case State.Intro:
                if (introLayout) introLayout.SetActive(true);
                Time.timeScale = 0;
                break;

            case State.Playing:
                if (introLayout) introLayout.SetActive(false);
                Time.timeScale = 1;
                break;

            case State.Paused:
                if (introLayout) introLayout.SetActive(true);
                Time.timeScale = 0;
                break;
        }
    }

    public void CallStage()
    {
        if (stageIndex < stages.Length) {
            Pattern current = stages[stageIndex];

            cicleManager.SetStage(current);
            //stageTimer.StartCoroutine(stageTimer.Set(this, stages[stageIndex].duration));
        } else {
            //end game
            Debug.Log("FIM lol");
        }
        stageIndex++;
    }

    public void IsPlayerActive(bool value)
    {
        switch (state)
        {
            case State.Intro:
                if (value) {
                    CallStage();
                    SetState(State.Playing);
                }
                break;

            case State.Playing:
                if (!value) SetState(State.Paused);
                break;

            case State.Paused:
                if (value) SetState(State.Playing);
                break;
        }
    }
}

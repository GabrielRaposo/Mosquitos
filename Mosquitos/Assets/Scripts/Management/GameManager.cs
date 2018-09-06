using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    enum State { Intro, Playing, Paused }

    [Header("UI")]
    public GameObject introLayout;
    public StageTimer stageTimer;

    [Header("Gameplay Info")]
    public CicleManager cicleManager;
    public Stage[] stages;

    int stageIndex;
    State state;

	void Start ()
    {
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

    void CallStage()
    {
        //mostrar título
        if(stageIndex < stages.Length) {
            cicleManager.SetStage(stages[stageIndex]);
            stageTimer.StartCoroutine(stageTimer.Set(this, stages[stageIndex].duration));
        } else {
            //end game
            Debug.Log("FIM lol");
        }
        stageIndex++;
    }

    public void EndTimer()
    {
        //Interrompe spawn
        //Apresenta o repelente
        //-
        CallStage();
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

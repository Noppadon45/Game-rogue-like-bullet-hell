using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    //State of Game 
    public enum GameState
    {
        GamePlay,
        GamePause,
        GameOver
    }
    //Store Current State in Game
    public GameState stateCurrent;      //Store the stateCurrent

    public GameState stateBefore;       //Store the stateBefore

    [Header("UI")]
    public GameObject PauseScene;

    void Awake()
    {
        DisableScene();
    }

    void Update()
    {
        //Define the current State

        switch (stateCurrent) 
        {
            //State GamePlay
            case GameState.GamePlay:
                CheckState();
                break;

            //State GamePause
            case GameState.GamePause:
                CheckState();
                break;

            //State GameOver
            case GameState.GameOver:
                break;

            default:
                Debug.Log("State Error");
                break;
        }
        
    }

    public void StateChange(GameState StateChange)
    {
        stateCurrent = StateChange;
    }

    public void PauseGame()
    {
        if (stateCurrent != GameState.GamePause)
        {
            stateBefore = stateCurrent;
            StateChange(GameState.GamePause);
            Time.timeScale = 0f;                    //Stop time in Game
            PauseScene.SetActive(true);             //Show PauseScene
            Debug.Log("PauseGame");
        }
    }

    public void ResumeGame()
    {
        if (stateCurrent == GameState.GamePause)
        {
            StateChange(stateBefore);
            Time.timeScale = 1f;                    //Start time in Game
            PauseScene.SetActive(false);            //Stop show PauseScene
            Debug.Log("ResumeGame");
        }
    }

    void CheckState()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (stateCurrent == GameState.GamePause)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    void DisableScene()
    {
        PauseScene.SetActive(false);
    }
    
}

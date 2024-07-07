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
    public GameState stateCurrent;

    void Update()
    {
        //Define the current State

        switch (stateCurrent) 
        {
            //State GamePlay
            case GameState.GamePlay:
                break;

            //State GamePause
            case GameState.GamePause:
                break;

            //State GameOver
            case GameState.GameOver:
                break;

            default:
                Debug.Log("State Error");
                break;
        }
        TestState();
    }

    void TestState()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            stateCurrent++;
            Debug.Log("E Was Pressed");
        }else if (Input.GetKeyDown(KeyCode.Q))
        {
            stateCurrent--;
            Debug.Log("Q Was Pressed");
        }
    }
}

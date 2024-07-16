using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

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

    [Header("Scenes")]
    public GameObject PauseScene;
    public GameObject ResultScene;

    //Current Stats Display
    [Header("Current Stats Display")]
    public Text CurrentHealthDisplay;
    public Text CurrentRecoveryDisplay;
    public Text CurrentMoveSpeedDisplay;
    public Text CurrentMightDisplay;
    public Text CurrentProjectileSpeedDisplay;
    public Text CurrentMagnetDisplay;

    [Header("Result Scene")]
    public Image CharacterImage;
    public Text CharacterName;

    //Check IsGameOver or not
    public bool IsGameOver = false;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }else
        {
            Debug.LogWarning("Extra" + this + "Delected");
            Destroy(gameObject);
        }

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
                if (!IsGameOver)
                {
                    IsGameOver = true;
                    Time.timeScale = 0f; //Stop game when GameOver
                    Debug.Log("State GameOver");
                    ResultDisplay();
                }
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
        ResultScene.SetActive(false);
    }

    public void GameOver()
    {
        StateChange(GameState.GameOver);    
    }

    void ResultDisplay()
    {
        ResultScene.SetActive(true);
    }

    public void AssignChooseCharacter(CharacterSciptableObject CharacterData) 
    {
        CharacterImage.sprite = CharacterData.Icon;
        CharacterName.text = CharacterData.Name;
    }
    
}

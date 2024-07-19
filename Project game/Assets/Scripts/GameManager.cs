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
    public Text LevelDisplay;
    public List<Image> WeaponImage = new List<Image>(6);
    public List<Image> PassiveImage = new List<Image>(6);
    

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

    public void AssignLevelPlayer(int LevelData)
    {
        LevelDisplay.text = LevelData.ToString();
    }

    public void AssignWeaponandPassiveImage(List <Image> WeaponImagesData , List<Image> PassiveImagesData)
    {
        if (WeaponImagesData.Count != WeaponImage.Count || PassiveImagesData.Count != PassiveImage.Count) 
        {
            Debug.Log("WeaponandPassive are different list of Image ");
            return;
        }

        //Assign Weapon
        for (int i = 0; i < WeaponImage.Count; i++) 
        {
            //Check Weapon Sprite that is not null
            if (WeaponImagesData[i].sprite)
            {
                //Enable list that Weapon Image and put WeaponData sprite to Weapon Image 
                WeaponImage[i].enabled = true;
                WeaponImage[i].sprite = WeaponImagesData[i].sprite;
            }
            else
            {
                //If Weapon Sprite is null Disble WeaponImage
                WeaponImage[i].enabled = false;
            }
        }

        //Assign Passive
        for (int i = 0; i < PassiveImage.Count; i++)
        {
            //Check Passive Sprite that is not null
            if (PassiveImagesData[i].sprite)
            {
                //Enable list that Passive Image and put PassiveData sprite to Passive Image 
                PassiveImage[i].enabled = true;
                PassiveImage[i].sprite = PassiveImagesData[i].sprite;
            }
            else
            {
                //If Passive Sprite is null Disble PassiveImage
                PassiveImage[i].enabled = false;
            }
        }
    }

}

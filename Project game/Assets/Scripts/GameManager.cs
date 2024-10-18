using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.U2D.Animation;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    //State of Game 
    public enum GameState
    {
        GamePlay,
        GamePause,
        GameOver,
        LevelUP
    }
    //Store Current State in Game
    public GameState stateCurrent;      //Store the stateCurrent

    public GameState stateBefore;       //Store the stateBefore

    [Header("Damage Text Pop")]
    public Canvas DamageText;
    public float Textsize = 20;
    public TMP_FontAsset textfont;
    public Camera RefCamera;

    [Header("Scenes")]
    public GameObject PauseScene;
    public GameObject ResultScene;
    public GameObject LevelUPScene;

    //Current Stats Display
    [Header("Current Stats Display")]
    public TMP_Text CurrentHealthDisplay;
    public TMP_Text CurrentRecoveryDisplay;
    public TMP_Text CurrentMoveSpeedDisplay;
    public TMP_Text CurrentMightDisplay;
    public TMP_Text CurrentProjectileSpeedDisplay;
    public TMP_Text CurrentMagnetDisplay;

    [Header("Result Scene")]
    public Image CharacterImage;
    public TMP_Text CharacterName;
    public TMP_Text LevelDisplay;
    public TMP_Text TimeSurviveDisplay;
    public List<Image> WeaponImage = new List<Image>(6);
    public List<Image> PassiveImage = new List<Image>(6);

    [Header("StopWatch")]
    float StopWatchTime;     //Current Time in Game
    public float TimeLimit;     //Time limit in each second
    public TMPro.TMP_Text StopWatchDisplay;       //Show StopWatch Time in Game

   

    //Check IsGameOver or not
    public bool IsGameOver = false;

    //Check IsLevelUP or not
    public bool IsLevelUP = false;

    //Reference to PlayerObject
    public GameObject PlayerObject;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
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
                StopWatchTimeUpdate();
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
            //State LevelUP
            case GameState.LevelUP:
                if (!IsLevelUP)
                {
                    IsLevelUP = true;
                    Time.timeScale = 0f;   //Stop game when LevelUP
                    Debug.Log("LevelUPScene");
                    LevelUPScene.SetActive(true);
                }
                break;

            default:
                Debug.Log("State Error");
                break;
        }

    }
    
    IEnumerator DamagePopUpCoroutine(string text, Transform target, float duration = 1f , float speed = 50f)
    {
        //Generate text

        GameObject textObject = new GameObject("DamageText");

        RectTransform RectT = textObject.AddComponent<RectTransform>();
        TextMeshProUGUI TextMP = textObject.AddComponent<TextMeshProUGUI>();
        TextMP.text = text;
        TextMP.horizontalAlignment = HorizontalAlignmentOptions.Center;
        TextMP.verticalAlignment = VerticalAlignmentOptions.Middle;
        TextMP.fontSize = Textsize;
        if (textfont) TextMP.font = textfont;
        
        RectT.position = RefCamera.WorldToScreenPoint(target.position);

        //Destroy Text when out of duration
        Destroy(textObject, duration);

        //Generate text object to canvas
        textObject.transform.SetParent(instance.DamageText.transform);
        textObject.transform.SetAsFirstSibling();
        

        //Function PopUp text and Fade Text when Popup overtime
        WaitForEndOfFrame f = new WaitForEndOfFrame();
        float t = 0;
        float PositonY = 0;
        while (t < duration)
        {
            //Wait for fream
            yield return f;
            t += Time.deltaTime;

            //Fade Text When PopUp
            TextMP.color = new Color(TextMP.color.r, TextMP.color.g , TextMP.color.b, 1 - t / duration);

            //PopUp Text
            PositonY += speed * Time.deltaTime;
            if (RectT.position != null)
            {
                RectT.position = RefCamera.WorldToScreenPoint(target.position + new Vector3(0, PositonY));
                
            }
            
        }
     

    }

    public static void DamagePopUp(string text, Transform target, float duration = 1f, float speed = 1f)
    {
        if (!instance.DamageText) return;

        if (!instance.RefCamera) instance.RefCamera = Camera.main;


        instance.StartCoroutine(instance.DamagePopUpCoroutine(
            text, target, duration, speed
            ));
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
        LevelUPScene.SetActive(false);
    }

    public void GameOver()
    {
        TimeSurviveDisplay.text = StopWatchDisplay.text;

        StateChange(GameState.GameOver);    
    }

    void ResultDisplay()
    {
        ResultScene.SetActive(true);
    }

    public void AssignChooseCharacter(CharacterData CharacterData) 
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

    void StopWatchTimeUpdate()
    {
        StopWatchTime += Time.deltaTime;

        StopWatchTimeDisplay();

        if (StopWatchTime > TimeLimit) 
        {
            PlayerObject.SendMessage("Kill");

        }
    }

    void StopWatchTimeDisplay()
    {
        //Calulate Time in Each Game
        int Minutes = Mathf.FloorToInt(StopWatchTime / 60);
        int Second = Mathf.FloorToInt(StopWatchTime % 60);

        //Display Time in Each Game
        StopWatchDisplay.text = string.Format("{0:00}:{1:00}", Minutes , Second);
    }

    public void LevelUPStart()
    {
        StateChange(GameState.LevelUP);     //Change State
        PlayerObject.SendMessage("RemoveandApplyUpgrade");
    }

    public void LevelUPEnd()
    {
        IsLevelUP = false;      
        Time.timeScale = 1;     //Resume Time in Game
        LevelUPScene.SetActive(false);
        StateChange(GameState.GamePlay);
    }

}

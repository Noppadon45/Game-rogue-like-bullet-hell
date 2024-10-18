using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PlayerStats : MonoBehaviour
{
    CharacterSciptableObject CharacterData;
    //Current Stats Player
    float currentHealth; 
    float currentRecovery; 
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;
    float currentMagnet;


    #region Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }
        set
        {
            //When value has changed
            if (currentHealth != value)
            {
                currentHealth = value;      //Update the value real time
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentHealthDisplay.text = "Health " + currentHealth;
                }
                //Add addition
            }
        }
    }

    public float CurrentRecovery
    {
        get { return currentRecovery; }
        set
        {
            //When value has changed
            if (currentRecovery != value)
            {
                currentRecovery = value;      //Update the value real time
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentRecoveryDisplay.text = "Recovery " + currentRecovery;
                }
                //Add addition
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return currentMoveSpeed; }
        set
        {
            //When value has changed
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;      //Update the value real time
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentMoveSpeedDisplay.text = "MoveSpeed " + currentMoveSpeed;
                }
                //Add addition
            }
        }
    }

    public float CurrentMight
    {
        get { return currentMight; }
        set
        {
            //When value has changed
            if (currentMight != value)
            {
                currentMight = value;      //Update the value real time
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentMightDisplay.text = "Might " + currentMight;
                }
                //Add addition
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return currentProjectileSpeed; }
        set
        {
            //When value has changed
            if (currentProjectileSpeed != value)
            {
                currentProjectileSpeed = value;      //Update the value real time
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentProjectileSpeedDisplay.text = "ProjectileSpeed " + currentProjectileSpeed;
                }
                //Add addition
            }
        }
    }

    public float CurrentMagnet
    {
        get { return currentMagnet; }
        set
        {
            //When value has changed
            if (currentMagnet != value)
            {
                currentMagnet = value;      //Update the value real time
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentMagnetDisplay.text = "Magnet " + currentMight;
                }
                //Add addition
            }
        }
    }

    #endregion

    public ParticleSystem EffectgotDamage;


    //Experience Level Player
    [Header("Experience/Level")]
    public int experience = 0;
    public int level = 1;
    public int experienceCap;

    //Class for define a level range 
    [System.Serializable]
    public class LevelRange
    {
        public int startLevel;
        public int endLevel;
        public int experienceCapIncrease;
    }


    //I-Freams
    [Header("Ifreams")]
    public float IfreamDuration;
    float IfreamTimer;
    bool Isifream;

    public List<LevelRange> levelRanges;

    Inventory inventory;
    public int WeaponIndex;
    public int PassiveIndex;

    [Header("UI")]
    public Image HealthBar;
    public Image ExperienceBar;
    public TMP_Text LevelDisplay;

    public GameObject secondweapon;
    public GameObject firstpassive, secondpasssive;


    void Start()
    {

        // initialize experience cap the first experience cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;

        //Set Current Stats Display
        GameManager.instance.CurrentHealthDisplay.text = "currentHealth " + currentHealth;
        GameManager.instance.CurrentRecoveryDisplay.text = "currentRecovery " + currentRecovery;
        GameManager.instance.CurrentMoveSpeedDisplay.text = "currentMoveSpeed " + currentMoveSpeed;
        GameManager.instance.CurrentMightDisplay.text = "currentMight " + currentMight;
        GameManager.instance.CurrentProjectileSpeedDisplay.text = "ProjectileSpeed " + currentProjectileSpeed;
        GameManager.instance.CurrentMagnetDisplay.text = "Magnet " + currentMagnet;

        GameManager.instance.AssignChooseCharacter(CharacterData);

        UpdateHealthBar();
        UpdateExperienceBar();
        UpdateLevelUP();
    }

    void Update()
    {
        if (IfreamTimer > 0) 
        {
            IfreamTimer = IfreamTimer - Time.deltaTime;
        }
        //If IfreamTimer < 0 Ifream false
        else if (Isifream)
        {
            Isifream = false;
        }
        Recoverheal();

        UpdateHealthBar();
    }
    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelupCheck();

        UpdateExperienceBar();
    }

    void LevelupCheck()
    {
        if (experience >= experienceCap) 
        {
            level++;
            experience = experience - experienceCap;
            int experienceCapIncrease = 0; 
            foreach (LevelRange range in levelRanges)
            {
                if(level >= range.startLevel && level <= range.endLevel)
                {
                    experienceCapIncrease = range.experienceCapIncrease;
                    break;
                }
            }
            experienceCap += experienceCapIncrease;

            UpdateLevelUP();

            GameManager.instance.LevelUPStart();
        }
    }

    void Awake()
    {

        CharacterData = CharacterSelect.GetData();
        CharacterSelect.instance.DestroySingleton();

        inventory = GetComponent<Inventory>();

        //Assgin Variable
        CurrentHealth = CharacterData.MaxHealth;
        CurrentRecovery = CharacterData.Recovery;
        CurrentMoveSpeed = CharacterData.MoveSpeed;
        CurrentMight = CharacterData.Might;
        CurrentProjectileSpeed = CharacterData.ProjectileSpeed;
        CurrentMagnet = CharacterData.Magnet;

        //Spawn the start weapon
        SpawnWeapon(CharacterData.StartingWeapon);

        //SpawnPassiveItem(firstpassive);
        //SpawnPassiveItem(secondpasssive);
        //SpawnWeapon(secondweapon);

        
    }
    

    public void takeDamage(float dmg)
    {
        if (!Isifream)
        {
            CurrentHealth -= dmg;

            //if player takeDamage play partical Effect got Damage
            if (EffectgotDamage)
                Instantiate(EffectgotDamage, transform.position, Quaternion.identity);

            IfreamTimer = IfreamDuration;
            Isifream = true;
            if (CurrentHealth <= 0)
            {
                Kill();
            }

            UpdateHealthBar();
        }
        else
        {

        }
    }

    void UpdateHealthBar()
    {
        HealthBar.fillAmount = currentHealth / CharacterData.MaxHealth;
    }

    void UpdateExperienceBar()
    {
        ExperienceBar.fillAmount = (float)experience / experienceCap;       //Cal Experience and fill in Experience Bar
    }

    void UpdateLevelUP()
    {
        LevelDisplay.text = "Lv " + level.ToString();       //Update Lv in top right in game
    }

    public void Kill()
    {
        //GameOver Scene
        if (!GameManager.instance.IsGameOver) 
        {
            GameManager.instance.AssignLevelPlayer(level);
            GameManager.instance.AssignWeaponandPassiveImage(inventory.WeaponUi , inventory.PassiveUi);
            GameManager.instance.GameOver();
        }
        Debug.Log("Player is Dead");
    }

    public void Heal(float amount)
    {
        //Heal when health < maxHealth
        if (CurrentHealth < CharacterData.MaxHealth)
        {
            CurrentHealth += amount;
            //dont heal when Health >= maxHealth
            if (CurrentHealth > CharacterData.MaxHealth)
            {
                CurrentHealth = CharacterData.MaxHealth;
            }
        } 
    }

    void Recoverheal()
    {
        if (CurrentHealth < CharacterData.MaxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;

            if (CurrentHealth > CharacterData.MaxHealth)
            {
                CurrentHealth = CharacterData.MaxHealth;
            }
        }
        
    }

    public void SpawnWeapon(GameObject weapon)
    {
        if (WeaponIndex >= inventory.WeaponSlots.Count -1)
        {
            Debug.Log("Inventory Full");
            return;
        }
        //Spawn the staring weapon
        GameObject spawnedWeapon = Instantiate (weapon , transform.position , Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);   //Set Weapon to be child  of player
        inventory.AddWeapon(WeaponIndex, spawnedWeapon.GetComponent<WeaponsController>());

        WeaponIndex++;
    }

    public void SpawnPassiveItem(GameObject Passive)
    {
        if (WeaponIndex >= inventory.PassiveSlots.Count - 1)
        {
            Debug.Log("Inventory Full");
            return;
        }
        //Spawn the staring Passive
        GameObject spawnPassive = Instantiate(Passive, transform.position, Quaternion.identity);
        spawnPassive.transform.SetParent(transform);   //Set Weapon to be child  of player
        inventory.AddPassive(PassiveIndex, spawnPassive.GetComponent<Passiveitem>());

        PassiveIndex++;
    }

    
}



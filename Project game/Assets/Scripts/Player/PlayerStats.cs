using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEditor.Build.Player;


public class PlayerStats : MonoBehaviour
{
    CharacterData characterData;        
    public CharacterData.Stats baseStats;       //Its use in CalculateStats() because It will calculate with base of baseStats of playerdata
    [SerializeField]
    CharacterData.Stats actualstats;        //This stats is actual stats use in game

    float currentHealth;

    #region Stats Properties
    public float CurrentHealth
    {
        get { return currentHealth; }

        //If player pause sceen , player health will also update
        set
        {
            //Check if the value has changed
            if (currentHealth != value)
            {
                currentHealth = value;      
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentHealthDisplay.text = string.Format("Health : {0} / {1}" , currentHealth, actualstats.maxHealth);
                }

            }
        }
    }


    public float maxHealth
    {
        get { return actualstats.maxHealth; }

        //If player pause sceen , player health will also update
        set
        {
            //Check if the value has changed
            if (actualstats.maxHealth != value)
            {
                actualstats.maxHealth = value;      
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentHealthDisplay.text = string.Format("Health : {0} / {1}", currentHealth, actualstats.maxHealth);
                }
                //Update the real time value of the stats

            }
        }
    }


   
    public float CurrentRecovery
    {
        get { return Recovery; }
        set { Recovery = value; }
    }
    public float Recovery
    {
        get { return actualstats.recovery; }
        set
        {
            //Check if the value has changed
            if (actualstats.recovery != value)
            {
                actualstats.recovery = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentRecoveryDisplay.text = "Recovery: " + actualstats.recovery;
                }
            }
        }
    }

    public float CurrentMoveSpeed
    {
        get { return MoveSpeed; }
        set { MoveSpeed = value; }
    }

    public float MoveSpeed
    {
        get { return actualstats.moveSpeed; }
        set
        {
            //Check if the value has changed
            if (actualstats.moveSpeed != value)
            {
                actualstats.moveSpeed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentMoveSpeedDisplay.text = "MoveSpeed: " + actualstats.moveSpeed;
                }
            }

        }
    }

    public float CurrentMight
    {
        get { return Might; }
        set { Might = value; }
    }

    public float Might
    {
        get { return actualstats.might; }
        set
        {
            //Check if the value has changed
            if (actualstats.might != value)
            {
                actualstats.might = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentMightDisplay.text = "Might: " + actualstats.might;
                }
            }
        }
    }

    public float CurrentProjectileSpeed
    {
        get { return Speed; }
        set { Speed = value; }
    }

    public float Speed
    {
        get { return actualstats.speed; }
        set
        {
            //Check if the value has changed
            if (actualstats.speed != value)
            {
                actualstats.speed = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentProjectileSpeedDisplay.text = "ProjectileSpeed: " + actualstats.speed;
                }
            }
        }
    }

    public float CurrentMagnet
    {
        get { return Magnet; }
        set { Magnet = value; }
    }

    public float Magnet
    {
        get { return actualstats.magnet; }
        set
        {
            if (actualstats.magnet != value)
            {
                //Check if the value has changed
                actualstats.magnet = value;
                if (GameManager.instance != null)
                {
                    GameManager.instance.CurrentMagnetDisplay.text = "Magnet : " + actualstats.magnet;
                }
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

    PlayerInventory playerInventory;
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

        playerInventory.Add(characterData.StartingWeapon);


        // initialize experience cap the first experience cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;

        //Set Current Stats Display
        GameManager.instance.CurrentHealthDisplay.text = "currentHealth " + currentHealth;
        GameManager.instance.CurrentRecoveryDisplay.text = "currentRecovery " + CurrentRecovery;
        GameManager.instance.CurrentMoveSpeedDisplay.text = "currentMoveSpeed " + CurrentMoveSpeed;
        GameManager.instance.CurrentMightDisplay.text = "currentMight " + CurrentMagnet;
        GameManager.instance.CurrentProjectileSpeedDisplay.text = "ProjectileSpeed " + CurrentProjectileSpeed;
        GameManager.instance.CurrentMagnetDisplay.text = "Magnet " + CurrentMagnet;

        GameManager.instance.AssignChooseCharacter(characterData);

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

    public void CalculateStats()
    {
        actualstats = baseStats;
        foreach (PlayerInventory.Slot s in playerInventory.PassiveSlot)
        {
            Passive p = s.item as Passive;
            if (p)
            {
                actualstats += p.GetBoosts();
            }
        }
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

        characterData = CharacterSelect.GetData();
        CharacterSelect.instance.DestroySingleton();

        playerInventory = GetComponent<PlayerInventory>();
        
        //Assign the variable
        baseStats = actualstats = characterData.stats;
        currentHealth = actualstats.maxHealth;
        
        
    }
    

    public void takeDamage(float dmg)
    {
        if (!Isifream)
        {
            CurrentHealth -= dmg;

            //if player takeDamage play partical Effect got Damage
            if (EffectgotDamage)
            {
                Destroy(Instantiate(EffectgotDamage, transform.position, Quaternion.identity), 5f);
            }
                

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
        //Upgrade the health bar
        HealthBar.fillAmount = currentHealth / actualstats.maxHealth;
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
   
            GameManager.instance.GameOver();
        }
        Debug.Log("Player is Dead");
    }

    public void Heal(float amount)
    {
        //Heal when health < maxHealth
        if (CurrentHealth < actualstats.maxHealth)
        {
            CurrentHealth += amount;
            //dont heal when Health >= maxHealth
            if (CurrentHealth > actualstats.maxHealth)
            {
                CurrentHealth = actualstats.maxHealth;
            }
        } 
    }

    void Recoverheal()
    {
        if (CurrentHealth < actualstats.maxHealth)
        {
            CurrentHealth += CurrentRecovery * Time.deltaTime;

            //If currenthealth > actual maxhealth use actual maxhealth stats
            if (CurrentHealth > actualstats.maxHealth)
            {
                CurrentHealth = actualstats.maxHealth;
            }
        }
        
    }

    [System.Obsolete("Old function")]
    public void SpawnWeapon(GameObject weapon)
    {
        if (WeaponIndex >= playerInventory.WeaponSlot.Count - 1)
        {
            Debug.Log("Inventory Full");
            return;
        }
        //Spawn the staring weapon
        GameObject spawnedWeapon = Instantiate (weapon , transform.position , Quaternion.identity);
        spawnedWeapon.transform.SetParent(transform);   //Set Weapon to be child  of player

        //inventory.AddWeapon(WeaponIndex, spawnedWeapon.GetComponent<WeaponsController>());

        WeaponIndex++;
    }

    [System.Obsolete("Old function")]
    public void SpawnPassiveItem(GameObject Passive)
    {
        if (PassiveIndex >= playerInventory.PassiveSlot.Count - 1)
        {
            Debug.Log("Inventory Full");
            return;
        }
        //Spawn the staring Passive
        GameObject spawnPassive = Instantiate(Passive, transform.position, Quaternion.identity);
        spawnPassive.transform.SetParent(transform);   //Set Weapon to be child  of player
        //inventory.AddPassive(PassiveIndex, spawnPassive.GetComponent<Passiveitem>());

        PassiveIndex++;
    }

    
}



using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    CharacterSciptableObject CharacterData;
    //Current Stats Player
    [HideInInspector]
    public float currentHealth;
    [HideInInspector]
    public float currentRecovery;
    [HideInInspector]
    public float currentMoveSpeed;
    [HideInInspector]
    public float currentMight;
    [HideInInspector]
    public float currentProjectileSpeed;
    [HideInInspector]
    public float currentMagnet;



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

    public GameObject secondweapon;
    public GameObject firstpassive, secondpasssive;


    void Start()
    {

        // initialize experience cap the first experience cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;   
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
    }
    public void IncreaseExperience(int amount)
    {
        experience += amount;
        LevelupCheck();
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
        }
    }

    void Awake()
    {

        CharacterData = CharacterSelect.GetData();
        CharacterSelect.instance.DestroySingleton();

        inventory = GetComponent<Inventory>();

        //Assgin Variable
        currentHealth = CharacterData.MaxHealth;
        currentRecovery = CharacterData.Recovery;
        currentMoveSpeed = CharacterData.MoveSpeed;
        currentMight = CharacterData.Might;
        currentProjectileSpeed = CharacterData.ProjectileSpeed;
        currentMagnet = CharacterData.Magnet;

        //Spawn the start weapon
        SpawnWeapon(CharacterData.StartingWeapon);

        SpawnPassiveItem(firstpassive);
        SpawnPassiveItem(secondpasssive);
        SpawnWeapon(secondweapon);

    }
    

    public void takeDamage(float dmg)
    {
        if (!Isifream)
        {
            currentHealth -= dmg;

            IfreamTimer = IfreamDuration;
            Isifream = true;
            if (currentHealth <= 0)
            {
                Kill();
            }
        }
        else
        {

        }
    } 

    public void Kill()
    {
        Debug.Log("Player is Dead");
    }

    public void Heal(float amount)
    {
        //Heal when health < maxHealth
        if (currentHealth < CharacterData.MaxHealth)
        {
            currentHealth += amount;
            //dont heal when Health >= maxHealth
            if (currentHealth > CharacterData.MaxHealth)
            {
                currentHealth = CharacterData.MaxHealth;
            }
        } 
    }

    void Recoverheal()
    {
        if (currentHealth < CharacterData.MaxHealth)
        {
            currentHealth += currentRecovery * Time.deltaTime;
        }
        else if (currentHealth > CharacterData.MaxHealth)
        {
            currentHealth = CharacterData.MaxHealth;
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

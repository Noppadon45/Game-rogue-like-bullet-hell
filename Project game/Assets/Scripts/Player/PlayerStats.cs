using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public CharacterSciptableObject CharacterData;
    //Current Stats Player
    float currentHealth;
    float currentRecovery;
    float currentMoveSpeed;
    float currentMight;
    float currentProjectileSpeed;

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

    void Start()
    {
        // initialize experience cap the first experience cap increase
        experienceCap = levelRanges[0].experienceCapIncrease;   
    }

    private void Update()
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
        currentHealth = CharacterData.MaxHealth;
        currentRecovery = CharacterData.Recovery;
        currentMoveSpeed = CharacterData.MoveSpeed;
        currentMight = CharacterData.Might;
        currentProjectileSpeed = CharacterData.ProjectileSpeed;
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
}

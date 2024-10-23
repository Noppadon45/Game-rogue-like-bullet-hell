using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Weapon Data", menuName = "WeaponData")]

public class WeaponData : ItemData
{

    [HideInInspector]
    public string behaviour;
    
    public Weapon.Stats baseStats;
    public Weapon.Stats[] linearGrown;
    public Weapon.Stats[] randomGrown;

    public Weapon.Stats GetLevelData(int level) 
    {
        //Pick LinearGrown Level
        if (level - 2 < linearGrown.Length)
        {
            return linearGrown[level - 2];
        }

        //Pick RandomGrown Level
        if (randomGrown.Length > 0)
        {
            return randomGrown[Random.Range(0, linearGrown.Length)];
        }

        Debug.LogWarning(string.Format("Weapon dont have level up stats config for level" , level));

        return new Weapon.Stats();
    }
}

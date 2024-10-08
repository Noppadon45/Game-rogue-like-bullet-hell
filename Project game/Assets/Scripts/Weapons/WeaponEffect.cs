using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This script will call when spawn gameobject like projectile , aura 
public abstract class WeaponEffect : MonoBehaviour
{
    [HideInInspector] public PlayerStats player;
    [HideInInspector] public Weapon weapon;

    public float GetDamage()
    {
        return weapon.GetDamage();
    }
} 

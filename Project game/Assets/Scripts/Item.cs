using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public int currentLevel = 1;
    public int maxLevel = 1;

    protected PlayerStats playerStats;

    public virtual void Initialise(ItemData data)
    {
        maxLevel = data.maxLevel;
        playerStats = FindObjectOfType<PlayerStats>();
    }

    public virtual bool CanLevelUp()
    {
        return currentLevel <= maxLevel;
    }

    public virtual bool DoLevelUp()
    {
        return true;
    }

    public virtual void OnEquip()
    {

    }

    public virtual void OnUnEquip()
    {

    }

    internal void Initialise(WeaponData data)
    {
        throw new NotImplementedException();
    }
}

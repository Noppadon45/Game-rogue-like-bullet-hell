using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<WeaponsController> WeaponSlots = new List<WeaponsController>(6);
    public int[] WeaponLevels = new int[6];
    public List<Passiveitem> PassiveSlots = new List<Passiveitem>(6);
    public int[] PassiveLevels = new int[6];

    public void AddWeapon (int Index , WeaponsController Weapon )   //Add Weapon to Inventory
    {
        WeaponSlots[Index] = Weapon;
        WeaponLevels[Index] = Weapon.WeaponData.Level;
    }
    public void AddPassive (int Index , Passiveitem Passive )   //Add Passive to Inventory
    {
        PassiveSlots[Index] = Passive;
        PassiveLevels[Index] = Passive.PassiveData.Level;
    }

    public void LevelUpWeapon (int Index)
    {
        if (WeaponSlots.Count > Index)
        {
                
        }
    }
    public void LevelUpPassive (int Index) 
    {
        

    }
}

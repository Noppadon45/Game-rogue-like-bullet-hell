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
            WeaponsController Weapon = WeaponSlots[Index];
            GameObject UpgradeWeapon = Instantiate(Weapon.WeaponData.NextLevelPrefab, transform.position, Quaternion.identity);
            UpgradeWeapon.transform.SetParent(transform);       //Set Weapon to child of Player
            AddWeapon(Index, UpgradeWeapon.GetComponent<WeaponsController>());
            Destroy(Weapon.gameObject);
            WeaponLevels[Index] = UpgradeWeapon.GetComponent<WeaponsController>().WeaponData.Level;     //Check correct Levelup Weapon
        }
    }
    public void LevelUpPassive (int Index) 
    {
        if (PassiveSlots.Count > Index)
        {
            Passiveitem Passive = PassiveSlots[Index];
            GameObject UpgradePassive = Instantiate(Passive.PassiveData.NextLevelPrefab, transform.position, Quaternion.identity);
            UpgradePassive.transform.SetParent(transform);       //Set Weapon to child of Player
            AddPassive(Index, UpgradePassive.GetComponent<Passiveitem>());
            Destroy(Passive.gameObject);
            PassiveLevels[Index] = UpgradePassive.GetComponent<Passiveitem>().PassiveData.Level;     //Check correct Levelup Passive
        }
    }
}

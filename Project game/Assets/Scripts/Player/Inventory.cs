using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<WeaponsController> WeaponSlots = new List<WeaponsController>(6);
    public int[] WeaponLevels = new int[6];
    public List<Image> WeaponUi = new List<Image>(6);
    public List<Passiveitem> PassiveSlots = new List<Passiveitem>(6);
    public int[] PassiveLevels = new int[6];
    public List<Image> PassiveUi = new List<Image>(6);

    public void AddWeapon (int Index , WeaponsController Weapon )   //Add Weapon to Inventory
    {
        WeaponSlots[Index] = Weapon;
        WeaponLevels[Index] = Weapon.WeaponData.Level;
        WeaponUi[Index].enabled = true;                     //Enable  the image Conponent
        WeaponUi[Index].sprite = Weapon.WeaponData.icon;
    }
    public void AddPassive (int Index , Passiveitem Passive )   //Add Passive to Inventory
    {
        PassiveSlots[Index] = Passive;
        PassiveLevels[Index] = Passive.PassiveData.Level;
        PassiveUi[Index].enabled = true;                     //Enable  the image Conponent
        PassiveUi[Index].sprite = Passive.PassiveData.icon;
    }

    public void LevelUpWeapon (int Index)
    {
        if (WeaponSlots.Count > Index)
        {
            WeaponsController Weapon = WeaponSlots[Index];
            if (!Weapon.WeaponData.NextLevelPrefab)         //If No NextLevelPrefab Weapon
            {
                Debug.Log("No Next Level For" + Weapon.name);
                return;

            }
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
            if (!Passive.PassiveData.NextLevelPrefab)         //If No NextLevelPrefab Passive
            {
                Debug.Log("No Next Level For" + Passive.name);
                return;

            }
            GameObject UpgradePassive = Instantiate(Passive.PassiveData.NextLevelPrefab, transform.position, Quaternion.identity);
            UpgradePassive.transform.SetParent(transform);       //Set Weapon to child of Player
            AddPassive(Index, UpgradePassive.GetComponent<Passiveitem>());
            Destroy(Passive.gameObject);
            PassiveLevels[Index] = UpgradePassive.GetComponent<Passiveitem>().PassiveData.Level;     //Check correct Levelup Passive
        }
    }
}
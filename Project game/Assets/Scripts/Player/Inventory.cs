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

    [System.Serializable]
    public class WeaponUpgrade
    {
        public GameObject InitWeapon;
        public WeaponScriptableObject WeaponData;
    }
    [System.Serializable]
    public class PassiveUpgrade
    {
        public GameObject InitPassive;
        public PassiveitemScriptableScript PassiveData;
    }
    [System.Serializable]
    public class UpgradeUI
    {
        public Text UpgradeItemDisplay;
        public Text DescriptionItemDisplay;
        public Image UpgradeImageDisplay;
        public Button UpgradeButton;
    }

    public List<WeaponUpgrade> WeaponUpgradesOptions = new List<WeaponUpgrade>();   //List WeaponUpgrade Option for Weapon
    public List<PassiveUpgrade> PassiveUpgradesOptions = new List<PassiveUpgrade>();    //List PassiveUpgrade Option for Passive
    public List<UpgradeUI> UpgradeUIOptions = new List<UpgradeUI>();    //List UpgradeUI Option in scene

    PlayerStats Player;

    void Start()
    {
        Player = GetComponent<PlayerStats>();
    }


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

    void ApplyUpgradeOption()
    {
        foreach (var UpgradeOption in UpgradeUIOptions)
        {
            int UpgradeType = Random.Range(1, 3);   //Choose between Weapon and Passive Items
            if (UpgradeType == 1)
            {
                WeaponUpgrade chooseWeaponUpgrade = WeaponUpgradesOptions[Random.Range(0, WeaponUpgradesOptions.Count)];
                
                if (chooseWeaponUpgrade != null)
                {
                    bool NewWeapon = false;     //Check that is a new Weapon or not
                    for (int i = 0; i < WeaponSlots.Count; i++) 
                    {
                        if (WeaponSlots[i] != null && WeaponSlots[i].WeaponData == chooseWeaponUpgrade.WeaponData)
                        {
                            NewWeapon = false;
                            if (!NewWeapon)
                            {
                                UpgradeOption.UpgradeButton.onClick.AddListener(() => LevelUpWeapon(i));        //Button function when choose Weapon and have Weapon then UpgradeWeapon
                            }
                            break;
                        }
                        else
                        {
                            NewWeapon = true;
                        }
                    }
                    if (NewWeapon)      //Spawn New Weapon
                    {
                        UpgradeOption.UpgradeButton.onClick.AddListener(() => Player.SpawnWeapon(chooseWeaponUpgrade.InitWeapon));  //Button function when choose Weapon and dont have Weapon then Spawn new Weapon

                    }
                }
            }
            else if (UpgradeType == 2)
            {
                PassiveUpgrade choosePassiveUpgrade = PassiveUpgradesOptions[Random.Range(0, PassiveUpgradesOptions.Count)];

                if (choosePassiveUpgrade != null) 
                {
                    bool NewPassive = false;    //Check that is a new Passive or not
                    for (int i = 0; i < PassiveSlots.Count; i++) 
                    {
                        if (PassiveSlots[i] != null && PassiveSlots[i].PassiveData == choosePassiveUpgrade.PassiveData)
                        {
                            NewPassive = false;
                            if (!NewPassive)
                            {
                                UpgradeOption.UpgradeButton.onClick.AddListener(() => LevelUpPassive(i));       //Button function when choose Passive and have Passive then UpgradePassive
                            }
                            break;
                        }else
                        {
                            NewPassive = true;
                        }

                    }
                    if (NewPassive)
                    {
                        UpgradeOption.UpgradeButton.onClick.AddListener(() => Player.SpawnPassiveItem(choosePassiveUpgrade.InitPassive));       //Button function when choose Passive and dont have Passive then Spawn new Passive
                    }

                }
            }
        }
    }
}

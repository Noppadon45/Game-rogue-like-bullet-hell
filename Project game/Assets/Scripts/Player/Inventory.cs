using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Obsolete]
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
        public int WeaponUpgradeIndex;
        public GameObject InitWeapon;
        public WeaponScriptableObject WeaponData;
    }
    [System.Serializable]
    public class PassiveUpgrade
    {
        public int PassiveUpgradeIndex;
        public GameObject InitPassive;
        public PassiveitemScriptableScript PassiveData;
    }
    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text UpgradeItemDisplay;
        public TMP_Text DescriptionItemDisplay;
        public Image UpgradeImageDisplay;
        public Button UpgradeButton;
    }

    public List<WeaponUpgrade> WeaponUpgradesOptions = new List<WeaponUpgrade>();   //List WeaponUpgrade Option for Weapon
    public List<PassiveUpgrade> PassiveUpgradesOptions = new List<PassiveUpgrade>();    //List PassiveUpgrade Option for Passive
    public List<UpgradeUI> UpgradeUIOptions = new List<UpgradeUI>();    //List UpgradeUI Option in scene

    public List<WeaponEvolution> weaponEvolutions = new List<WeaponEvolution>();
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

        if (GameManager.instance != null && GameManager.instance.IsLevelUP)
        {
            GameManager.instance.LevelUPEnd();
        }
        
    }
    public void AddPassive (int Index , Passiveitem Passive )   //Add Passive to Inventory
    {
        PassiveSlots[Index] = Passive;
        PassiveLevels[Index] = Passive.PassiveData.Level;
        PassiveUi[Index].enabled = true;                     //Enable  the image Conponent
        PassiveUi[Index].sprite = Passive.PassiveData.icon;

        if (GameManager.instance != null && GameManager.instance.IsLevelUP)
        {
            GameManager.instance.LevelUPEnd();
        }
    }

    public void LevelUpWeapon (int Index , int WeaponUpgradeIndex)
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

            WeaponUpgradesOptions[WeaponUpgradeIndex].WeaponData = UpgradeWeapon.GetComponent<WeaponsController>().WeaponData;

            

            if (GameManager.instance != null && GameManager.instance.IsLevelUP)
            {
                GameManager.instance.LevelUPEnd();
            }
        }
    }
    public void LevelUpPassive (int Index , int PassiveUpgradeIndex) 
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

            PassiveUpgradesOptions[PassiveUpgradeIndex].PassiveData = UpgradePassive.GetComponent<Passiveitem>().PassiveData;

            if (GameManager.instance != null && GameManager.instance.IsLevelUP)
            {
                GameManager.instance.LevelUPEnd();
            }
        }
    }

    void ApplyUpgradeOption()
    {
        List <WeaponUpgrade> UpgradeWeaponavailable = new List <WeaponUpgrade>(WeaponUpgradesOptions);
        List <PassiveUpgrade> UpgradePassiveavailable = new List <PassiveUpgrade>(PassiveUpgradesOptions);

        foreach (var UpgradeOption in UpgradeUIOptions)
        {
            if (UpgradeWeaponavailable.Count == 0 && UpgradePassiveavailable.Count == 0)
            {
                return;
            }
            int UpgradeType;
            if (UpgradeWeaponavailable.Count == 0)      //When dont have Weapon Item show Passive Item Only
            {
                UpgradeType = 2;
            }
            else if (UpgradePassiveavailable.Count == 0)         //When dont have Passive Item show Weapon Item Only
            {
                UpgradeType = 1;
            }else
            {
                UpgradeType = Random.Range(1, 3);       //If Weapon or Passive avalible Choose between Weapon or Passive Item
            }

            if (UpgradeType == 1)
            {
                WeaponUpgrade chooseWeaponUpgrade = UpgradeWeaponavailable[Random.Range(0, UpgradeWeaponavailable.Count)];

                UpgradeWeaponavailable.Remove(chooseWeaponUpgrade);         //Remove Weapon Upgrade Option when player already that Weapon
                
                if (chooseWeaponUpgrade != null)
                {
                    EnableUpgradeUI(UpgradeOption);
                    bool NewWeapon = false;     //Check that is a new Weapon or not
                    for (int i = 0; i < WeaponSlots.Count; i++) 
                    {
                        if (WeaponSlots[i] != null && WeaponSlots[i].WeaponData == chooseWeaponUpgrade.WeaponData)
                        {
                            NewWeapon = false;
                            if (!NewWeapon)
                            {
                                if (!chooseWeaponUpgrade.WeaponData.NextLevelPrefab)    //When dont have next Level Weapon
                                {
                                    DisableUpgradeUI(UpgradeOption);
                                    break;

                                }
                                UpgradeOption.UpgradeButton.onClick.AddListener(() => LevelUpWeapon(i, chooseWeaponUpgrade.WeaponUpgradeIndex));        //Button function when choose Weapon and have Weapon then UpgradeWeapon

                                //Set the Desciption and name to the next level
                                UpgradeOption.DescriptionItemDisplay.text = chooseWeaponUpgrade.WeaponData.NextLevelPrefab.GetComponent<WeaponsController>().WeaponData.NameDescription;
                                UpgradeOption.UpgradeItemDisplay.text = chooseWeaponUpgrade.WeaponData.NextLevelPrefab.GetComponent<WeaponsController>().WeaponData.Name;
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

                        //Apply the init Description and name
                        UpgradeOption.DescriptionItemDisplay.text = chooseWeaponUpgrade.WeaponData.NameDescription;
                        UpgradeOption.UpgradeItemDisplay.text = chooseWeaponUpgrade.WeaponData.Name;

                    }
                    UpgradeOption.UpgradeImageDisplay.sprite = chooseWeaponUpgrade.WeaponData.icon;
                }
            }
            else if (UpgradeType == 2)
            {
                PassiveUpgrade choosePassiveUpgrade = UpgradePassiveavailable[Random.Range(0, UpgradePassiveavailable.Count)];

                UpgradePassiveavailable.Remove(choosePassiveUpgrade);       //Remove Passive Upgrade Option when player already that Passive

                if (choosePassiveUpgrade != null) 
                {
                    EnableUpgradeUI(UpgradeOption);
                    bool NewPassive = false;    //Check that is a new Passive or not
                    for (int i = 0; i < PassiveSlots.Count; i++) 
                    {
                        if (PassiveSlots[i] != null && PassiveSlots[i].PassiveData == choosePassiveUpgrade.PassiveData)
                        {
                            NewPassive = false;
                            if (!NewPassive)
                            {
                                if (!choosePassiveUpgrade.PassiveData.NextLevelPrefab)
                                {
                                    DisableUpgradeUI(UpgradeOption);
                                    break;
                                }
                                   //Button function when choose Passive and have Passive then UpgradePassive

                                //Set the Desciption and name to the next level
                                UpgradeOption.DescriptionItemDisplay.text = choosePassiveUpgrade.PassiveData.NextLevelPrefab.GetComponent<Passiveitem>().PassiveData.NameDescription;
                                UpgradeOption.UpgradeItemDisplay.text = choosePassiveUpgrade.PassiveData.NextLevelPrefab.GetComponent<Passiveitem>().PassiveData.Name;
                            }
                            break;
                        }else
                        {
                            NewPassive = true;
                        }

                    }
                    if (NewPassive)
                    {
                             //Button function when choose Passive and dont have Passive then Spawn new Passive

                        //Apply the init Description and name
                        UpgradeOption.DescriptionItemDisplay.text = choosePassiveUpgrade.PassiveData.NameDescription;
                        UpgradeOption.UpgradeItemDisplay.text = choosePassiveUpgrade.PassiveData.Name;
                    }
                    UpgradeOption.UpgradeImageDisplay.sprite = choosePassiveUpgrade.PassiveData.icon;


                }
            }
        }
    }

    void RemoveUpgradeOption()
    {
        foreach (var UpgradeOption in UpgradeUIOptions)
        {
            UpgradeOption.UpgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(UpgradeOption);        //Disable UpgradeUI for disable all UI option  before applying upgrade to them
        }
    }

    public void RemoveandApplyUpgrade()
    {
        RemoveUpgradeOption();
        ApplyUpgradeOption();
    }

    void DisableUpgradeUI(UpgradeUI UI)
    {
        UI.UpgradeItemDisplay.transform.parent.gameObject.SetActive(false);
    }
     void EnableUpgradeUI(UpgradeUI UI)
    {
        UI.UpgradeItemDisplay.transform.parent.gameObject.SetActive(true);
    }

    public List<WeaponEvolution> GetPossibleEvolution()
    {
        List<WeaponEvolution> PossibleEvolution = new List<WeaponEvolution>();

        foreach (WeaponsController weapon in WeaponSlots)
        {
            if (weapon != null)
            {
                foreach (Passiveitem Catalyst in PassiveSlots)
                {
                    if (Catalyst != null)
                    {
                        foreach (WeaponEvolution evolution in weaponEvolutions)
                        {
                            if (weapon.WeaponData.Level >= evolution.WeaponBaseData.Level && Catalyst.PassiveData.Level >= evolution.CatalystPassiveItemData.Level)
                            {
                                PossibleEvolution.Add(evolution);
                            }
                        }
                    }
                }
            }
        }

        return PossibleEvolution;
    }

    public void EvolveWeapon(WeaponEvolution Evolution)
    {
        for (int weaponSlotIndex = 0; weaponSlotIndex < WeaponSlots.Count; weaponSlotIndex++)
        {
            WeaponsController weapon = WeaponSlots[weaponSlotIndex];

            if (!weapon)
            {
                continue;
            }

            for (int catalystSlotIndex = 0; catalystSlotIndex < PassiveSlots.Count; catalystSlotIndex++)
            {
                Passiveitem catalyst = PassiveSlots[catalystSlotIndex];

                if (!catalyst)
                {
                    continue;
                }

                if (weapon && catalyst && weapon.WeaponData.Level >= Evolution.WeaponBaseData.Level && catalyst.PassiveData.Level >= Evolution.CatalystPassiveItemData.Level)
                {
                    GameObject EvolvedWeapon = Instantiate(Evolution.EvoWeapon, transform.position, Quaternion.identity);
                    WeaponsController evolvedWeaponController = EvolvedWeapon.GetComponent<WeaponsController>();

                    EvolvedWeapon.transform.SetParent(transform);   //Set Weapon to be a child of the player
                    AddWeapon(weaponSlotIndex, evolvedWeaponController);
                    Destroy(weapon.gameObject);

                    //Update level and icon
                    WeaponLevels[weaponSlotIndex] = evolvedWeaponController.WeaponData.Level;
                    WeaponUi[weaponSlotIndex].sprite = evolvedWeaponController.WeaponData.icon;

                    //Update the option upgrade Evolve
                    WeaponUpgradesOptions.RemoveAt(evolvedWeaponController.WeaponData.weaponEvolveandRemove);   

                    Debug.LogWarning("Evolve");

                    return;
                }
            }
        }
    }

    
    
        
    

}

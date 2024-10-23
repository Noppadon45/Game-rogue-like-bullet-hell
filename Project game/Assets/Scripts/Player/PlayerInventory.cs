using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class PlayerInventory : MonoBehaviour
{

    //Inside Slot It contain
    //1.Item(Weapon/Passive)
    //2.Image(UI equivalent of the slot
    [System.Serializable] 
    public class Slot
    {
        public Item item;
        public Image image;

        public void Assign(Item assignItem)
        {
            item = assignItem;
            if (item is Weapon)
            {
                Weapon w = item as Weapon;
                image.enabled = true;
                image.sprite = w.data.icon;
            }
            else
            {
                Passive p = item as Passive;
                image.enabled = true;
                image.sprite = p.data.icon;
            }
            Debug.Log(string.Format("Assign {0} item to player", item.name));
        }

        public void Clear()
        {
            item = null;
            image.enabled = false;
            image.sprite = null;
        }

        public bool IsEmpty()
        {
            return item == null;
        }

    }

    public List<Slot> WeaponSlot = new List<Slot>(5);
    public List<Slot> PassiveSlot = new List<Slot>(5);

    [System.Serializable]
    public class UpgradeUI
    {
        public TMP_Text upgradeName;
        public TMP_Text upgradeDescription;
        public Image upgradeIcon;
        public Button upgradeButton;
    }

    //Inside Slot It contain
    //List of Weapon and Passive for weapon and passive
    //UpgradeOption it store weapon and passive can upgrade in game
    [Header("UI")]
    public List<WeaponData> availableWeapon = new List<WeaponData>();
    public List<PassiveData> availablePassive = new List<PassiveData>();
    public List<UpgradeUI> upgradeUIOption = new List<UpgradeUI>();

    PlayerStats playerStats;
    
    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

    //Check if the inventory has an item of a certain type
    public bool Has(ItemData type)
    {
        return Get(type);
    }

    public Item Get(ItemData type) 
    { 
        if (type is WeaponData)
        {
            return Get(type as WeaponData);
        }
        else if (type is PassiveData)
        {
            return Get(type as PassiveData);
        }
        return null;

    }

    //Find a passive of certain type in the inventory
    public Passive Get(PassiveData type)
    {
        foreach (Slot s in PassiveSlot)
        {
            Passive p = s.item as Passive;
            if (p.data == type)
            {
                return p;
            }
        }
        return null;
    }

    //Find a weapon of certain type in the inventory
    public Weapon Get(WeaponData type)
    {
        foreach (Slot s in WeaponSlot)
        {
            Weapon w = s.item as Weapon;
            if (w.data == type)
            {
                return w;
            }
        }
        return null;
    }

    //Remove a weapon of partical type as specifile by weapon data
    public bool Remove(WeaponData data , bool removeUpgradeAvailable = false)
    {
        if (removeUpgradeAvailable)
        {
            availableWeapon.Remove(data);
        }
        for (int i = 0; i < WeaponSlot.Count; i++)
        {
            Weapon w = WeaponSlot[i].item as Weapon;
            if (w.data == data)
            {
                WeaponSlot[i].Clear();
                w.OnEquip();
                Destroy(w.gameObject);
                return true;
            }
        }
        return false;
    }

    //Remove a passive of partical type as specifile by passive data
    public bool Remove(PassiveData data , bool removeUpgradeAvailable = false)
    {
        if (removeUpgradeAvailable)
        {
            availablePassive.Remove(data);
        }
        for (int i = 0; i < PassiveSlot.Count; i++)
        {
            Passive p = PassiveSlot[i].item as Passive;
            if (p.data == data) 
            {
                PassiveSlot[i].Clear();
                p.OnEquip();
                Destroy(p.gameObject);
                return true;
            }
        }
        return false;
    }

    //If item is passed , determine what type it is and call it
    //Optional boolean to remove this item from upgrade list
    public bool Remove(ItemData data , bool removeUpgradeAvailable = false)
    {
        if (data is PassiveData)
        {
            return Remove(data as PassiveData, removeUpgradeAvailable);
        }
        else if (data is WeaponData)
        {
            return Remove(data as WeaponData, removeUpgradeAvailable);
        }
        return false;
    }

    //Find an empty slot and add a weapon of a certain type 
    //the slot number that the item was put in
    public int Add(WeaponData data)
    {
        int slotNum = -1;

        //Find the empty slot of weapon
        for (int i = 0; i < WeaponSlot.Capacity ;i++)
        {
            if (WeaponSlot[i].IsEmpty())
            {
                slotNum = i;
                break;
            }
        }

        //If no empty slot , exit
        if (slotNum < 0)
        {
            return slotNum;
        }

        //Create the weapon in the slot
        //Get the type of weapon that will spawn
        Type weaponType = Type.GetType(data.behaviour);

        if (weaponType != null)
        {
            //Spawn the weapon Object
            GameObject go = new GameObject(data.baseStats.name + "Controller");
            Weapon spawnWeapon = (Weapon)go.AddComponent(weaponType);
            spawnWeapon.Initialise(data);
            spawnWeapon.transform.SetParent(transform);             //Set the weapon to the child of player
            spawnWeapon.transform.localPosition = Vector2.zero;     
            spawnWeapon.OnEquip();

            //Assign the weapon to the slot
            WeaponSlot[slotNum].Assign(spawnWeapon);

            //Close the levelup Ui
            if (GameManager.instance != null && GameManager.instance.IsLevelUP)
            {
                GameManager.instance.LevelUPEnd();
            }
            playerStats.CalculateStats();

            return slotNum;
        }
        else
        {
            Debug.Log(string.Format("Invalid Weapon for {0}",data.name));
        }
        return -1;
    }

    //Find an empty slot and add a passive of a certain type 
    //the slot number that the item was put in
    public int Add(PassiveData data)
    {
        int slotNum = -1;

        //Find the empty slot of passive
        for (int i = 0; i < PassiveSlot.Capacity; i++)
        {
            if (PassiveSlot[i].IsEmpty())
            {
                slotNum = i;
                break;
            }
        }

        //If no empty slot , exit
        if (slotNum < 0)
        {
            return slotNum;
        }

        //Create the passive in the slot
        //Get the type of passive that will spawn
        GameObject go = new GameObject(data.baseStats.name + "Passive");
        Passive p = go.AddComponent<Passive>();
        p.Initialise(data);
        p.transform.SetParent(transform);           //Set the passive to be the child of player
        p.transform.localPosition = Vector2.zero;

        //Assign the passive to the slot
        PassiveSlot[slotNum].Assign(p);

        //Close the levelup Ui 
        if (GameManager.instance != null && GameManager.instance.IsLevelUP)
        {
            GameManager.instance.LevelUPEnd();
        }
        playerStats.CalculateStats();

        return slotNum;
        
    }

    //If will add item this function will determine that
    public int Add(ItemData data)
    {
        if (data is WeaponData)
        {
            return Add(data as WeaponData);
        }
        else if (data is PassiveData)
        {
            return Add(data as PassiveData);
        }
        return -1;
    }

    public void LevelUpWeapon(int slotIndex , int upgradeIndex)
    {
        if (WeaponSlot.Count > slotIndex)
        {
            Weapon weapon = WeaponSlot[slotIndex].item as Weapon;

            //If player level is max dont level up
            if (!weapon.DoLevelUp())
            {
                Debug.Log(string.Format("Fail to LevelUp {0} Weapon", weapon.name));
                return;
            }
        }

        if (GameManager.instance != null && GameManager.instance.IsLevelUP)
        {
            GameManager.instance.LevelUPEnd();
        }
    }

    public void LevelUPPassiveItem(int slotIndex, int upgradeIndex)
    {
        if (PassiveSlot.Count > slotIndex)
        {
            Passive passive = PassiveSlot[slotIndex].item as Passive;

            if (!passive.DoLevelUp())
            {
                Debug.Log(string.Format("Fail to LevelUp {0} Pasive", passive.name));
                return;
            }
        }

        if (GameManager.instance != null && GameManager.instance.IsLevelUP)
        {
            GameManager.instance.LevelUPEnd();
        }
        playerStats.CalculateStats();
    }

    //This function it show upgrade option should appear
    void ApplyUpgradeOption()
    {

        //Make a duplicate of the available weapon and passive upgrade list
        List<WeaponData> availableWeaponUpgrade = new List<WeaponData>(availableWeapon);
        List<PassiveData> availablePassiveUpgrade = new List<PassiveData>(availablePassive);

        //In the each slot of upgradeUI
        foreach (UpgradeUI upgradeOption in upgradeUIOption)
        {
            //If no more available upgrade , then return
            if (availableWeaponUpgrade.Count == 0 && availablePassiveUpgrade.Count == 0)
            {
                return;
            }

            //This upgrade should be for weapon or passive
            int UpgradeType;

            if (availableWeaponUpgrade.Count == 0)
            {
                UpgradeType = 2;
            }
            else if (availablePassiveUpgrade.Count == 0)
            {
                UpgradeType = 1;
            }
            else
            {
                //Random Generate a number between 1 or 2
                UpgradeType = UnityEngine.Random.Range(1, 3);
            }

            //Generate weapon upgrade
            if (UpgradeType == 1)
            {
                //Pick up a weapon upgrade and then remove it because weapon this upgrade dont get it twice
                WeaponData chooseWeaponUpgrade = availableWeaponUpgrade[UnityEngine.Random.Range(0 , availableWeaponUpgrade.Count)];
                availableWeaponUpgrade.Remove(chooseWeaponUpgrade);

                //Ensure that selected weapon is valid
                if (chooseWeaponUpgrade != null)
                {
                    //Turn on upgradeUI
                    EnableUpgradeUI(upgradeOption);

                    //Loop each weapon if it find a match weapon it will
                    //use event listener to the button that will level up that weapon
                    //when this upgrade option is clicked
                    bool isLevelUp = false;
                    for (int i = 0; i < WeaponSlot.Count; i++)
                    {
                        Weapon w = WeaponSlot[i].item as Weapon;
                        if (w != null && w.data == chooseWeaponUpgrade) 
                        {
                            //If the weapon is already max level ,weapon cant upgrade
                            if (chooseWeaponUpgrade.maxLevel <= w.currentLevel)
                            {
                                //Disable UpgradeUI(upgradeOption)
                                isLevelUp = false;
                                break;
                            }
                        
                            //Set the Event Listener , weapon and level description to be that of the next level
                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, i));             //Apply button function
                            Weapon.Stats nextLevel = chooseWeaponUpgrade.GetLevelData(w.currentLevel + 1);
                            upgradeOption.upgradeDescription.text = nextLevel.description;
                            upgradeOption.upgradeName.text = nextLevel.name;
                            upgradeOption.upgradeIcon.sprite = chooseWeaponUpgrade.icon;
                            isLevelUp = true;
                            break;
                        }
                    }

                    //If cant levelup it mean spawn a new weapon
                    if (!isLevelUp)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => Add(chooseWeaponUpgrade));        //Apply button function
                        upgradeOption.upgradeDescription.text = chooseWeaponUpgrade.baseStats.description;      //Apply initial weapon description
                        upgradeOption.upgradeName.text = chooseWeaponUpgrade.name;                              //Apply initial weapon name
                        upgradeOption.upgradeIcon.sprite = chooseWeaponUpgrade.icon;                            //Aooly initial weapon icon
                    }
                }
            }
            else if (UpgradeType == 2)
            {
                //It will disable an upgrade slot  if that weapon has already reach max level
                PassiveData choosePassiveUpgrade = availablePassiveUpgrade[UnityEngine.Random.Range(0, availablePassiveUpgrade.Count)];
                availablePassiveUpgrade.Remove(choosePassiveUpgrade);

                if (choosePassiveUpgrade != null)
                {
                    //Turn on upgradeUI
                    EnableUpgradeUI(upgradeOption);

                    //Loop each passive if it find a match weapon it will
                    //use event listener to the button that will level up that passive
                    //when this upgrade option is clicked
                    bool isLevelUp = false;
                    for (int i = 0; i < PassiveSlot.Count; i++)
                    {
                        Passive p = PassiveSlot[i].item as Passive;
                        if (p != null && p.data == choosePassiveUpgrade)
                        {
                            //If the passive is already max level ,passive cant upgrade
                            if (choosePassiveUpgrade.maxLevel <= p.currentLevel)
                            {
                                //Disable UpgradeUI(upgradeOption)
                                isLevelUp = false;
                                break;
                            }

                            //Set the Event Listener , passive and level description to be that of the next level
                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUPPassiveItem(i, i));        //Apply button function
                            Passive.Modifier nextLevel = choosePassiveUpgrade.GetLevelData(p.currentLevel + 1);
                            upgradeOption.upgradeDescription.text = nextLevel.description;
                            upgradeOption.upgradeName.text = nextLevel.name;
                            upgradeOption.upgradeIcon.sprite = choosePassiveUpgrade.icon;
                            isLevelUp = true;
                            break;
                        }
                    }
                    //If cant levelup it mean spawn a new passive
                    if (!isLevelUp)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => Add(choosePassiveUpgrade));       //Apply button function
                        Passive.Modifier nextLevel = choosePassiveUpgrade.baseStats;                    
                        upgradeOption.upgradeDescription.text = nextLevel.description;                          //Apply initial passive description
                        upgradeOption.upgradeName.text = nextLevel.name;                                        //Apply initial passive name
                        upgradeOption.upgradeIcon.sprite = choosePassiveUpgrade.icon;                           //Aooly initial passive icon
                    }
                }
            }
        }
    }

    void RemoveUpgradeOption()
    {
        foreach (UpgradeUI upgradeOption in upgradeUIOption)
        {
            upgradeOption.upgradeButton.onClick.RemoveAllListeners();
            DisableUpgradeUI(upgradeOption);        //Call the DisableUpgradeUI to disable all UI optipn before apply upgrade to them
        }
    }

    public void RemoveAndApplyUpgrade()
    {
        RemoveUpgradeOption();
        ApplyUpgradeOption();
    }

    void DisableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeName.transform.parent.gameObject.SetActive(false);
    }

    void EnableUpgradeUI(UpgradeUI ui)
    {
        ui.upgradeName.transform.parent.gameObject.SetActive(true);
    }
}

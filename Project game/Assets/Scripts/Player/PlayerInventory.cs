using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


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

    public List<Slot> WeaponSlot = new List<Slot>(6);
    public List<Slot> PassiveSlot = new List<Slot>(6);

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
        if (type is WeaponData) return Get(type as WeaponData);
        else if (type is PassiveData) return Get(type as PassiveData);
        return null;

    }

    //Find a passive of certain type in the inventory
    public Passive Get(PassiveData type)
    {
        foreach (Slot s in PassiveSlot)
        {
            Passive p = s.item as Passive;
            if (p.data == type)
                return p;
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
                return w;
        }
        return null;
    }

    //Remove a weapon of partical type as specifile by weapon data
    public bool Remove(WeaponData data , bool removeUpgradeAvailable = false)
    {
        if (removeUpgradeAvailable) availableWeapon.Remove(data);

        for (int i = 0; i < WeaponSlot.Count; i++)
        {
            Weapon w = WeaponSlot[i].item as Weapon;
            if (w.data == data)
            {
                WeaponSlot[i].Clear();
                w.OnUnEquip();
                Destroy(w.gameObject);
                return true;
            }
        }
        return false;
    }

    //Remove a passive of partical type as specifile by passive data
    public bool Remove(PassiveData data , bool removeUpgradeAvailable = false)
    {
        if (removeUpgradeAvailable) availablePassive.Remove(data);

        for (int i = 0; i < WeaponSlot.Count; i++)
        {
            Passive p = WeaponSlot[i].item as Passive;
            if (p.data == data) 
            {
                WeaponSlot[i].Clear();
                p.OnUnEquip();
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
        if (data is PassiveData) return Remove(data as PassiveData, removeUpgradeAvailable);
        else if (data is WeaponData) return Remove(data as WeaponData, removeUpgradeAvailable);
        return false;
    }

    //Find an empty slot and add a weapon of a certain type 
    //the slot number that the item was put in
    public int Add(WeaponData data)
    {
        int slotNum = -1;

        //Find the empty slot of weapon
        for (int i = 0; i < WeaponSlot.Capacity; i++)
        {
            if (WeaponSlot[i].IsEmpty())
            {
                slotNum = i;
                break;
            }
        }

        //If no empty slot , exit
        if (slotNum < 0) return slotNum;

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
                GameManager.instance.LevelUPEnd();
            
            

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
        if (slotNum < 0) return slotNum;
        

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
        if (data is WeaponData) return Add(data as WeaponData); 
        else if (data is PassiveData) return Add(data as PassiveData);
        return -1;
    }

    // Overload so that we can use both ItemData or Item to level up an
    // item in the inventory.
    public void LevelUpWeapon(int slotIndex , int upgradeIndex)
    {
        if (WeaponSlot.Count > slotIndex)
        {
            Weapon weapon = WeaponSlot[slotIndex].item as Weapon;

            // Don't level up the weapon if it is already at max level.
            if (!weapon.DoLevelUp())
            {
                Debug.LogWarning(string.Format(
                    "Failed to level up {0}.",
                    weapon.name
                ));
                return;
            }
        }

        if (GameManager.instance != null && GameManager.instance.IsLevelUP)
        {
            GameManager.instance.LevelUPEnd();
        }
    }

    public void LevelUpPassiveItem(int slotIndex, int upgradeIndex)
    {
        if (PassiveSlot.Count > slotIndex)
        {
            Passive p = PassiveSlot[slotIndex].item as Passive;
            if (!p.DoLevelUp())
            {
                Debug.LogWarning(string.Format(
                    "Failed to level up {0}.",
                    p.name
                ));
                return;
            }
        }

        if (GameManager.instance != null && GameManager.instance.IsLevelUP)
        {
            GameManager.instance.LevelUPEnd();
        }
        playerStats.CalculateStats();
    }

    // Determines what upgrade options should appear.
    void ApplyUpgradeOptions()
    {
        // <availableUpgrades> is the list of possible upgrades that we will populate from
        // <allPossibleUpgrades>, which is a list of all available weapons and passives.
        List<WeaponData> availableWeaponUpgrades = new List<WeaponData>(availableWeapon);
        List<PassiveData> availablePassiveItemUpgrades = new List<PassiveData>(availablePassive);

        foreach (UpgradeUI upgradeOption in upgradeUIOption)
        {
            // If there are no more avaiable upgrades, then we abort.
            if (availableWeaponUpgrades.Count == 0 && availablePassiveItemUpgrades.Count == 0)
                return;

            // Determine whether this upgrade should be for passive or active weapons.
            int upgradeType;
            if (availableWeaponUpgrades.Count == 0)
            {
                upgradeType = 2;
            }
            else if (availablePassiveItemUpgrades.Count == 0)
            {
                upgradeType = 1;
            }
            else
            {
                // Random generates a number between 1 and 2.
                upgradeType = UnityEngine.Random.Range(1, 3);
            }

            // Generates an active weapon upgrade.
            if (upgradeType == 1)
            {

                // Pick a weapon upgrade, then remove it so that we don't get it twice.
                WeaponData chosenWeaponUpgrade = availableWeaponUpgrades[UnityEngine.Random.Range(0, availableWeaponUpgrades.Count)];
                availableWeaponUpgrades.Remove(chosenWeaponUpgrade);

                // Ensure that the selected weapon data is valid.
                if (chosenWeaponUpgrade != null)
                {
                    // Turns on the UI slot.
                    EnableUpgradeUI(upgradeOption);

                    // Loops through all our existing weapons. If we find a match, we will
                    // hook an event listener to the button that will level up the weapon
                    // when this upgrade option is clicked.
                    bool isLevelUp = false;
                    for (int i = 0; i < WeaponSlot.Count; i++)
                    {
                        Weapon w = WeaponSlot[i].item as Weapon;
                        if (w != null && w.data == chosenWeaponUpgrade)
                        {
                            // If the weapon is already at the max level, do not allow upgrade.
                            if (chosenWeaponUpgrade.maxLevel <= w.currentLevel)
                            {
                                DisableUpgradeUI(upgradeOption);
                                isLevelUp = true;
                                break;
                            }

                            // Set the Event Listener, item and level description to be that of the next level
                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, i)); //Apply button functionality
                            Weapon.Stats nextLevel = chosenWeaponUpgrade.GetLevelData(w.currentLevel + 1);
                            upgradeOption.upgradeDescription.text = nextLevel.description;
                            upgradeOption.upgradeName.text = nextLevel.name;
                            upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.icon;
                            isLevelUp = true;
                            break;
                        }
                    }

                    // If the code gets here, it means that we will be adding a new weapon, instead of
                    // upgrading an existing weapon.
                    if (!isLevelUp)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => Add(chosenWeaponUpgrade)); //Apply button functionality
                        upgradeOption.upgradeDescription.text = chosenWeaponUpgrade.baseStats.description;  //Apply initial description
                        upgradeOption.upgradeName.text = chosenWeaponUpgrade.baseStats.name;    //Apply initial name
                        upgradeOption.upgradeIcon.sprite = chosenWeaponUpgrade.icon;
                    }
                }
            }
            else if (upgradeType == 2)
            {
                // NOTE: We have to recode this system, as right now it disables an upgrade slot if
                // we hit a weapon that has already reached max level.
                PassiveData chosenPassiveUpgrade = availablePassiveItemUpgrades[UnityEngine.Random.Range(0, availablePassiveItemUpgrades.Count)];
                availablePassiveItemUpgrades.Remove(chosenPassiveUpgrade);

                if (chosenPassiveUpgrade != null)
                {
                    // Turns on the UI slot.
                    EnableUpgradeUI(upgradeOption);

                    // Loops through all our existing passive. If we find a match, we will
                    // hook an event listener to the button that will level up the weapon
                    // when this upgrade option is clicked.
                    bool isLevelUp = false;
                    for (int i = 0; i < PassiveSlot.Count; i++)
                    {
                        Passive p = PassiveSlot[i].item as Passive;
                        if (p != null && p.data == chosenPassiveUpgrade)
                        {
                            // If the passive is already at the max level, do not allow upgrade.
                            if (chosenPassiveUpgrade.maxLevel <= p.currentLevel)
                            {
                                DisableUpgradeUI(upgradeOption);
                                isLevelUp = true;
                                break;
                            }
                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpPassiveItem(i, i)); //Apply button functionality
                            Passive.Modifier nextLevel = chosenPassiveUpgrade.GetLevelData(p.currentLevel + 1);
                            upgradeOption.upgradeDescription.text = nextLevel.description;
                            upgradeOption.upgradeName.text = nextLevel.name;
                            upgradeOption.upgradeIcon.sprite = chosenPassiveUpgrade.icon;
                            isLevelUp = true;
                            break;
                        }
                    }

                    if (!isLevelUp) //Spawn a new passive item
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => Add(chosenPassiveUpgrade)); //Apply button functionality
                        Passive.Modifier nextLevel = chosenPassiveUpgrade.baseStats;
                        upgradeOption.upgradeDescription.text = nextLevel.description;  //Apply initial description
                        upgradeOption.upgradeName.text = nextLevel.name;  //Apply initial name
                        upgradeOption.upgradeIcon.sprite = chosenPassiveUpgrade.icon;
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
        ApplyUpgradeOptions();
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

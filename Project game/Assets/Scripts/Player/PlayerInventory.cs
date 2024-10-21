using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Unity.VisualScripting;

public class PlayerInventory : MonoBehaviour
{
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

    [Header("UI")]
    public List<WeaponData> availableWeapon = new List<WeaponData>();
    public List<PassiveData> availablePassive = new List<PassiveData>();
    public List<UpgradeUI> upgradeUIOption = new List<UpgradeUI>();

    PlayerStats playerStats;

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
    }

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

    public int Add(WeaponData data)
    {
        int slotNum = -1;

        for (int i = 0; i < WeaponSlot.Capacity ;i++)
        {
            if (WeaponSlot[i].IsEmpty())
            {
                slotNum = i;
                break;
            }
        }

        if (slotNum < 0)
        {
            return slotNum;
        }

        Type weaponType = Type.GetType(data.behaviour);

        if (weaponType != null)
        {
            GameObject go = new GameObject(data.baseStats.name + "Controller");
            Weapon spawnWeapon = (Weapon)go.AddComponent(weaponType);
            spawnWeapon.Initialise(data);
            spawnWeapon.transform.SetParent(transform);
            spawnWeapon.transform.localPosition = Vector2.zero;
            spawnWeapon.OnEquip();

            WeaponSlot[slotNum].Assign(spawnWeapon);

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


    public int Add(PassiveData data)
    {
        int slotNum = -1;

        for (int i = 0; i < PassiveSlot.Capacity; i++)
        {
            if (PassiveSlot[i].IsEmpty())
            {
                slotNum = i;
                break;
            }
        }

        if (slotNum < 0)
        {
            return slotNum;
        }

        GameObject go = new GameObject(data.baseStats.name + "Passive");
        Passive p = go.AddComponent<Passive>();
        p.Initialise(data);
        p.transform.SetParent(transform);
        p.transform.localPosition = Vector2.zero;


        PassiveSlot[slotNum].Assign(p);

        if (GameManager.instance != null && GameManager.instance.IsLevelUP)
        {
            GameManager.instance.LevelUPEnd();
        }
        playerStats.CalculateStats();

        return slotNum;
        
    }

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

    void ApplyUpgradeOption()
    {
        List<WeaponData> availableWeaponUpgrade = new List<WeaponData>(availableWeapon);
        List<PassiveData> availablePassiveUpgrade = new List<PassiveData>(availablePassive);

        foreach (UpgradeUI upgradeOption in upgradeUIOption)
        {
            if (availableWeaponUpgrade.Count == 0 && availablePassiveUpgrade.Count == 0)
            {
                return;
            }
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
                UpgradeType = UnityEngine.Random.Range(1, 3);
            }

            if (UpgradeType == 1)
            {
                WeaponData chooseWeaponUpgrade = availableWeaponUpgrade[UnityEngine.Random.Range(0 , availableWeaponUpgrade.Count)];
                availableWeaponUpgrade.Remove(chooseWeaponUpgrade);

                if (chooseWeaponUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                    bool isLevelUp = false;
                    for (int i = 0; i < WeaponSlot.Count; i++)
                    {
                        Weapon w = WeaponSlot[i].item as Weapon;
                        if (w != null && w.data == chooseWeaponUpgrade) 
                        {
                            if (chooseWeaponUpgrade.maxLevel <= w.currentLevel)
                            {
                                isLevelUp = false;
                                break;
                            }
                        

                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUpWeapon(i, i));
                            Weapon.Stats nextLevel = chooseWeaponUpgrade.GetLevelData(w.currentLevel + 1);
                            upgradeOption.upgradeDescription.text = nextLevel.description;
                            upgradeOption.upgradeName.text = nextLevel.name;
                            upgradeOption.upgradeIcon.sprite = chooseWeaponUpgrade.icon;
                            isLevelUp = true;
                            break;
                        }
                    }
                    if (!isLevelUp)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => Add(chooseWeaponUpgrade));
                        upgradeOption.upgradeDescription.text = chooseWeaponUpgrade.baseStats.description;
                        upgradeOption.upgradeName.text = chooseWeaponUpgrade.name;
                        upgradeOption.upgradeIcon.sprite = chooseWeaponUpgrade.icon;
                    }
                }
            }
            else if (UpgradeType == 2)
            {
                PassiveData choosePassiveUpgrade = availablePassiveUpgrade[UnityEngine.Random.Range(0, availablePassiveUpgrade.Count)];
                availablePassiveUpgrade.Remove(choosePassiveUpgrade);

                if (choosePassiveUpgrade != null)
                {
                    EnableUpgradeUI(upgradeOption);

                    bool isLevelUp = false;
                    for (int i = 0; i < PassiveSlot.Count; i++)
                    {
                        Passive p = PassiveSlot[i].item as Passive;
                        if (p != null && p.data == choosePassiveUpgrade)
                        {
                            if (choosePassiveUpgrade.maxLevel <= p.currentLevel)
                            {
                                isLevelUp = false;
                                break;
                            }


                            upgradeOption.upgradeButton.onClick.AddListener(() => LevelUPPassiveItem(i, i));
                            Passive.Modifier nextLevel = choosePassiveUpgrade.GetLevelData(p.currentLevel + 1);
                            upgradeOption.upgradeDescription.text = nextLevel.description;
                            upgradeOption.upgradeName.text = nextLevel.name;
                            upgradeOption.upgradeIcon.sprite = choosePassiveUpgrade.icon;
                            isLevelUp = true;
                            break;
                        }
                    }
                    if (!isLevelUp)
                    {
                        upgradeOption.upgradeButton.onClick.AddListener(() => Add(choosePassiveUpgrade));
                        Passive.Modifier nextLevel = choosePassiveUpgrade.baseStats;
                        upgradeOption.upgradeDescription.text = nextLevel.description;
                        upgradeOption.upgradeName.text = nextLevel.name;
                        upgradeOption.upgradeIcon.sprite = choosePassiveUpgrade.icon;
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

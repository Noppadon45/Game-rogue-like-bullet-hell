using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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
        public TMP_Text upgradeIcon;
        public TMP_Text upgradeButton;
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
}

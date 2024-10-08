using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEditor.Search;
using UnityEngine;

[System.Obsolete("It will be replace in WeaponData Script")]
[CreateAssetMenu(fileName = "WeaponScriptableObject", menuName = "ScriptableObjects/Weapon")]
public class WeaponScriptableObject : ScriptableObject
{
    [SerializeField]
    GameObject prefab;
    public GameObject Prefab { get => prefab; private set => prefab = value; }

    //Base Stats Weapons
    [SerializeField]
    float Damage;
    public float damage { get => Damage; private set => Damage = value; }

    [SerializeField]
    float Speed;
    public float speed { get => Speed; private set => Speed = value; }

    [SerializeField]
    float CoolDownDuration;
    public float coolDownDuration { get => CoolDownDuration; private set => CoolDownDuration = value; }

    [SerializeField]
    int pierce;
    public int Pierce { get => pierce; private set => pierce = value; }

    [SerializeField]
    int level;
    public int Level { get => level; private set => level = value; }

    [SerializeField]
    GameObject nextLevelPrefab;     //Prefab the next level 
    public GameObject NextLevelPrefab { get => nextLevelPrefab; private set => nextLevelPrefab = value; }

    [SerializeField]
    new string name;        //Name of Weapon or Passive when getting Upgrade
    public string Name { get => name; private set => name = value; }

    [SerializeField]
    string nameDescription;     //Description of Weapon or Passive when getting Upgrade
    public string NameDescription { get => nameDescription; private set => nameDescription = value; }


    [SerializeField]
    Sprite Icon;        //Not mean modify in game just only in Editor
    public Sprite icon { get => Icon; private set => Icon = value; }

    [SerializeField]
    int WeaponEvolveandRemove;       //Not mean modify in game just only in Editor
    public int weaponEvolveandRemove { get => WeaponEvolveandRemove; private set => WeaponEvolveandRemove = value; }
}

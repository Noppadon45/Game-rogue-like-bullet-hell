using System.Collections;
using System.Collections.Generic;
using System.IO.Enumeration;
using UnityEngine;

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
}

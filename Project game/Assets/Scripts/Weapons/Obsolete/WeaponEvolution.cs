using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu (fileName = "WeaponEvolution" , menuName = "ScriptableObjects/WeaponEvolution")]
public class WeaponEvolution : ScriptableObject
{
    public WeaponScriptableObject WeaponBaseData;
    public PassiveitemScriptableScript CatalystPassiveItemData;
    public WeaponScriptableObject EvolutionWeaponData;
    public GameObject EvoWeapon;

}

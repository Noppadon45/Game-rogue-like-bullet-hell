using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base Scirpt All melee Weapon pleace on Prefab melee weapons
public class MeleeWeaponBehaviour : MonoBehaviour
{
    public float AttackAfterSecound;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        Destroy(gameObject, AttackAfterSecound);
    }

    
}

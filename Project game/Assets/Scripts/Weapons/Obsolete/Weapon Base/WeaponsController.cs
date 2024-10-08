using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basic script All Weapons

public class WeaponsController : MonoBehaviour
{
    [Header("Weapons Stats")]
    public WeaponScriptableObject WeaponData;
    float CurrentCoolDown;


    protected PlayerMovement pm;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        CurrentCoolDown = WeaponData.coolDownDuration;     //set CD current durations
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        CurrentCoolDown -= Time.deltaTime;      //Attack < 0
        if (CurrentCoolDown < 0f)
        {
            Attack();
        }
    }

    protected virtual void Attack()
    {
        CurrentCoolDown = WeaponData.coolDownDuration;
    }
}

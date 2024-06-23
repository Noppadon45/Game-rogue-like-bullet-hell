using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Basic script All Weapons

public class WeaponsController : MonoBehaviour
{
    [Header("Weapons Stats")]
    public GameObject prefab;
    public float Damage;
    public float Speed;
    public float CoolDownDuration;
    float CurrentCoolDown;
    public float pierce;

    protected PlayerMovement pm;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        pm = FindObjectOfType<PlayerMovement>();
        CurrentCoolDown = CoolDownDuration;     //set CD current durations
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
        CurrentCoolDown = CoolDownDuration;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Base Scirpt All melee Weapon pleace on Prefab melee weapons
public class MeleeWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject weaponData;
    public float DestroyAfterSecound;

    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCoolDownDuration;
    protected float currentPierce;

    void Awake()
    {
        currentDamage = weaponData.damage;
        currentSpeed = weaponData.speed;
        currentCoolDownDuration = weaponData.coolDownDuration;
        currentPierce = weaponData.Pierce;
    }

    public float GetCurrentDamage()
    {
        return currentDamage *= FindObjectOfType<PlayerStats>().currentMight;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, DestroyAfterSecound);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(GetCurrentDamage());
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakbleProp breakble))
            {
                breakble.TakeDamage(GetCurrentDamage());
            }
        }


    }


}

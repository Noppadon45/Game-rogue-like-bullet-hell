using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ProjectileWeapon : Weapon
{
    protected float currentAttackInterval;      //Attack Count Interval
    protected int currentAttackCount;           //Attack Count of weapon

    protected override void Update()
    {
        base.Update();

        if (currentAttackInterval > 0)
        {
            currentAttackInterval -= Time.deltaTime;
            if (currentAttackInterval <= 0 )
            {
                Attack(currentAttackCount);
            }
        }
    }

    public override bool CanAttack()
    {
        if (currentAttackCount > 0) 
        {
            return true;
        }
        return base.CanAttack();
    }

    protected override bool Attack(int AttackCount = 1)
    {
        if (!currentStats.projectilePrefab)
        {
            Debug.Log(string.Format("Projectile prefab has not been set {0}" , name));
            currentCooldown = data.baseStats.cooldown;
            return false;
        }
        
        if (!CanAttack())
        {
            return false;
        }

        float spawnAngle = GetSpawnAngle();

        Projectile prefab = Instantiate(currentStats.projectilePrefab, playerStats.transform.position + (Vector3)GetSpawnOffset(spawnAngle) , Quaternion.Euler(0 , 0 , spawnAngle));


        prefab.weapon = this;
        prefab.player = playerStats;

        if (currentCooldown <= 0 )
        {
            currentCooldown += currentStats.cooldown;
        }

        AttackCount--;

        if (AttackCount > 0)
        {
            currentAttackCount = AttackCount;
            currentAttackInterval = data.baseStats.projectileInterval;
        }
        return true;
    }


    protected virtual float GetSpawnAngle()
    {
        return Mathf.Atan2(playermovement.moveDirection.y , playermovement.moveDirection.x) * Mathf.Rad2Deg;
    }

    protected virtual Vector2 GetSpawnOffset(float spawnAngle = 0)
    {
        return Quaternion.Euler(0 , 0 , spawnAngle) * new Vector2(
            Random.Range(currentStats.spawnVarient.xMin , currentStats.spawnVarient.xMax),
            Random.Range(currentStats.spawnVarient.yMin, currentStats.spawnVarient.yMax)
            );
    }
}

using System.Buffers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ProjectileWeapon : Weapon
{
    protected float currentAttackInterval;      //Attack time Interval
    protected int currentAttackCount;           //Attack Count of weapon

    protected override void Update()
    {
        base.Update();

        //if currentAttackInterval > 0 call attack
        if (currentAttackInterval > 0)
        {
            currentAttackInterval -= Time.deltaTime;

            //if currentAttackInterval <= 0 weapon will spawn from number of currentAttackCount
            if (currentAttackInterval <= 0 )
            {
                Attack(currentAttackCount);
            }
        }
    }
    //if currentcooldown <= 0f
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
        //if no projectilePrefab Show error what prefab has not been set
        if (!currentStats.projectilePrefab)
        {
            Debug.Log(string.Format("Projectile prefab has not been set {0}" , name));
            currentCooldown = data.baseStats.cooldown;
            return false;
        }
        //Check Weapon can attack or not
        if (!CanAttack())
        {
            return false;
        }
        //Calculate the angle and offset of weapon will spawn projectile
        float spawnAngle = GetSpawnAngle();

        //Spawn a copy of the projectile
        Projectile prefab = Instantiate(currentStats.projectilePrefab, playerStats.transform.position + (Vector3)GetSpawnOffset(spawnAngle) , Quaternion.Euler(0 , 0 , spawnAngle));


        prefab.weapon = this;
        prefab.player = playerStats;
            
        //Reset cooldown if this attack using cooldown
        if (currentCooldown <= 0 )
        {
            currentCooldown += currentStats.cooldown;
        }

        AttackCount--;

        //Perform next attack
        if (AttackCount > 0)
        {
            currentAttackCount = AttackCount;
            currentAttackInterval = data.baseStats.projectileInterval;
        }
        return true;
    }

    //Get which direction of the player and spawning projectile should face
    protected virtual float GetSpawnAngle()
    {
        return Mathf.Atan2(playermovement.moveDirection.y , playermovement.moveDirection.x) * Mathf.Rad2Deg;
    }

    //Generate a random point to spawn the projectile on and Rotate the facing of the point by SpawnAngle
    protected virtual Vector2 GetSpawnOffset(float spawnAngle = 0)
    {
        return Quaternion.Euler(0 , 0 , spawnAngle) * new Vector2(
            Random.Range(currentStats.spawnVarient.xMin , currentStats.spawnVarient.xMax),
            Random.Range(currentStats.spawnVarient.yMin, currentStats.spawnVarient.yMax)
            );
    }
}

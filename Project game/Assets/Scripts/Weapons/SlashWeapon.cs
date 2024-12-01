using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlashWeapon : ProjectileWeapon
{
    int currentSpawnCount;      //How many Time slash will attack repeat
    float currentSpawnOffset;       //If more than 2 slash weapon it will start offset it upward

    protected override bool Attack(int AttackCount = 1)
    {
        //If no projectile prefab assigned , leave and warning
        if (!currentStats.projectilePrefab)
        {
            Debug.Log(string.Format("Projectile prefab has not been set for {0}" , name));
            currentCooldown = currentStats.cooldown;
            return false;
        }

        //If no projectile prefab assigned set the weapon on cool down
        if (!CanAttack())
        {
            return false;
        }

        //If first time attack the weapon , reset the currentspawncount and currentspawnoffset
        if (currentCooldown <= 0)
        {
            currentSpawnCount = 0;
            currentSpawnOffset = 0f;
        }

        //Calulate the angle and offset of our spawned projectile
        //If currentspawncount is even , flip the direction of the weapon
        float spawnDirection = Mathf.Sign(playermovement.LastMovementVector.x) * (currentSpawnCount % 2 != 0 ? -1 : 1);     //If -1 will spawn left of player and if 1 will spawn right of player
        Vector2 spawnOffset = new Vector2(spawnDirection * Random.Range(currentStats.spawnVarient.xMin, currentStats.spawnVarient.xMax), currentSpawnOffset);

        //spawn a copy of the weapon
        Projectile prefab = Instantiate(currentStats.projectilePrefab , playerStats.transform.position + (Vector3)spawnOffset , Quaternion.identity);

        //Set ourselves to be the owner
        prefab.player = playerStats;

        //Fliip the projectile sprite
        if (spawnDirection < 0)
        {
            prefab.transform.localScale = new Vector3 (-Mathf.Abs(prefab.transform.localScale.x) , prefab.transform.localScale.y , prefab.transform.localScale.z);
        }

        //Assign the stats 
        prefab.weapon = this;
        currentCooldown += currentStats.cooldown;
        AttackCount--;

        //Check where the next projectile should spawn
        currentSpawnCount++;
        if (currentSpawnCount > 1 && currentSpawnCount % 2 == 0)
        {
            currentAttackCount = AttackCount;
            currentAttackInterval = currentStats.projectileInterval;
        }

        if (AttackCount > 0)
        {
            currentAttackCount = AttackCount;
            currentAttackInterval = currentStats.projectileInterval;
        }

        return true;

    }
}

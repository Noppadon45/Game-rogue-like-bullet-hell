using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aura : WeaponEffect
{
    Dictionary<EnemyStats , float> affectedTargets = new Dictionary<EnemyStats, float> ();
    List<EnemyStats> targetstoUnaffect = new List<EnemyStats>();

    void Update()
    {
        Dictionary<EnemyStats, float> affectedTargsCopy = new Dictionary<EnemyStats, float>(affectedTargets);

        foreach (KeyValuePair<EnemyStats, float> pair in affectedTargsCopy) 
        {
            affectedTargets[pair.Key] -= Time.deltaTime;
            if (pair.Value <= 0)
            {
                if (targetstoUnaffect.Contains(pair.Key))
                {
                    affectedTargets.Remove(pair.Key);
                    targetstoUnaffect.Remove(pair.Key);
                }
                else
                {
                    Weapon.Stats stats = weapon.GetStats();
                    affectedTargets[pair.Key] = stats.cooldown;
                    pair.Key.TakeDamage(GetDamage(), transform.position, stats.knockback);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyStats es))
        {
            if (affectedTargets.ContainsKey(es))
            {
                targetstoUnaffect.Add(es);
            }
        }
    }
}

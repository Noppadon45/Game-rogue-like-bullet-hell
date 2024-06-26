using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraBehaviour : MeleeWeaponBehaviour
{
    List<GameObject> markedEnemies;
    protected override void Start()
    {
        base.Start();
        markedEnemies = new List<GameObject>();
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy") && !markedEnemies.Contains(col.gameObject))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);

            markedEnemies.Add(col.gameObject);      //Market Cant take damage twice in roll
        }
        else if (col.CompareTag("Prop"))
        {
            if (col.gameObject.TryGetComponent(out BreakbleProp breakble) && !markedEnemies.Contains(col.gameObject))
            {
                breakble.TakeDamage(currentDamage);
                markedEnemies.Add(col.gameObject);
            }
        }
    }

}

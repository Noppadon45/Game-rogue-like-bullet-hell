using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    EnemyStats enemyStats;
    Transform player;

    Vector2 knockbackVelocity;
    float knockbackDuration;

    // Start is called before the first frame update
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (knockbackDuration > 0)
        {
            transform.position += (Vector3)knockbackVelocity * Time.deltaTime;
            knockbackDuration -= Time.deltaTime;
        }else
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, enemyStats.CurrentMoveSpeed * Time.deltaTime);       //Eneme move to player
        }
        
    }

    public void Knockback(Vector2 velocity , float duration)
    {
        if (knockbackDuration > 0 )
        {
            return;
        }
        
        knockbackVelocity = velocity;
        knockbackDuration = duration;
    }
}

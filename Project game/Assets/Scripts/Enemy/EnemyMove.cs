using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    EnemyStats enemyStats;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        enemyStats = GetComponent<EnemyStats>();
        player = FindObjectOfType<PlayerMovement>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.transform.position , enemyStats.CurrentMoveSpeed * Time.deltaTime);       //Eneme move to player
    }
}

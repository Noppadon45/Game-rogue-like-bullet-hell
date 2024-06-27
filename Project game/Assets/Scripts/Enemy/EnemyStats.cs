using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject EnemyData;

    //Current Stats
    [HideInInspector]
    public float CurrentMoveSpeed;
    [HideInInspector]
    public float CurrentHealth;
    [HideInInspector]
    public float CurrentDamage;

    public float deSpawnDistance = 20f;
    Transform player;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform;
    }
    void Update()
    {
        if (Vector2.Distance(transform.position, player.position) >= deSpawnDistance) 
        {
            SpawnNearPlayer();
        }
    }
    void Awake()
    {
        CurrentMoveSpeed = EnemyData.MoveSpeed;
        CurrentHealth = EnemyData.MaxHealth;
        CurrentDamage = EnemyData.Damage;
    }

    public void TakeDamage(float Damage)
    {
        CurrentHealth -= Damage;

        if (CurrentHealth <= 0)
        {
            Kill();
        }
    }

    public void Kill()
    {
        Destroy(gameObject);
    }


    // Reference from PlayerStats takeDamage()
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.takeDamage(CurrentDamage);       //use EnemyData
        }
    }

    private void OnDestroy()
    {
        EnemySpawner EnemySpawner = FindObjectOfType<EnemySpawner>();
        EnemySpawner.Enemygetkill();
    }

    //Spawn enemy again when enemy go far Player
    void SpawnNearPlayer()
    {
        EnemySpawner enemySpawner = FindObjectOfType<EnemySpawner>();
        transform.position = player.position + enemySpawner.SpawnPositonEnemy[Random.Range(0 , enemySpawner.SpawnPositonEnemy.Count)].position;
    }
}
    
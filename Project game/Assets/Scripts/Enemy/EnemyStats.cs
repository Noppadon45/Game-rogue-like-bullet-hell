using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(SpriteRenderer))]
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

    [Header("Damage Feedback")]
    public Color damageColor = new Color(1, 0, 0, 1);
    public float damageFlashDuration = 0.2f;
    public float deathFadeTime = 0.6f;
    Color originColor;
    SpriteRenderer sr;
    EnemyMove movement;

    void Start()
    {
        player = FindObjectOfType<PlayerStats>().transform; // Find player in scene
        sr = GetComponent<SpriteRenderer>();
        originColor = sr.color;

        movement = GetComponent<EnemyMove>();       // Reference to movement script
    }
    void Update()
    {
        // If enemy is too far from the player, reposition closer
        if (Vector2.Distance(transform.position, player.position) >= deSpawnDistance) 
        {
            SpawnNearPlayer();
        }
    }
    void Awake()
    {
        // Initialize enemy stats from scriptable object
        CurrentMoveSpeed = EnemyData.MoveSpeed;
        CurrentHealth = EnemyData.MaxHealth;
        CurrentDamage = EnemyData.Damage;
    }

    // Called when the enemy takes damage
    public void TakeDamage(float Damage , Vector2 sourcePosition , float knockbackforce = 5f , float knockbackDuration = 0.2f)
    {
        CurrentHealth -= Damage;
        StartCoroutine(DamageFlash());

        // Show damage popup
        if (Damage > 0)
        {
            GameManager.DamagePopUp(Mathf.FloorToInt(Damage).ToString(), transform);
        }
        
        //Knockback if > 0
        if (knockbackforce > 0)
        {
            //Get direction of knockback
            Vector2 dir = (Vector2)transform.position - sourcePosition;
            movement.Knockback(dir.normalized * knockbackforce, knockbackDuration);
        }

        //Kill Enemy if Health <= 0
        if (CurrentHealth <= 0)
        {
            Kill();
        }
    }

    //fade out and destroy enemy when dead
    IEnumerator Killfade()
    {
        WaitForEndOfFrame w = new WaitForEndOfFrame();
        float t = 0, orgin = sr.color.a;
        while (t < deathFadeTime) {
            yield return w;
            t += Time.deltaTime;

            sr.color = new Color(sr.color.r , sr.color.g , sr.color.b, (1 - t / deathFadeTime) * orgin);
        }

        Destroy(gameObject);
    }

    //flash red briefly when damaged
    IEnumerator DamageFlash()
    {
        sr.color = damageColor;
        yield return new WaitForSeconds(damageFlashDuration);
        sr.color = originColor;
    }

    // Triggers the fade-out death animation
    public void Kill()
    {
        StartCoroutine(Killfade());
    }


    // Reference from PlayerStats takeDamage()
    public void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerStats player = collision.gameObject.GetComponent<PlayerStats>();
            player.takeDamage(CurrentDamage);       //use EnemyData
        }
    }

    // Notify spawner that this enemy has died
    public void OnDestroy()
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
    
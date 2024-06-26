using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : MonoBehaviour
{
    public EnemyScriptableObject EnemyData;

    //Current Stats
    float CurrentMoveSpeed;
    float CurrentHealth;
    float CurrentDamage;

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
}

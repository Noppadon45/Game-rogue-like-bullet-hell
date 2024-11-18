using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : WeaponEffect
{
    public enum DamageSource { projectile, owner };
    public DamageSource damageSource = DamageSource.projectile;     //Control how knockback is calcurate for the enemy when the projectile hit
    public bool isAutoAim = false;                              //Can select Auto aim to enemy or disable
    public Vector3 rotationSpeed = new Vector3(0, 0, 0);

    protected Rigidbody2D rb;
    protected int piercing;
 
    protected virtual void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        Weapon.Stats stats = weapon.GetStats();

        if (rb.bodyType == RigidbodyType2D.Dynamic)
        {
            rb.angularVelocity = rotationSpeed.z;
            rb.velocity = transform.right * stats.speed;
        }

        float area = stats.area == 0 ? 1 : stats.area;

        transform.localScale = new Vector3(
            area * Mathf.Sign(transform.localScale.x),
            area * Mathf.Sign(transform.localScale.y),
            1
        );

        piercing = stats.piercing;

        if (stats.lifetime > 0)
        {
            Destroy(gameObject, stats.lifetime);
        }

        if (isAutoAim) 
        {
            AutoAim();
        }
        
    }


    public virtual void AutoAim()
    { 
        float aimAngle;
        EnemyStats[] targets = FindObjectsOfType<EnemyStats>();

        if (targets.Length > 0)
        {
            EnemyStats selectedTarget = targets[Random.Range(0, targets.Length)];
            Vector2 different = selectedTarget.transform.position - transform.position;
            aimAngle = Mathf.Atan2(different.y, different.x) * Mathf.Rad2Deg;
        }
        else
        {
            aimAngle = Random.Range(0f, 360f);
        }

        transform.rotation = Quaternion.Euler(0, 0, aimAngle);
    }
       
    protected virtual void FixedUpdate()        //called method everyfream if WeaponEffect is Enable
    {
        if (rb.bodyType == RigidbodyType2D.Kinematic)
        {
            Weapon.Stats stats = weapon.GetStats();
            transform.position += transform.right * stats.speed * Time.fixedDeltaTime;
            rb.MovePosition(transform.position);
            transform.Rotate(rotationSpeed * Time.fixedDeltaTime);
        }
    }
    

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        EnemyStats enemyStats = other.GetComponent<EnemyStats>();
        BreakbleProp breakbleProp = other.GetComponent<BreakbleProp>();

        if (enemyStats)
        {
            Vector3 source = damageSource == DamageSource.owner && player ? player.transform.position : transform.position;
            
            enemyStats.TakeDamage(GetDamage(), source);

            Weapon.Stats stats = weapon.GetStats();
            piercing--;
            if (stats.hitEffect)
            {
                Destroy(Instantiate(stats.hitEffect , transform.position , Quaternion.identity), 5f);
            }
        }
        else if (breakbleProp)
        {
            breakbleProp.TakeDamage(GetDamage());
            piercing--;

            Weapon.Stats stats = weapon.GetStats();
            if (stats.hitEffect)
            {
                Destroy(Instantiate(stats.hitEffect, transform.position, Quaternion.identity), 5f);
            }
        }

        if (piercing <= 0)
        {
            Destroy(gameObject);
        }
    }



}
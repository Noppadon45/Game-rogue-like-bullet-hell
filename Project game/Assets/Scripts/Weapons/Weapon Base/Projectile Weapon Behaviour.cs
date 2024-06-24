using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

//Script Projectile Behaviour [Place on prefab weapons projectiles]
public class ProjectileWeaponBehaviour : MonoBehaviour
{
    public WeaponScriptableObject WeaponData;
    protected Vector3 direction;
    public float DestroyAfterSecound;

    protected float currentDamage;
    protected float currentSpeed;
    protected float currentCooldownDuraton;
    protected float currentPierce;

    void Awake()
    {
        currentDamage = WeaponData.damage;
        currentSpeed = WeaponData.speed;
        currentCooldownDuraton = WeaponData.coolDownDuration;
        currentPierce = WeaponData.Pierce;
    }

    protected virtual void Start()
    {
        Destroy(gameObject, DestroyAfterSecound);
    }
    public void DirectionChecker(Vector3 Dir)
    {
        direction = Dir;

        float dirx = direction.x;
        float diry = direction.y;

        Vector3 scale = transform.localScale;
        Vector3 rotation = transform.rotation.eulerAngles;

        if (dirx < 0 && diry == 0) //left
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
        }
        else if (dirx > 0 && diry == 0) //right
        {
            scale.x = scale.x * 1;
            scale.y = scale.y * 1;
        }
        else if (dirx == 0 && diry > 0) //up
        {
            rotation.z = 90f;
        }
        else if (dirx == 0 && diry < 0) //down
        {
            rotation.z = -90f;
        }
        else if (dirx < 0 && diry > 0) //left up
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = -45f;
        }
        else if (dirx < 0 && diry < 0) //left down
        {
            scale.x = scale.x * -1;
            scale.y = scale.y * -1;
            rotation.z = 45f;
        }
        else if (dirx > 0 && diry > 0) //right up
        {
            scale.x = scale.x * 1;
            scale.y = scale.y * 1;
            rotation.z = 45f;
        }
        else if (dirx > 0 && diry < 0) //right down
        {
            scale.x = scale.x * 1;
            scale.y = scale.y * 1;
            rotation.z = -45f;
        }


        transform.localScale = scale;
        transform.rotation = Quaternion.Euler(rotation);
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Enemy"))
        {
            EnemyStats enemy = col.GetComponent<EnemyStats>();
            enemy.TakeDamage(currentDamage);        //check WeaponData.damage 
            ReducePierce();
        }
    }

    void ReducePierce() //Destroy when the pierce hit 0
    {
        currentPierce--;
        if (currentPierce <= 0)
        {
            Destroy(gameObject);
        }
    }
}

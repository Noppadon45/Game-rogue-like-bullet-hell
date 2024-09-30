    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // Start is called before the first frame update
    [System.Serializable]
    public struct Stats
    {

        public string name;
        public string description;

        [Header("Visual Weapon")]
        //public Projectile projectilePrefab; //When attach projectile prefab Projectile prefab will spawn every time cooldown reach
        //public Aura auraPrefab;  //Aura prefab Aura prefab will spawn weapon had equip
        public ParticleSystem hitEffect;
        public Rect spawnVarient;

        [Header("Value")]
        public float lifetime;
        public float damage;
        public float damageVariance;
        public float area;
        public float speed;
        public float cooldown;
        public float projectileInterval;
        public float knockback;

        public int number;
        public int piercing;
        public int maxInstance;

        public static Stats operator +(Stats s1, Stats s2)
        {
            Stats result = new Stats();
            result.name = s2.name ?? s1.name;
            result.description = s2.description ?? s1.description;
            //result.projectilePrefab = s2.projectilePrefab ?? s1.projectilePrefab;
            //result.auraPrefab = s2.auraPrefab ?? s1.auraPrefab;
            result.hitEffect = s2.hitEffect == null ? s1.hitEffect : s2.hitEffect ; ;
            result.spawnVarient = s2.spawnVarient;
            result.lifetime = s1.lifetime + s2.lifetime;
            result.damage = s1.damage + s2.damage;
            result.damageVariance = s1.damageVariance + s2.damageVariance;
            result.area = s1.area + s2.area;
            result.speed = s1.speed + s2.speed;
            result.cooldown = s1.cooldown + s2.cooldown;
            result.number = s1.number + s2.number;
            result.piercing = s1.piercing + s2.piercing;
            result.projectileInterval = s1.projectileInterval + s2.projectileInterval;
            result.knockback = s1.knockback + s2.knockback;

            return result;
        }

        public float GetDamage()
        {
            return damage + Random.Range(0 , damageVariance);
        }
    }

}

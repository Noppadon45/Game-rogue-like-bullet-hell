using UnityEngine;

public abstract class Weapon : Item
{
    // Start is called before the first frame update 
    [System.Serializable]
    public struct Stats
    {

        public string name;
        public string description;

        [Header("Visual Weapon")]
        public Projectile projectilePrefab; //When attach projectile prefab Projectile prefab will spawn every time cooldown reach
        public Aura auraPrefab;  //Aura prefab Aura prefab will spawn weapon had equip
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
            result.projectilePrefab = s2.projectilePrefab ?? s1.projectilePrefab;
            result.auraPrefab = s2.auraPrefab ?? s1.auraPrefab;
            result.hitEffect = s2.hitEffect == null ? s1.hitEffect : s2.hitEffect;
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



    protected Stats currentStats;

    public WeaponData data;     //Get Weapon Data from ScriptableObject Script

    protected float currentCooldown;

    protected PlayerMovement playermovement;        //Ref Current player movement


    //Every Start Game Initialise Current Weapon
    public virtual void Initialise(WeaponData data)
    {
        base.Initialise(data);
        this.data = data;
        currentStats = data.baseStats;
        playermovement = GetComponentInParent<PlayerMovement>();
        currentCooldown = currentStats.cooldown;
    }

    //Assign the CurrentStats from WeaponData Script
    protected virtual void Awake()
    {
        if (data)
        {
            currentStats = data.baseStats;
        }
    }

    
    protected virtual void Start()
    {
        //If have WeaponData Script Initialise it
        if (data)
        {
            Initialise(data);
        }
    }

    protected virtual void Update()
    {
        //Attack when current cooldown is < 0f
        currentCooldown -= Time.deltaTime;
        if (currentCooldown <= 0f)
        {
            Attack(currentStats.number);
        }
    }

    
    
    public override bool DoLevelUp()
    {
        base.DoLevelUp();
        if (!CanLevelUp())
        {
            Debug.Log(string.Format("Cant level up {0} to levelup {1} , max level of {2} already reached.", name, currentLevel, data.maxLevel));
            return false;
        }
        //If Can Level Up Add that Stats to the Weapon Script
        currentStats += data.GetLevelData(++currentLevel);
        return true;
    }

    public virtual bool CanAttack()
    {
        return currentCooldown <= 0;
    }

    //Check if current weapon can attack and add Weapon cooldown Stats to the current cooldown for Perform it
    protected virtual bool Attack(int AttackNumber = 1)
    {
        if (CanAttack())
        {
            currentCooldown += currentStats.cooldown;
            return true;
        }
        return false;
    }


    //Get Damage weapon stats and calulate with currentmight of playerStats Script
    public virtual float GetDamage()
    {
        return currentStats.GetDamage() * playerStats.CurrentMight;
    }

    public virtual Stats GetStats()
    {
        return currentStats;
    }

}

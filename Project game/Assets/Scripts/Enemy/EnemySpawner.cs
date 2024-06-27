using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string WaveName;
        public List<EnemyGroup> EnemyGroups;        //list of All EnemyGroup 
        public int WaveQuota;   //All total Enemy spawn for this wave
        public float Spawntime; //Time to Spawn 
        public int Spawncount;  //All total Enemy in game
    }

    [System.Serializable ]
    public class EnemyGroup
    {
        public string EnemyName;        //EnemyName for this group
        public int EnemyCount;      //EnemyCount for this group
        public int Spawncount;
        public GameObject EnemyPrefab;      //List of EnemyPrefab for this group
    }

    public List<Wave> Waves;    //List of All wave
    public int currentWave;         //Index of currentWave [Index start 0]

    [Header("SpawnerTimer")]
    float spawnTimer;       //Time to Spawn next Wave
    public int EnemyAlive;
    public int MaxEnemyAlive;   //The maximum of Enemy alive in game
    public bool IsMaxEnemyAlive = false;




    Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<PlayerStats>().transform;
        CalWaveQuota();
    }

    // Update is called once per frame
    void Update()
    {
        //Timer
        spawnTimer += Time.deltaTime;

        //If Timer >= Time to SpawnWave then SpawnEnemy
        if (spawnTimer >= Waves[currentWave].Spawntime ) 
        {
            spawnTimer = 0f;
            SpawnEnemy();
        }
    }


    void CalWaveQuota()
    {
        int CurrentWaveQuota = 0;
        foreach (var EnemyGroup in Waves[currentWave].EnemyGroups) 
        {
            CurrentWaveQuota += EnemyGroup.EnemyCount;

        }
        Waves[currentWave].WaveQuota = CurrentWaveQuota;
        Debug.LogWarning(CurrentWaveQuota);
    }

    void SpawnEnemy()
    {
        //If current Enemy in Waves < Enemy in Quota
        if (Waves[currentWave].Spawncount < Waves[currentWave].WaveQuota  && !IsMaxEnemyAlive)
        {
            //Spawn Enemy Each of type until Quota
            foreach (var EnemyGroups in Waves[currentWave].EnemyGroups)
            {
                //When Enemy Each of type < Emeny Each of type spawn
                if (EnemyGroups.Spawncount < EnemyGroups.EnemyCount)
                {
                    //Limit Enemy number can alive
                    if (EnemyAlive >= MaxEnemyAlive)
                    {
                        IsMaxEnemyAlive = true;
                        return;
                    }
                    Vector2 SpawnPositon = new Vector2(Player.transform.position.x + Random.Range(-10f, 10f) , Player.transform.position.y + Random.Range(-10f, 10f));
                    Instantiate(EnemyGroups.EnemyPrefab, SpawnPositon, Quaternion.identity);

                    EnemyGroups.Spawncount++;
                    Waves[currentWave].Spawncount++;
                    EnemyAlive++;
                }
            }
        }
        //Reset IsMaxEnemyAlive when IsMaxEnemyAlive is below Maximum
        if (EnemyAlive < MaxEnemyAlive)
        {
            IsMaxEnemyAlive = false;
        }
    }
    public void Enemygetkill()
    {
        //
        EnemyAlive--;
    }
}

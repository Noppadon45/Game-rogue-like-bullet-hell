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
    public float waveInterval;
    public int MaxEnemyAlive;   //The maximum of Enemy alive in game
    public bool IsMaxEnemyAlive = false;
    bool IsWaveActive = false;
    

    [Header("Spawn Positons")]
    public List<Transform> SpawnPositonEnemy;   //Store List of Positon Enemy Spawn


    Transform Player;
    // Start is called before the first frame update
    void Start()
    {
        Player = FindObjectOfType<PlayerStats>().transform; // Find the player in the scene
        CalWaveQuota();              // Calculate how many enemies to spawn in current wave
    }

    // Update is called once per frame
    void Update()
    {
        // Check if wave has finished spawning and prepare the next one
        if (currentWave < Waves.Count && Waves[currentWave].Spawncount == 0 && !IsWaveActive)
        {
            StartCoroutine(BeginNextWave());
        }
        // Count time for next spawn
        spawnTimer += Time.deltaTime;

        //If Timer >= Time to SpawnWave then SpawnEnemy
        if (spawnTimer >= Waves[currentWave].Spawntime ) 
        {
            spawnTimer = 0f;
            SpawnEnemy();
        }
    }
        
    IEnumerator BeginNextWave()
    {
        IsWaveActive = true;
        //Wait for the next wave current
        yield return new WaitForSeconds(waveInterval);

        if (currentWave < Waves.Count - 1)
        {
            IsWaveActive = false;   
            currentWave++;
            CalWaveQuota();
        }
    }

    // Calculates how many enemies to spawn in the current wave
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
                    
                    //Spawn Enemy random nearby Player
                    Instantiate(EnemyGroups.EnemyPrefab, Player.position + SpawnPositonEnemy[Random.Range(0 , SpawnPositonEnemy.Count)].position, Quaternion.identity);
                
                    EnemyGroups.Spawncount++;
                    Waves[currentWave].Spawncount++;
                    EnemyAlive++;

                    //Limit Enemy number can alive
                    if (EnemyAlive >= MaxEnemyAlive)
                    {
                        IsMaxEnemyAlive = true;
                        return;
                    }
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
        //Decrease Enemy Alive
        EnemyAlive--;

        //Reset IsMaxEnemyAlive when IsMaxEnemyAlive is below Maximum
        if (EnemyAlive < MaxEnemyAlive)
        {
            IsMaxEnemyAlive = false;
        }
    }
}

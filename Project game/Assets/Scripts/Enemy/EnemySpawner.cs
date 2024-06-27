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
        public int WaveQuata;   //All total Enemy spawn for this wave
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

    public List<Wave> waves;    //List of All wave

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

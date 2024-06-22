using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public List<GameObject> TerrainChunks;
    public GameObject Player;
    public float checkerRadius;
    public LayerMask terrainMask;
    public GameObject currentChunk;
    Vector3 PlayerLastPosition;

    [Header("Optimization")]
    public List<GameObject> SpawnChunks;
    GameObject latestchunks;
    public float MaxOpDist;
    float OpDist;
    float optimizerCoolDown;
    public float optimizerCoolDownduration;
    // Start is called before the first frame update
    void Start()
    {
        PlayerLastPosition = Player.transform.position;
        UpdateCurrentChunk();
    }

    // Update is called once per frame
    void Update()
    {
        ChunkChecker();
        ChunkOptimization();
    }

    void ChunkChecker()
    {
        if (!currentChunk)
        {
            return;
        }
        Vector3 MoveDirection = Player.transform.position - PlayerLastPosition;
        PlayerLastPosition = Player.transform.position;

        UpdateCurrentChunk();

        string DirectionName = GetDirectionName(MoveDirection);

        if (DirectionName.Equals("Up") || DirectionName.Equals("Down"))
        {
            CheckAndSpawnChunk(DirectionName);
            CheckAndSpawnChunk("Left " + DirectionName);
            CheckAndSpawnChunk("Right " + DirectionName);
        }
        else if (DirectionName.Equals("Left") || DirectionName.Equals("Right"))
        {
            CheckAndSpawnChunk(DirectionName);
            CheckAndSpawnChunk(DirectionName + " Up");
            CheckAndSpawnChunk(DirectionName + " Down");
        }
        else
        {
            string[] subDirectionNames = DirectionName.Split(' ');
            CheckAndSpawnChunk(DirectionName);
            CheckAndSpawnChunk(subDirectionNames[0]);
            CheckAndSpawnChunk(subDirectionNames[1]);
        }
    }
    void CheckAndSpawnChunk(string Direction)
    {
        if (!Physics2D.OverlapCircle(currentChunk.transform.Find(Direction).position, checkerRadius, terrainMask))
        {
            SpawnChunk(currentChunk.transform.Find(Direction).position);
        }
    }
     string GetDirectionName(Vector3 Direction)
        {
         Direction = Direction.normalized;

        if (Mathf.Abs(Direction.x) > Mathf.Abs(Direction.y))
            {
            if (Direction.y > 0.5f)
            {
               return Direction.x > 0 ? "Right Up" : "Left Up";
            }
            else if (Direction.y < -0.5f)
            {
               return Direction.x > 0 ? "Right Down" : "Left Down";
            }
            else
            {
                return Direction.x > 0 ? "Right" : "Left";
            }
        }
        else
        {
            if (Direction.x > 0.5f)
            {
                return Direction.y > 0 ? "Right Up" : "Right Down";
            }
            else if (Direction.x < -0.5f)
            {
                return Direction.y > 0 ? "Left Up" : "Left Down";
            }
            else
            {
                return Direction.x > 0 ? "Up" : "Down";
            }
        }
    }
    void SpawnChunk(Vector3 spawnPositon)
    {
        int rand = Random.Range(0 , TerrainChunks.Count);
        latestchunks = Instantiate(TerrainChunks[rand], spawnPositon, Quaternion.identity);
        SpawnChunks.Add(latestchunks);

    }

    void ChunkOptimization()
    {
        optimizerCoolDown -= Time.deltaTime;
        if (optimizerCoolDown <= 0f)
        {
            optimizerCoolDown = optimizerCoolDownduration;
        }
        else
        {
            return;
        }
        foreach (GameObject Chunk in SpawnChunks)
        {
            OpDist = Vector3.Distance(Player.transform.position, Chunk.transform.position);
            if (OpDist > MaxOpDist)
            {
                Chunk.SetActive(false);
            }
            else
            {
                Chunk.SetActive(true);
            }
        }
    }
    void UpdateCurrentChunk()
    {
        Collider2D hitCollider = Physics2D.OverlapCircle(Player.transform.position, checkerRadius, terrainMask);
        if (hitCollider != null)
        {
            currentChunk = hitCollider.gameObject;
        }
        else
        {
            Debug.LogError("Current Chunk not found at player start position");
        }
    }

}

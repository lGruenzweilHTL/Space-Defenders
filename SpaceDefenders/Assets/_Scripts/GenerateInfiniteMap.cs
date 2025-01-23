using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateInfiniteMap : MonoBehaviour
{
    const int CHUNK_SPAWN_DISTANCE = 25;
    const int CHUNK_DELETE_DISTANCE = 50;
    
    [SerializeField] private Transform player;

    [SerializeField] private GameObject sampleChunk;

    private List<GameObject> allChunks;

    private GameObject currentChunk;
    void Awake()
    {
        allChunks = new List<GameObject>();
        GameObject firstChunk = Instantiate(sampleChunk, transform);
        allChunks.Add(firstChunk);
        currentChunk = firstChunk;
    }

    void Update()
    {
        //Determine current Chunk
        foreach (GameObject chunkObj in allChunks)
        {
            Transform center = chunkObj.GetComponent<ChunkData>().centerPoint.transform;
            if(Mathf.Abs(player.position.x - center.position.x) <= 25 && 
               Mathf.Abs(player.position.y - center.position.y) <= 25)
            {
                currentChunk = chunkObj;
            }
        }

        ChunkData data = currentChunk.GetComponent<ChunkData>();

        #region Spawn Chunks

        //Right
        if (Mathf.Abs(player.position.x - data.rightBorder.position.x) < CHUNK_SPAWN_DISTANCE)
        {
            //spawn new chunk
            Vector2 spawnPos = new Vector2(data.rightBorder.position.x + 24, data.transform.position.y);     
            SpawnChunkAtPosition(spawnPos);
        }

        //Left
        if (Mathf.Abs(player.position.x - data.leftBorder.position.x) < CHUNK_SPAWN_DISTANCE)
        {
            //spawn new chunk
            Vector2 spawnPos = new Vector2(data.leftBorder.position.x - 24, data.transform.position.y);
            SpawnChunkAtPosition(spawnPos);
        }

        //Top
        if (Mathf.Abs(player.position.y - data.topBorder.position.y) < CHUNK_SPAWN_DISTANCE)
        {
            //spawn new chunk
            Vector2 spawnPos = new Vector2(data.transform.position.x, data.topBorder.position.y + 24);
            SpawnChunkAtPosition(spawnPos);
        }

        //Bottom
        if (Mathf.Abs(player.position.y - data.bottomBorder.position.y) < CHUNK_SPAWN_DISTANCE)
        {
            //spawn new chunk
            Vector2 spawnPos = new Vector2(data.transform.position.x, data.bottomBorder.position.y - 24);
            SpawnChunkAtPosition(spawnPos);
        }

        #endregion

        DeleteFarChunks();
    }

    void SpawnChunkAtPosition(Vector2 spawnPos)
    {
        for (int j = 0; j < allChunks.Count; j++)
        {
            if ((Vector2)allChunks[j].transform.position == spawnPos) return;
        }
        allChunks.Add(Instantiate(sampleChunk, spawnPos, Quaternion.identity, transform));
    }

    void DeleteFarChunks()
    {
        foreach (GameObject chunk in allChunks)
        {
            if (Vector2.Distance(player.position, chunk.GetComponent<ChunkData>().centerPoint.position) > CHUNK_DELETE_DISTANCE)
            {
                Destroy(chunk);
                allChunks.Remove(chunk);
                break;
            }
        }
    }
}
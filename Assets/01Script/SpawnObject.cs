﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    DumbbellCoin = 0,
    BookCoin,
    MicCoin,
    GameCoin,
    Obstacle01,
}

public enum ObjectPosition
{
    Ground,
    Air
}

public class SpawnObject : MonoBehaviour
{
    private int spawnRate;

    private ObjectPosition objectPos;

    private Vector3 obstacleOffset01 = new Vector3(0.0f, 1.85f, 0.0f);
    private void Start()
    {
        if (transform.parent.name == "GroundSpawnPos")
        {
            objectPos = ObjectPosition.Ground;
        }
        else if (transform.parent.name == "AirSpawnPos")
        {
            objectPos = ObjectPosition.Air;
        }
        SpawnRandomObject();
    }
    public void SpawnRandomObject()
    {
        spawnRate = Random.Range(1, 1000);
        
        if (objectPos == ObjectPosition.Ground)
        {
            if (spawnRate < 250)
            {
                SpawnObjectManager.instance.SpawnObject((int)ObjectType.Obstacle01, transform.position + obstacleOffset01);
            }
            else if (spawnRate < 400)
            {
                SpawnObjectManager.instance.SpawnObject((int)ObjectType.DumbbellCoin, transform.position);
            }
            else if (spawnRate < 550)
            {
                SpawnObjectManager.instance.SpawnObject((int)ObjectType.BookCoin, transform.position);
            }
            else if (spawnRate < 700)
            {
                SpawnObjectManager.instance.SpawnObject((int)ObjectType.MicCoin, transform.position);
            }
            else if (spawnRate < 850)
            {
                SpawnObjectManager.instance.SpawnObject((int)ObjectType.GameCoin, transform.position);
            }
        }
        else if (objectPos == ObjectPosition.Air)
        {
            if (spawnRate < 150)
            {
                SpawnObjectManager.instance.SpawnObject((int)ObjectType.DumbbellCoin, transform.position);
            }
            else if (spawnRate < 300)
            {
                SpawnObjectManager.instance.SpawnObject((int)ObjectType.BookCoin, transform.position);
            }
            else if (spawnRate < 450)
            {
                SpawnObjectManager.instance.SpawnObject((int)ObjectType.MicCoin, transform.position);
            }
            else if (spawnRate < 600)
            {
                SpawnObjectManager.instance.SpawnObject((int)ObjectType.GameCoin, transform.position);
            }
        }
    }
}

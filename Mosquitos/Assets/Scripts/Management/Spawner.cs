using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public EnemyPool[] pools;
    public float spawnDelay;

    public IEnumerator SpawnCicle()
    {
        while (true)
        {
            if (spawnDelay < .05f) spawnDelay = .05f;
            yield return new WaitForSeconds(spawnDelay);
            yield return new WaitWhile(() => Time.timeScale < 1);
            GameObject enemy = pools[0].GetEnemy(RandomizeSpawnPosition(), RandomizeSize());
        }
    }

    Vector2 RandomizeSpawnPosition()
    {
        int limitX = 12;
        int limitY = 8;

        Vector2 spawnPosition = Vector2.zero;

        if (CoinFlip()) {
            spawnPosition += limitX * (CoinFlip() ? Vector2.right : Vector2.left);
            float maxRange = limitY - 3;
            spawnPosition += Vector2.up * Random.Range(-maxRange, maxRange);
        } else {
            spawnPosition += limitY * (CoinFlip() ? Vector2.up : Vector2.down);
            float maxRange = limitX - 4;
            spawnPosition += Vector2.right * Random.Range(-maxRange, maxRange);
        }

        return spawnPosition;
    }

    Size RandomizeSize()
    {
        int 
            pChance = 7,
            mChance = 3,
            gChance = 1;
        
        Size s;
        int rng = Random.Range(0, pChance + mChance + gChance);
        if(rng < pChance) {
            s = Size.P;
        } else if(rng < pChance + mChance) {
            s = Size.M;
        } else {
            s = Size.G;
        }

        return s;
    }

    bool CoinFlip()
    {
        return Random.Range(0, 2) == 0 ? true : false;
    }
}

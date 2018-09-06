using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CicleManager : MonoBehaviour {

    public Stage[] stages;
    public EnemyPool[] pools;

    public void SetStage(Stage stage)
    {
        StopAllCoroutines();

        foreach(EnemyData ed in stage.enemyData)
        {
            int poolIndex;
            switch (ed.type)
            {
                default:
                case EnemyType.Random: poolIndex = 0; break;
                case EnemyType.Chaser: poolIndex = 1; break;
                case EnemyType.Aim:    poolIndex = 2; break;
            }
            if (ed.ratioPerMinute < 1) ed.ratioPerMinute = 1;
            StartCoroutine(SpawnCicle(pools[poolIndex], ed.size, 60 / ed.ratioPerMinute));
        }
    }

    public IEnumerator SpawnCicle(EnemyPool pool, Size size, float delay)
    {
        while (true)
        {
            if (delay < .05f) delay = .05f;
            yield return new WaitForSeconds(delay);
            yield return new WaitWhile(() => Time.timeScale < 1);
            GameObject enemy = pool.GetEnemy(RandomizeSpawnPosition(), size);
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

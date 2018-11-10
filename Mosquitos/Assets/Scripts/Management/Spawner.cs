using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameManager gameManager;
    public EnemyPool[] pools;

    static int activeQuantity;
    public static Spawner instance;

    private void Awake()
    {
        instance = this;
    }

    public void SetStage(Pattern stage)
    {
        StopAllCoroutines();
        StartCoroutine(StageCicle(stage.patternsData)); 
    }

    public IEnumerator StageCicle(List<PatternData> patternsData)
    {
        if (patternsData != null)
        foreach (PatternData pattern in patternsData)
        {
            int poolIndex;
            switch (pattern.type)
            {
                default:
                case EnemyType.Random: poolIndex = 0; break;
                case EnemyType.Chaser: poolIndex = 1; break;
                case EnemyType.Aim:    poolIndex = 2; break;
            }

            yield return SpawnFormation(pattern, pools[poolIndex]);
        }

        gameManager.CallStage();
    }

    public IEnumerator SpawnFormation(PatternData pattern, EnemyPool pool)
    {
        Vector3 guidepoint;
        float limitCoord;
        Vector3 lineDirection;
        switch (pattern.spawnSide)
        {
            case SpawnSide.Left:
                guidepoint = new Vector2(-12, 0);
                limitCoord = 6f;
                lineDirection = Vector3.up;
                break;

            case SpawnSide.Right:
                guidepoint = new Vector2(12, 0);
                limitCoord = 6f;
                lineDirection = Vector3.up;
                break;

            default:
                guidepoint = new Vector2(0, 8);
                limitCoord = 10f;
                lineDirection = Vector3.right;
                break;
        }

        lineDirection *= (pattern.invert ? 1 : -1);

        yield return new WaitForSeconds(pattern.activationTimer / PlayerAgeData.difficultyScaler);
        for(int i = 0; i < pattern.quantity; i++)
        {
            pool.GetEnemy(
                guidepoint + GetLinePosition(limitCoord, lineDirection, i, pattern.quantity), 
                SetSpawnRotation(pattern.spawnSide)
            );
            if(pattern.delayBetweenSpawns > 0)
            {
                yield return new WaitForSeconds(pattern.delayBetweenSpawns / PlayerAgeData.difficultyScaler);
            }
        }
    }

    Vector3 GetLinePosition(float limitCoord, Vector3 direction, int index, int quantity)
    {
        float linePos = (limitCoord * 2) / (quantity + 1) * (index + 1);
        linePos -= limitCoord;
        return direction * linePos;
    }

    float SetSpawnRotation(SpawnSide side)
    {
        switch (side)
        {
            case SpawnSide.Left:  return 270;
            case SpawnSide.Right: return 90;
            default:              return 180;
        }
    }

    bool CoinFlip()
    {
        return Random.Range(0, 2) == 0 ? true : false;
    }

    public void ChangeQuantity(int value)
    {
        activeQuantity += value;
        //if(activeQuantity < 1)
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
}

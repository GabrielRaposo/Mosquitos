using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PatternData")]
public class Pattern : ScriptableObject
{
    public List<PatternData> patternsData;
}

[System.Serializable]
public class PatternData
{
    public float activationTimer;
    public SpawnSide spawnSide;
    public int quantity;
    public bool invert;
    public float delayBetweenSpawns;
    public EnemyType type;
}

public enum EnemyType
{
    Line,
    Aim,
    Chaser,
    Random
}

public enum SpawnSide
{
    Up,
    Left,
    Right
}
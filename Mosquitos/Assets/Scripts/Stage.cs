using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "StageData")]
public class Stage : ScriptableObject
{
    public string title;
    public int duration;
    public List<EnemyData> enemyData;
}

[System.Serializable]
public class EnemyData
{
    public EnemyType type;
    public Size size;
    public float ratioPerMinute;
}

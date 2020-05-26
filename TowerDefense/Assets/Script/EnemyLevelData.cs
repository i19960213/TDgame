using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>敵人資料</summary>
[CreateAssetMenu(fileName = "Level", menuName = "-----MyTDGame-----/LevelData")]
public class EnemyLevelData : ScriptableObject
{
    [SerializeField]
    public List<EnemyData> list = new List<EnemyData>();
    [SerializeField]
    public int unlockMoney;
}
[System.Serializable]
public struct EnemyData
{
    public EnemyType enemyType;
    public float hp;
    public float speed;
    // 敵人細節
    public float cd;
    public int money;
}

public enum EnemyType
{
    CUBE,
}

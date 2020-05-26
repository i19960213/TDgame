using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>塔的資料</summary>
[CreateAssetMenu(fileName = "TowerAsset", menuName = "-----MyTDGame-----/TowerData")]
public class TowerAsset : ScriptableObject
{
    public Sprite icon = null;
    /// <summary>解鎖金額 (如果為0表示4內建的塔)</summary>
    public int unlockPrice = 0;
    public string storeName = "";
    /// <summary>是否已經解鎖</summary>
    public bool isUnlock = false;
    // 0級應該是塔售價+0元升級費 1級只有升級費
    //List 多功能 排序 自由增減東西無視記憶體大小
    //Array 連續的記憶體位置 < 這裡用的
    public TowerData[] list = new TowerData[0];
}

/// <summary>單一等級的資料</summary>
[System.Serializable]
public struct TowerData
{
    /// <summary>售價</summary>
    [Header("售價")] public int price;
    /// <summary>偵測範圍</summary>
    [Header("偵測範圍")] public float attackRange;
    /// <summary>攻速</summary>
    [Header("每秒攻速")] [Range(0f,10f)] public float attackSpeed;
    /// <summary>砲塔轉速</summary>
    [Header("砲塔轉速")] public float rotateSpeed;
    /// <summary>攻擊力</summary>
    [Header("攻擊力")] public float attack;
    /// <summary>子彈速度</summary>
    [Header("子彈速度")] public float bulletSpeed;
    /// <summary>子彈攻擊範圍</summary>
    [Header("攻擊濺射範圍")] public float bulletAttackRange;
    /// <summary>塔的模組</summary>
    [Header("模組")] public GameObject skin;

}

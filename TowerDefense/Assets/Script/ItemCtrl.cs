using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCtrl : MonoBehaviour
{
    /// <summary>初始化按鈕</summary>
    /// <param name="towerAsset">塔的資料</param>
    /// <param name="Act_Button">按鈕事件</param>
    public void Create(TowerAsset towerAsset, System.Action<TowerAsset> Act_Button)
    {
        this.towerAsset = towerAsset;
        this.Act_Button = Act_Button;
    }

    public TowerAsset towerAsset;
    System.Action<TowerAsset> Act_Button;


    //表層
    public void BuyMe()
    {
        // 呼叫按鈕事件並傳送我的資料進去
        Act_Button.Invoke(towerAsset);

    }
}


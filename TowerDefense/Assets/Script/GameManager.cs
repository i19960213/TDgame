using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary> 提供遊戲系統的功能 </summary>
public class GameManager 
{
    //單例
    public static GameManager instance
    {
        get 
        {
            if(_instance == null)
            {
                _instance = new GameManager();
            }
            return _instance;
        }
        set 
        {
            _instance = value;
        }
    }
    static GameManager _instance = null;

    //定義一個廣播格式 設定回傳值 要傳遞的東西
    public delegate void Dge_UpdateGameValue();
    //利用格式 建立一個事件
    public event Dge_UpdateGameValue Evt_UpdateGameValue;

    public int money
    {
        get { return _money; }
        set 
        { 
            _money = value;
            if (Evt_UpdateGameValue != null)
                Evt_UpdateGameValue.Invoke();
        }
    }
    int _money = 0;

    public int u_money
    {
        get { return SaveManager.instance.nowData.unloackMoney; }
        set
        {
            SaveManager.instance.nowData.unloackMoney = value;
            if (Evt_UpdateGameValue != null)
                Evt_UpdateGameValue.Invoke();
            SaveManager.instance.SaveGame(0);
        }
    }
    int _u_money = 0;


    public delegate void Dge_BuyButtonStatus(bool isOpen);
    /// <summary>廣播是否開啟購買按鈕(地板放置)</summary>
    public event Dge_BuyButtonStatus Evt_BuyButtonStatus;
    /// <summary>執行Evt_SetBuyButton廣播</summary>
    /// <param name="isOpen">是否開啟購買紐(地板放置)</param>
    public void SetBuyButtonStatus(bool isOpen)
    {
        if (Evt_BuyButtonStatus != null)
            Evt_BuyButtonStatus.Invoke(isOpen);
    }
 

    /// <summary>玩家選擇的塔</summary>
    public TowerAsset selectTowerAsset = null;

    public float selloff = 0.8f;

    public delegate void Dge_TowerChange();
    /// <summary>當塔的數量發生變化時 </summary>
    public event Dge_TowerChange Evt_TowerChange;
    /// <summary>通知塔的數量發生變化時</summary>
    public void TowerChange()
    {
        if(Evt_TowerChange != null)
            Evt_TowerChange.Invoke();
    }
}

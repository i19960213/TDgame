﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager
{
    public static SaveManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaveManager();
                _instance.Load();
            }
            return _instance;
        }
        set { _instance = value; }
    }
    static SaveManager _instance;

    /// <summary>存檔資料</summary>
    public AllSaveData allSaveData = new AllSaveData();
    /// <summary>當前資料</summary>
    public PlayerData nowData = new PlayerData();

    /// <summary>載入檔案</summary>
    void Load()
    {
        // 拉出原始檔(文字)
        string jsonData = PlayerPrefs.GetString("AllSaveData");
        if (jsonData == "")
        {
            // 無紀錄 需要手動初始話
            // 憑空做一個玩家資料
            PlayerData tempPlayerData = CreateNewPlayerData();
            // <<添加資料>>
            // 憑空做一個全體資料
            AllSaveData tempAllSaveData = new AllSaveData();
            tempAllSaveData.playerDataList = new List<PlayerData>();
            tempAllSaveData.playerDataList.Add(tempPlayerData);

            allSaveData = tempAllSaveData;
        }
        else
        {
            // 有紀錄 讀取近來吧
            allSaveData = JsonUtility.FromJson<AllSaveData>(jsonData);
        }
    }
    /// <summary>存檔</summary>
    /// <param name="number">第幾筆資料</param>
    public void SaveGame(int number)
    {
        // 把這筆存檔資料放進資料表裡面
        SaveManager.instance.allSaveData.playerDataList[number] = SaveManager.instance.nowData;
        // 呼叫SaveManager把所有資料塞進硬碟裡
        Save();
    }
    /// <summary>儲存檔案</summary>
    void Save()
    {
        // 把資料轉換成文字
        string jsonData = JsonUtility.ToJson(allSaveData);
        // 把文字存起來
        PlayerPrefs.SetString("AllSaveData", jsonData);
    }

    /// <summary>建立一筆新的玩家資料</summary>
    public PlayerData CreateNewPlayerData()
    {
        // 憑空做一個玩家資料
        PlayerData tempPlayerData = new PlayerData();
        tempPlayerData.unloackMoney = 200;
        tempPlayerData.unlockStuff = new List<string>();
        tempPlayerData.unlockStuff.Add("MachineGun");

        return tempPlayerData;
    }

    /// <summary>載入0號存檔</summary>
    public void LoadPlayerData_0()
    {
        // 從主資料裡面拉出0號資料出來用
        nowData = allSaveData.playerDataList[0];
    }

    public void Log()
    {
        // 測試將資料結構轉換成json字串
        Debug.Log(JsonUtility.ToJson(allSaveData, true));
    }

    /// <summary>此商品是否存在</summary>
    public bool IsExist(string name)
{
    bool isUnlock = false;
    for (int j = 0; j < SaveManager.instance.nowData.unlockStuff.Count; j++)
    {
        //檢查每一個解鎖過的東西 是否=我
        if (SaveManager.instance.nowData.unlockStuff[j] == name)
        {
            isUnlock = true;
            break; // 中斷迴圈
        }
    }
    return isUnlock;
    }
}

// ----------玩家紀錄檔的資料結構----------

/// <summary>所有要儲存的東西</summary>
[System.Serializable]
public struct AllSaveData
{
    [SerializeField]
    public List<PlayerData> playerDataList;
}

/// <summary>單筆玩家資料</summary>
[System.Serializable]
public struct PlayerData
{
    /// <summary>解鎖用的錢</summary>
    [SerializeField]
    public int unloackMoney;
    /// <summary>已解鎖的商品名</summary>
    [SerializeField]
    public List<string> unlockStuff;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AskWindows : MonoBehaviour
{
    
    public static AskWindows instance;
    private void Start()
    {
        instance = this;
    }
    [SerializeField] GameObject allStuff;
    [SerializeField] Text sayStuff;
    [SerializeField] GameObject buttonYes, buttonNo, buttonOk;

    TowerAsset myTowerAsset;
    public void Open(TowerAsset asset)
    {
        myTowerAsset = asset;
        allStuff.SetActive(true);
        if(GameManager.instance.u_money >= asset.unlockPrice)
        {
            sayStuff.text = "是否要購買「" + asset.storeName + "」？";

            buttonYes.SetActive(true);
            buttonNo.SetActive(true);
            buttonOk.SetActive(false);
        }
        else
        {
            sayStuff.text = "金錢不足！";

            buttonYes.SetActive(false);
            buttonNo.SetActive(false);
            buttonOk.SetActive(true);
        }
    }

    public void Yes()
    {
        for (int i =0; i< GameMenu.instance.storeList.Length; i++)
        {
            if(GameMenu.instance.storeList[i].name == myTowerAsset.name)
            {
                //遇到了要買的東西 扣錢
                GameManager.instance.u_money -= myTowerAsset.unlockPrice;
                //解鎖塔
                SaveManager.instance.nowData.unlockStuff.Add(GameMenu.instance.storeList[i].name);
                //存檔
                SaveManager.instance.SaveGame(0);
                // 刷新介面
                GameMenu.instance.UpdateUI();
                //把我自己關起來
                allStuff.SetActive(false);
            }
        }
    }
    public void No()
    {
        allStuff.SetActive(false);
    }
    public void Ok()
    {
        allStuff.SetActive(false);
    }
}

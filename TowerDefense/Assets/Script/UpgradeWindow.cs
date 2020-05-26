using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeWindow : MonoBehaviour
{
    public static UpgradeWindow instance = null;
    void Start()
    {
        instance = this;
        mainUI.SetActive(false);
    }

    [SerializeField] GameObject mainUI;
    TowerAsset mainAsset;
    TowerBase mainTower;
    Land mainLand;
    public void Open(TowerAsset mainAsset , TowerBase mainTower , Land mainLand)
    {
        // 指定資料
        this.mainAsset = mainAsset;
        this.mainTower = mainTower;
        this.mainLand = mainLand;
        // 第一次刷新資料
        UpdateUI();
        mainUI.SetActive(true);
    }
    public void Close()
    {
        mainUI.SetActive(false);

    }

    [SerializeField] Image icon;
    [SerializeField] Text levelInfo;
    [SerializeField] Text atkRgeInfo;
    [SerializeField] Text atkInfo;
    [SerializeField] Text atkSpdInfo;
    [SerializeField] Text upgradePriceInfo;
    [SerializeField] Text sellPriceInfo;
    [SerializeField] GameObject upgradeButton;

    void UpdateUI()
    {
        icon.sprite = mainAsset.icon;
        //可升級 (當等級小於升級資料的長度時)
        if (mainTower.level < mainAsset.list.Length -1 )
        {
            levelInfo.text = "Lv" + mainTower.level + " >> Lv" + (mainTower.level + 1);
            atkRgeInfo.text = "攻擊範圍" + mainAsset.list[mainTower.level].attackRange + " >> " +mainAsset.list[mainTower.level + 1].attackRange ;
            atkInfo.text = "攻擊力" + mainAsset.list[mainTower.level].attack + " >> " + mainAsset.list[mainTower.level + 1].attack;
            atkSpdInfo.text = "攻擊速度" + mainAsset.list[mainTower.level].attackSpeed + " >> " + mainAsset.list[mainTower.level + 1].attackSpeed;
            upgradePriceInfo.text = mainAsset.list[mainTower.level + 1].price.ToString();
            upgradeButton.SetActive(true);

        }
        else //顯示最高等級
        {
            levelInfo.text = "Lv " + mainTower.level ;
            atkRgeInfo.text = "攻擊範圍 " + mainAsset.list[mainTower.level].attackRange;
            atkInfo.text = "攻擊力 " + mainAsset.list[mainTower.level].attack ;
            atkSpdInfo.text = "攻擊速度 " + mainAsset.list[mainTower.level].attackSpeed ;
            upgradeButton.SetActive(false);

        }
        // 賣掉塔的總金額
        float totalMoney = 0f;
        for (int i = 0; i <= mainTower.level; i++)
        {
            totalMoney = totalMoney + mainAsset.list[i].price;
        }
        totalMoney = totalMoney * GameManager.instance.selloff;
        sellPriceInfo.text = Mathf.CeilToInt(totalMoney).ToString();
    }

    public void UpgradeButton()
    {
        if(GameManager.instance.money >= mainAsset.list[mainTower.level + 1].price)
        {
            GameManager.instance.money -= mainAsset.list[mainTower.level + 1].price;
            mainTower.UpdateTower(mainTower.level+ 1);
            // 再次刷新頁面
            UpdateUI();
        }
    }
    public void SellButton()
    {
        float totalMoney = 0f;
        for (int i = 0; i <= mainTower.level; i++)
        {
            totalMoney = totalMoney + mainAsset.list[i].price;
        }
        totalMoney = totalMoney * GameManager.instance.selloff;

        GameManager.instance.money += (int)totalMoney;
        //清理地板上的資料
        mainLand.CleanData();
        // 砍塔
        mainTower.KillMe();
        //主動關閉介面
        Close();
        // 賣塔了 要求所有AI重新尋找路線
        GameManager.instance.TowerChange();
    }
}



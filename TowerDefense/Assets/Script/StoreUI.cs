using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StoreUI : MonoBehaviour
{
    /// <summary>塔資料包</summary>
    [SerializeField] TowerAsset[] assetList = new TowerAsset[0];
    /// <summary>預設商品樣板</summary>
    [SerializeField] GameObject item = null;
    /// <summary>樣版的售價欄位</summary>
    [SerializeField] Text item_money = null;
    /// <summary>樣板的圖示</summary>
    [SerializeField] Image item_image = null;
    /// <summary>擺樣板的視窗</summary>
    [SerializeField] RectTransform menuTransform = null;
    void Start()
    {
        //先調整好樣板中參數 
        //再複製樣板
        for (int i = 0; i < assetList.Length; i++ )
        {
            // 多判斷一層 此商品是否存在
            if (SaveManager.instance.IsExist(assetList[i].name))
            {
                //用迴圈一個一個抓 把項目售價設定成第0筆資料的售價
                item_money.text = assetList[i].list[0].price.ToString("###,###");
                //指定按鈕圖示
                item_image.sprite = assetList[i].icon;
                //實作樣板
                GameObject tempItem = Instantiate(item, menuTransform);
                // 把0號塔的資料塞進按鈕控制
                tempItem.GetComponent<ItemCtrl>().Create(assetList[i], BuyButton);
                // 把複製出來的商品改成可顯示
                tempItem.SetActive(true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    /// <summary>要求買塔</summary>
    /// <param name="towerAsset">塔的資料包</param>
    void BuyButton(TowerAsset towerAsset)
    {
        if (GameManager.instance.money >= towerAsset.list[0].price)
        {
            //錢夠買塔時 進入放置模式
            //記下剛剛點甚麼
            GameManager.instance.selectTowerAsset = towerAsset;
            // 進行廣播叫地板開起按鈕
            GameManager.instance.SetBuyButtonStatus(true);
        }
    }

    /// <summary>取消按鈕</summary>
    public void CancelButton()
    {
        // 進行廣播叫地板關閉按鈕
        GameManager.instance.SetBuyButtonStatus(false);
    }
}

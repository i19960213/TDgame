using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    [SerializeField] GameObject buyButton = null;
    [SerializeField] GameObject upgradeButton = null;

    private void OnEnable()
    {
        GameManager.instance.Evt_BuyButtonStatus += BuyButtonStatus;
    }

    private void OnDisable()
    {
        GameManager.instance.Evt_BuyButtonStatus -= BuyButtonStatus;
    }

    private void Start()
    {
        buyButton.SetActive(false);
        upgradeButton.SetActive(false);
    }

    void BuyButtonStatus(bool isOpen) // 
    {
        //沒塔的處理 買開升級關
        if(myTowerBase == null)
        {
            buyButton.SetActive(isOpen);
            upgradeButton.SetActive(false);
        }
        //有塔的處理
        if(myTowerBase != null)
        {
            //有塔時 買關
            buyButton.SetActive(false);
            upgradeButton.SetActive(!isOpen);
        }
    }

    TowerBase myTowerBase;
    public void BuyButton()
    {
        //錢夠用 且自己身上沒有塔
        if (GameManager.instance.money >= GameManager.instance.selectTowerAsset.list[0].price && myTowerBase == null)
        {
            // 而且這裡不會擋到路
            if (NodeManager.instance.IsPathOk(this.transform.position))
            {
                // 扣錢
                GameManager.instance.money -= GameManager.instance.selectTowerAsset.list[0].price;
                
                //---------------------------------------------------------------------------------------------------------------------
                //動態利用TowerAsset名稱載入塔的物件 
                Object tempTower = Resources.Load("Tower/" + GameManager.instance.selectTowerAsset.name);
                if (tempTower == null)
                    Debug.LogError("檔名和資料不一致" + GameManager.instance.selectTowerAsset.name);
                //複製塔 並且成為我的子物件
                GameObject myTower = Instantiate((GameObject)tempTower, this.transform);
                //把我身上塔的程式抓起來備用
                myTowerBase = myTower.GetComponent<TowerBase>();
                // 修改塔的位置
                myTower.transform.localPosition = new Vector3(0f, 0.5f, 0f);
                //卸載Resources占用的記憶體
                //Resources.UnloadAsset(tempTower);
                //---------------------------------------------------------------------------------------------------------------------

                //  升級到0等
                myTowerBase.UpdateTower(0);

                //改變Tag狀態
                this.gameObject.tag = "Obstacle";

                //關閉買塔按鈕
                GameManager.instance.SetBuyButtonStatus(false);
                // 蓋塔了 要求所有AI重新尋找路線
                GameManager.instance.TowerChange();
            }


        }
    }
    /// <summary>升級按鈕</summary>
    public void UpgradeButton()
    {
        UpgradeWindow.instance.Open(myTowerBase.mainAsset , myTowerBase , this);
    }

    /// <summary>清理至乾淨狀態</summary>
    public void CleanData()
    {
        // 清掉本地資料
        myTowerBase = null;
        // 重製按鈕狀態到非購買過的情況
        BuyButtonStatus(false);
        //改變Tag狀態
        this.gameObject.tag = "Ok";
    }
}


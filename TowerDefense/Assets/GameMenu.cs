using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour, IUnityAdsListener
{
    public static GameMenu instance;
    public TowerAsset[] storeList = new TowerAsset[0];
    /// <summary>商店位置</summary>
    [SerializeField] RectTransform storeBg = null;
    /// <summary>商品</summary>
    [SerializeField] GameObject item = null;

    void Awake()
    {
        // 載入0號紀錄檔
        SaveManager.instance.LoadPlayerData_0();
    }

    void Start()
    {
        instance = this;

        //初始化廣告
        StartAD();
        UpdateUI();
    }

    List<GameObject> needKill = new List<GameObject>();

  public  void UpdateUI()
    {
        // 砍
        foreach(GameObject g in needKill)
        {
            Destroy(g);
        }
        //清除列表
        needKill.Clear();

        for (int i = 0; i < storeList.Length; i++)
        {
            GameObject tempObject = Instantiate(item, storeBg);
            // 被複製的東西都被放進刪除列表
            needKill.Add(tempObject);
            tempObject.SetActive(true);
            // 第一個子物件轉換成image 然後設定圖片
            tempObject.transform.GetChild(0).GetComponent<Image>().sprite = storeList[i].icon;
            tempObject.transform.GetChild(1).GetComponent<Text>().text = storeList[i].storeName;
            tempObject.transform.GetChild(2).GetComponent<Text>().text = storeList[i].unlockPrice.ToString();


            // 詢問存檔 此筆資料是否存在
            if (SaveManager.instance.IsExist(storeList[i].name))
            {
                //如果已經解鎖 就顯示鎖頭
                tempObject.transform.GetChild(3).localScale = Vector3.one;
                //讓金錢不顯示
                tempObject.transform.GetChild(2).localScale = Vector3.zero;
            }
            else
            {
                tempObject.transform.GetChild(3).localScale = Vector3.zero;
                //讓金錢顯示
                tempObject.transform.GetChild(2).localScale = Vector3.one;
            }
            //塞資料給按鈕
            tempObject.GetComponent<MainMenuItem>().asset = storeList[i];
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }


    #region 廣告
    [SerializeField] Text uMoneyText = null;
    void StartAD()
    {
        uMoneyText.text = GameManager.instance.u_money.ToString();
        GameManager.instance.Evt_UpdateGameValue += onMoneyChange;

        //初始化廣告SDK
#if UNITY_IOS
        Advertisement.Initialize("3598684", false);
#endif

#if UNITY_ANDROID
        Advertisement.Initialize("3598685", false);
#endif
        //插頁式廣告
        //Advertisement.Show();
    }
    //這個程式消失時發生 關閉遊戲 換到別關
    void OnDisable()
    {
        GameManager.instance.Evt_UpdateGameValue -= onMoneyChange;
    }


    void onMoneyChange()
    {
        uMoneyText.text = GameManager.instance.u_money.ToString();

    }

    public void ButtonShowAD()
    {
        if(Advertisement.IsReady() == true)
        {
            Advertisement.Show("rewardedVideo");
            //給編輯器測試
#if UNITY_EDITOR
            GameManager.instance.u_money += 500;
#endif

        }
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.C))
        {
            Debug.LogWarning("清除所有紀錄檔");
            PlayerPrefs.DeleteAll();
        }
    }

    // 接收廣告結果
    public void OnUnityAdsDidFinish(string id , ShowResult result)
    {
        if(result == ShowResult.Finished)
        {
            Debug.Log("看完了");
            GameManager.instance.u_money += 500;
        }
        if(result == ShowResult.Skipped)
        {
            Debug.Log("跳過了");
        }
        if(result == ShowResult.Failed)
        {
            Debug.Log("失敗");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        //廣告載好 主動通知
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidError(string message)
    {
        throw new System.NotImplementedException();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        throw new System.NotImplementedException();
    }
    #endregion



}

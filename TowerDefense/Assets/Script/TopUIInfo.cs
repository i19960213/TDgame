using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TopUIInfo : MonoBehaviour
{
    /// <summary>當我存在</summary>
    private void OnEnable()
    {
        //訂閱
        GameManager.instance.Evt_UpdateGameValue += UpdateUI;
    }
    /// <summary>當我不存在</summary>
    private void OnDisable()
    {
        //切換關卡 關閉遊戲 或被刪
        GameManager.instance.Evt_UpdateGameValue -= UpdateUI;
    }

    [SerializeField]Text moneyText = null;
    /// <summary>刷新介面 </summary>
    void UpdateUI()
    {
        // 數值發生變化
        moneyText.text = "Money : " + GameManager.instance.money.ToString("###,###");
    }

}

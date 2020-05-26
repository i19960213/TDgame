using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateEnemySystem : MonoBehaviour
{
    public static CreateEnemySystem instance = null;
    void Awake()
    {
        instance = this;
    }


    public bool isCreate = false;
    public void SetWave(EnemyLevelData data)
    {
        isCreate = true;
        //有新資料
        mainData = data;
        //計數器歸零
        number = 0;
    }
    /// <summary>生怪器資料包</summary>
    EnemyLevelData mainData;
    /// <summary>生到第幾隻</summary>
    int number = 0;
    float timer = 0f;
    /// <summary>怪整體的生命數量</summary>
    public int life = 0;
    [SerializeField] GameObject enemyObj;
    [SerializeField] Transform startPos , endPos;
    void Update()
    {
        // 等待....MVCGame
        //持續生怪
        //一開始無資料時return掉
        if (MVCGame.instance.isGameOver == true)
            return;
        if (mainData == null)
            return;
        if(number < mainData.list.Count)
        {
            timer += Time.deltaTime;
            if (timer > mainData.list[number].cd)
            {
                //產生怪物
               GameObject temp =  Instantiate(enemyObj, startPos.position, startPos.rotation);
                //塞數值
                temp.GetComponent<EnemyAI>().Create(mainData.list[number], endPos.position) ;
                //龜0為了生下一隻
                timer = 0f;
                //生怪數字往前加
                number++;
                life++;
            }
        }
        else
        {
            //生完怪了
            isCreate = false;
        }
        
    }
}

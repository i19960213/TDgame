using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{

    [SerializeField] HealthModule health;
    EnemyData myData;
    // Start is called before the first frame update
    public void Create(EnemyData data, Vector3 endPos)
    {
        
        myData = data;
        //指定血量
        health.StartInfo(data.hp);
        //指定跑速
        speed = data.speed;
        // 刷新一次路徑
        UpdatePath();

        //監聽塔的變化 當發生變化就呼叫UpdatePath
        GameManager.instance.Evt_TowerChange += UpdatePath;
    }
    List<Node> myPath = new List<Node>();
    void UpdatePath()
    {
        number = 0;
        Vector3 tempPos = this.transform.position;
        tempPos.y = 0;
        myPath = NodeManager.instance.GetPath(tempPos);
    }

    int number = 0;
   [SerializeField] float speed = 0.5f;
     void Update()
    {
        //求出要看的點
        Vector3 lookPos = myPath[number].pos;
        lookPos.y = this.transform.position.y;
        //看著點
        this.transform.LookAt(lookPos);

        if(MVCGame.instance.isGameOver == false)
        {
            //向前走
            this.transform.Translate(0f, 0f, speed * Time.deltaTime, Space.Self);
        }
        // 如果很靠近目標了 就把眼光潮下一個點切換
        if (Vector3.Distance(this.transform.position,lookPos) < 0.1f)
        {
            if(number < myPath.Count -1 )
            {
                number = number + 1;
            }
            if(number == myPath.Count - 1)
            {
                //到終點了 遊戲結束
                MVCGame.instance.GameOver();
            }
        }
    }

    public void OnDeath()
    {
        GameManager.instance.Evt_TowerChange -= UpdatePath;
        GameManager.instance.money += myData.money;
    }
}

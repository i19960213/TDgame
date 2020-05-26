using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MVCGame : MonoBehaviour
{
    static public MVCGame instance;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        StartCoroutine("MainGame");
        GameManager.instance.money = 5000;
    }

    [SerializeField] Text logBox;
    [SerializeField] GameObject tips1 , tips2;
    [SerializeField] int wave = 0;
    [SerializeField] List<EnemyLevelData> waveList = new List<EnemyLevelData>();
    [SerializeField] CreateEnemySystem createEnemySystem;

    [SerializeField] GameObject deathUI = null;
    public bool isGameOver = false;

    int start_u_money;

    IEnumerator MainGame()
    {
        // 偷記下一開始有多少錢
        start_u_money = GameManager.instance.u_money;
        logBox.text = "歡迎加入";
        yield return new WaitForSeconds(2f);
        logBox.text = "這是你的金錢";
        tips1.SetActive(true);
        yield return new WaitForSeconds(2f);
        logBox.text = "這是商店";
        tips1.SetActive(false);
        tips2.SetActive(true);
        yield return new WaitForSeconds(2f);
        logBox.text = "現在買一座塔吧";
        tips2.SetActive(false);
       
        // 當波數小於波量時
        while (wave < waveList.Count)
        {
            for(int i= 10; i > 0 ; i--)
            {
                logBox.text = "倒數計時" + i.ToString() + "秒";
                yield return new WaitForSeconds(1f);
            }
            logBox.text = "";

            createEnemySystem.SetWave(waveList[wave]);
            //如果怪孩沒生完或還沒死光  就不停卡半秒
            while (createEnemySystem.isCreate || createEnemySystem.life >0)
            {
                yield return new WaitForSeconds(1f);
            }
            //給錢
            logBox.text ="您得到了" + waveList[wave].unlockMoney.ToString() + "獎牌用於解鎖更高級的塔";
            GameManager.instance.u_money += waveList[wave].unlockMoney;
            yield return new WaitForSeconds(3f);
            wave++;
        }






        logBox.text = "請期待下次更新，感謝全破！";
    }

    [SerializeField] Text winText1, winText2;
    public void GameOver()
    {
        isGameOver = true;
        // 顯示玩家活到的波數
        winText1.text = "你活到了第" + (wave+1).ToString() + "波";
        // 顯示玩家這場拿到的錢
        winText2.text = "本次賺到了" + (start_u_money - GameManager.instance.u_money).ToString() + "獎牌";
        deathUI.SetActive(true);
    }

    public void ButtonAD()
    {
        //下廣告
        //測試用復活
        // 找出所有有Enemy的TAG的敵人
        GameObject[] allEnemy = GameObject.FindGameObjectsWithTag("Enemy");
        //砍了所有o
        foreach(GameObject o in allEnemy)
        {
            Destroy(o);
        }
        isGameOver = false;
        deathUI.SetActive(false);
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene(0);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletCtrl : MonoBehaviour
{
     TowerData towerData ;
     Transform target;
     Vector3 targetPos;

    public void Create(TowerData towerData , Transform target)
    {
        this.towerData = towerData;
        this.target = target;
    }
    void LateUpdate()  //render frame
    {
        // 如果目標存在才更新目標的座標
        if (target != null)
            targetPos = target.position;
        //讓子彈盯著目標
        this.transform.LookAt(targetPos);
        //確認移動距離
        float moveDistance = towerData.bulletSpeed * Time.deltaTime;

        if (Vector3.Distance(this.transform.position , targetPos) < moveDistance )
        {
            this.transform.position = targetPos;
            Explosion();
        }
        else
        {
            // 讓子彈飛
            this.transform.Translate(0f, 0f, moveDistance, Space.Self);
        }
    }
    [SerializeField] GameObject explosionObj;
    [SerializeField] float explosionLifeTime = 2f;
    [SerializeField] LayerMask enemyLayerMask;
    void Explosion()
    {
        //濺射範圍
        Collider[] allEnemy = Physics.OverlapSphere(targetPos, (towerData.bulletAttackRange <0.2f? 0.2f : towerData.bulletAttackRange), enemyLayerMask);

        for (int i = 0; i< allEnemy.Length; i++)
        {
            //試著抓敵人的生命模塊
            HealthModule enemyHealth =  allEnemy[i].GetComponent<HealthModule>();
            if(enemyHealth != null)
            {
                //傳遞傷害
                enemyHealth.TakeDamage(towerData.attack);
                //製造特效
                Destroy(Instantiate(explosionObj, allEnemy[i].transform.position, this.transform.rotation), explosionLifeTime);
            }
        }

        Destroy(this.gameObject);
    }      

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserGun : TowerBase
{
    [SerializeField] LineRenderer lineRenderer;
    protected override void OnAttack(Transform target)
    {
        percent = 1f;
        //檢查是否有敵人
        if(target != null)
        {
            //檢查敵人可否被打
          HealthModule targetHealth = target.GetComponent<HealthModule>();
            if(targetHealth != null)
            {
                //傳傷害
                targetHealth.TakeDamage(mainAsset.list[level].attack);
            }
            // 紀錄目標
            this.target = target;
        }
    }

    float percent = 0f; //0無雷射 1滿載
    [SerializeField] [Header("最大粗度")] Vector2 maxLaserSize;
    Transform target;
    Vector3 targetPos = Vector3.zero;
    
    void LateUpdate()
    {
        if (percent == 0f) 
        {
            return;
        }

        percent -= Time.deltaTime * (this.level+1);
        if (percent < 0f)
            percent = 0;

        //如果有敵人
        if (target != null)
        {
            // 讓目標坐標 = 最近的敵人的位置 (最遠敵人為)enemyList.Count-1
            targetPos = target.position;
        }
        // 雷射寬度 = 最大*%
        lineRenderer.startWidth = Mathf.Lerp(maxLaserSize.x , maxLaserSize.y , level/3f ) * percent;
        //指定雷射起始位置為頭
        lineRenderer.SetPosition(0, turret.position);
        //指定雷射終點為敵人座標
        lineRenderer.SetPosition(1, targetPos);

        

    }
}


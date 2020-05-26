using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : TowerBase
{
    [SerializeField] GameObject attackFire;
    [SerializeField] Vector3 attackFireOffset;
    [SerializeField] GameObject bullet;

    protected override void OnAttack(Transform target)
    {
        //攻擊時發生的事情
        //複製攻擊火焰 存起來
        //<<之後改成物件池>>
        GameObject tempAttackFire = Instantiate(attackFire);
        Destroy(tempAttackFire, 0.7f);
        tempAttackFire.transform.SetParent(turret);
        tempAttackFire.transform.localScale = Vector3.one;
        tempAttackFire.transform.localRotation = Quaternion .Euler(0f , 90f , 0f);
        tempAttackFire.transform.localPosition = attackFireOffset;

        //射子彈
       GameObject temp =  Instantiate(bullet , turret.position , turret.rotation);
       BulletCtrl tempScript =  temp.GetComponent<BulletCtrl>();
        tempScript.Create(mainAsset.list[level], target.transform);
    }
}

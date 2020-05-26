using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthModule : MonoBehaviour
{
    public void StartInfo( float hp)
    {
        maxhp = hp;
    }
    //紀錄身上的血
    float hp
    {
        get { return _hp; }
        set { _hp = value; UpdateHpbar(); }
    }
    float _hp;
    [SerializeField] Image hpBar = null;
    //刷新血條
    void UpdateHpbar()
    {
        if (hpBar == null)
            return;
        hpBar.fillAmount = hp / maxhp;
    }
    [SerializeField] float maxhp;
     void Start()
    {
        hp = maxhp;
    }
    //接收傷害
    public void TakeDamage(float damage)
    {
        hp = hp - damage;
        if (hp <= 0f)
        {
            Death();
        }
    }
    void Death()
    {
        // 傳訊息給OnDeath
        this.gameObject.SendMessage("OnDeath");
        // 跟生怪機說生命減少了
        CreateEnemySystem.instance.life--;
        Destroy(this.gameObject);
    }
    //UI
}

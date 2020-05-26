using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 綁定組件不給刪
[RequireComponent (typeof (SphereCollider)) ]
[RequireComponent (typeof(Rigidbody)) ]
public abstract class TowerBase : MonoBehaviour
{
    /// <summary>主資料</summary>
    public TowerAsset mainAsset;
    /// <summary>等級</summary>
    public int level = 0;
    /// <summary>攻擊力</summary>
    protected float attack;
    /// <summary>攻速</summary>
    protected float attackSpeed;
    /// <summary>可攻擊範圍</summary>
    protected float attackRange
    {
        get { return _attackRange; }
        set { _attackRange = value; radar.radius = value;  }
    }
    float _attackRange = 0f;
    /// <summary>子彈的攻擊範圍</summary>
    protected float bulletAttackRange;
    /// <summary>砲塔轉速</summary>
    protected float rotateSpeed = 180f;


   [SerializeField] [HideInInspector] GameObject skinObj;
   [SerializeField] [HideInInspector] SphereCollider radar;

    protected virtual void Reset()
    {
        // 先把原本的東西清空
        for (int i = this.transform.childCount -1 ; i >= 0 ; i--)
            DestroyImmediate(this.transform.GetChild(i).gameObject);

        // 製作skin子物件
        skinObj = new GameObject("Skin");
        // 設定母物件為我
        skinObj.transform.SetParent(this.transform);
        // 歸零座標
        skinObj.transform.localPosition = Vector3.zero;
        //設定Scale 為 0.1 0.1 0.1
        skinObj.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        //攻擊範圍檢定器
        radar = this.gameObject.GetComponent<SphereCollider>();
        radar.isTrigger = true;
    }

    protected virtual void Start()
    {

    }

    #region 升級塔
    public virtual void UpdateTower(int level)
    {
        // 移除舊的塔
        if(skinObj.transform.childCount >= 1)
           DestroyImmediate( skinObj.transform.GetChild(0).gameObject);
        // 實例化出相應等級的模型
        GameObject tempSkin =  Instantiate(mainAsset.list[level].skin);
        //把塔的模型放進skin裡面
        tempSkin.transform.SetParent(skinObj.transform);
        //把塔的座標與縮放歸位
        tempSkin.transform.localPosition = Vector3.zero;
        tempSkin.transform.localScale = Vector3.one;

        //改數值-----------
        this.level = level;
        this.attackRange = mainAsset.list[level].attackRange;
        this.rotateSpeed = mainAsset.list[level].rotateSpeed;
        this.attackSpeed = mainAsset.list[level].attackSpeed;
        this.attack = mainAsset.list[level].attack;
        this.bulletAttackRange = mainAsset.list[level].bulletAttackRange;
    }
    #endregion

   protected Transform turret
    {
        get
        {
            if(_turret == null)
            {
                Transform[] allChildren =  this.transform.GetComponentsInChildren<Transform>();
                foreach(Transform t in allChildren)
                {
                    if (t.name == "Turret")
                        _turret = t;
                }
            }
            return _turret;
        }
        set
        {
            _turret = value;
        }
    }
    Transform _turret;
    //讓子物件可以存取
   [SerializeField] Transform currentTarget = null;

    float nextAttackableTime = 0f;
    //計算用旋轉量
    Quaternion originalQuatrenion = new Quaternion();
    protected virtual void Update()
    {
        //當目前的對象死亡 or 大於最大攻擊範圍時
        if(currentTarget == null || Vector3.Distance(this.transform.position , currentTarget.position) > attackRange)
        {
            //清理死去的敵人
            CleanList();
            //排序敵人 依近到遠
            EnemyListSort();
            if(enemyList.Count > 0) 
            {
                //指定一個最靠近的敵人為新目標
                currentTarget = enemyList[0].transform;
            }
        }

        if (currentTarget == null)
        {
            return;
        }

        //敵人在攻擊範圍

        //-----------------------------------------------旋轉(轉速)------------------------------------------------------
        //A到B的向量 = B-A 並轉換成旋轉量 
        Quaternion lookTargetQuaternion = Quaternion.LookRotation(currentTarget.position - turret.position);
        // 自己的旋轉量 = 自己 與 A到B的向量轉換的旋轉量 + 時間
        originalQuatrenion = Quaternion.Slerp(originalQuatrenion, lookTargetQuaternion, Time.deltaTime * rotateSpeed);

        //把算好的數字給頭使用
        turret.rotation = originalQuatrenion;
        //處理頭的差值
        turret.Rotate(0f, -90f, 0f, Space.Self);
       
        //---------------------------------------------------攻速--------------------------------------------------------
        if(Time.time > nextAttackableTime)
        {
            OnAttack(currentTarget);
            //讓1秒除攻速 => 攻速2為一秒兩下
            nextAttackableTime = Time.time + (1f / attackSpeed);
        }
    }

    protected abstract void OnAttack(Transform target);


    /// <summary>敵人列表</summary>
    [SerializeField] protected List<GameObject> enemyList = new List<GameObject>();

    //聯集 |
    //交集 &
    [SerializeField] LayerMask enemyLayerMask;

    protected virtual void OnTriggerEnter(Collider other)
    {
      /*  
        if ((other.gameObject.layer & enemyLayerMask) != 0)
        {
            //兩個東西交集了
            // 加入敵人列表
            enemyList.Add(other.gameObject);
        }
        */

        //求出對方的
        LayerMask otherLayer = 1 << other.gameObject.layer;
        if((otherLayer & enemyLayerMask) == otherLayer)
        {
            //兩個東西交集了
            // 加入敵人列表
            enemyList.Add(other.gameObject);
        }

    }
    protected virtual void OnTriggerExit(Collider other)
    {
        LayerMask otherLayer = 1 << other.gameObject.layer;
        if ((otherLayer & enemyLayerMask) == otherLayer)
        {
            enemyList.Remove(other.gameObject);
        }
    }

    /// <summary>將敵人從近到遠進行排序 </summary>
    void EnemyListSort()
    {
        //呼叫Sort進行排序的動作
        enemyList.Sort((x, y) =>
        {
            //傳入排序公式 => 我跟X的距離 比較 我跟Y的距離
            return Vector3.Distance(this.transform.position, x.transform.position).CompareTo(Vector3.Distance(this.transform.position, y.transform.position));
         } ) ;
    }

    void CleanList()
    {
        //倒過來跑的迴圈 檢查enemyList 看誰死了
        for(int i = enemyList.Count -1 ; i >= 0; i--)
        {
            if(enemyList [i] == null)
            {
                enemyList.RemoveAt(i);
            }
        }
    }

    public virtual void KillMe()
    {
        // 馬上刪 destory效能好時才刪 第一時間只有關閉顯示
        DestroyImmediate(this.gameObject);
    }
}

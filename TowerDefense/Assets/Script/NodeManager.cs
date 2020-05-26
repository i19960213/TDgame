using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    // Singleton
    public static NodeManager instance = null;
     void Awake()
    {
        instance = this;
        CreateNode();
    }

    [Header("寬度(行數)")] public int xMax;
    [Header("深度(列數)")] public int zMax;

    /// <summary>主要的節點位置</summary>
    public Node[,] nodes;

    [SerializeField] LayerMask landMask = 0;
    [SerializeField] string canMoveTag = "0";

    void CreateNode()
    {
        // 初始化記憶體大小
        nodes = new Node[xMax, zMax];
        for(int x = 0; x < xMax; x++)
        {
            for(int z =0; z < zMax; z++)
            {
                //遍歷二維陣列裡所有東西
                Node newNode = new Node(GetNodePos(x,z), x, z);
                // 放進二維度
                nodes[x, z] = newNode;
                nodes[x, z].isObstacle = true;
                // 檢查周圍有沒有可以踩的地方 以自己的點的位置創一個圓 layerMask
                Collider[] objs = Physics.OverlapSphere(nodes[x, z].pos, 0.1f, landMask);
                // 陣列中跑迴圈 符合的人o 的tag為canMoveTag 符合的可走動
                foreach (Collider o in objs)
                {
                    if(o.tag == canMoveTag)
                    {
                        nodes[x, z].isObstacle = false;
                        break;
                    }
                }
            }
        }
    }

    public Vector3 GetNodePos(int x , int z)
    {
        Vector3 outPos = Vector3.zero;
        outPos.x = ((float)xMax * -0.5f) + x + 0.5f;
        outPos.z = ((float)zMax * -0.5f) + z + 0.5f;
        outPos += this.transform.position;
        return outPos;
    }

    /// <summary>用座標取得節點</summary>
    public Node GetNodeByPos(Vector3 pos)
    {
        for (int x = 0; x < xMax; x++)
        {
            for (int z = 0; z < zMax; z++)
            {
                if(Vector3.Distance(nodes[x,z].pos , pos) < 0.5f)
                {
                    return nodes[x, z];
                }
            }
        }
        return null;
    }
    Vector3 buyPosMask = new Vector3(0f, 100f, 0f);
    /// <summary>檢查這個行列數究竟可不可以生成節點</summary>
    public bool HaveStuff(int x , int z)
    {
        //排除不可能有東西的情況
        if(x<0 || x > (xMax - 1))
        {
            return false;
        }
        if (z < 0 || z > (zMax - 1))
        {
            return false;
        }
        //有東西但不能踩
        if(nodes[x, z].isObstacle)
        {
            return false;
        }
        // 如果這個位置即將要買東西 同樣也不列入計算
        if(nodes[x,z].pos == buyPosMask)
        {
            return false;
        }
        return true;
    }

    /// <summary>取得節點附近的鄰居</summary>
    public List<Node> GetNeighbors(Node node)
    {
        List<Node> outList = new List<Node>();
        //檢查左邊 左邊有人
        if(HaveStuff(node.x-1 , node.z) == true)
        {
            outList.Add(nodes[node.x - 1, node.z]);
        }
        //檢查右邊 右邊有人
        if (HaveStuff(node.x + 1, node.z) == true)
        {
            outList.Add(nodes[node.x + 1, node.z]);
        }
        //檢查上邊 上邊有人
        if (HaveStuff(node.x , node.z+1) == true)
        {
            outList.Add(nodes[node.x , node.z+1]);
        }
        //檢查下邊 下邊有人
        if (HaveStuff(node.x , node.z-1) == true)
        {
            outList.Add(nodes[node.x , node.z-1]);
        }
        return outList;
    }


    // ---------------------------------------------A*----------------------------------------------
    #region("A*本體")

    NodeSort openList; // 尚未計算
    NodeSort closeList; // 算完的
    /// <summary>取得移動到此地的預估成本</summary>
    float GetCost(Node start , Node end)
    { 
        Vector3 temp = end.pos - start.pos;
        return temp.magnitude;
    }

    /// <summary>算出兩個節點之間的路徑</summary>
    public List<Node> FindPath(Node start , Node end)
    {
        //初始化
        openList = new NodeSort();
        closeList = new NodeSort();
        //指定起點數值
        start.g = 0f;
        start.c = GetCost(start, end); //粗估出到終點的成本
        // 把起點放進未計算列表
        openList.Add(start);
        //計算用節點
        Node node = null;
        // 還有未計算的東西 就不停的進迴圈
        while ( openList.Count != 0)
        {
            //推動node

            // 把計算用的節點 指到未計算的且最近的節點
            node = openList.GetFirst();
            // 把這個節點放進已計算
            closeList.Add(node);
            //移除未計算列表中節點
            openList.Remove(node);
            //遇到當前的點就是終點時
            if(node.pos == end.pos)
            {
                //直接找這個點的族譜
                return GetAllParent(node);
            }

            //計算可能的鄰居
            List<Node> neighbors = new List<Node>();
            //找出當前節點的所有鄰居
            neighbors = GetNeighbors(node);
            //一個個檢查
            for(int i =0; i < neighbors.Count; i++)
            {
                Node neighbor = neighbors[i];
                // 如果這鄰居不在已經計算過的列表中
                if (closeList.IsCountains(neighbor) == false)
                {
                    float cost_g, total;

                    // 不在計算過的列表中
                    if(openList.IsCountains(neighbor) == false)
                    {
                        //普通計算
                        cost_g = GetCost(node, neighbor);
                        total = cost_g + node.g;

                        neighbor.g = total;
                        neighbor.parent = node;
                        neighbor.c = GetCost(neighbor, end);

                        openList.Add(neighbor);
                    }
                    else // 已經在列表中
                    {
                        cost_g = GetCost(node, neighbor);
                        total = cost_g + node.g;
                        //可能的捷徑
                        if(neighbor.g > total)
                        {
                            neighbor.g = total;
                            neighbor.parent = node;
                        }
                    }
                }
            }

        }
        if(node.pos != end.pos)
        {
            Debug.Log("沒路可走");
            return null;
        }
        return GetAllParent(node);
    }

    /// <summary>找出所有祖先</summary>
    public List<Node> GetAllParent(Node node)
    {
        List<Node> temp = new List<Node>();
        while (node != null)
        {
            temp.Add(node);
            node = node.parent;
        }
        temp.Reverse();
        return temp;
    }
    #endregion
    // ---------------------------------------------A*----------------------------------------------


    [SerializeField] Transform startPos, endPos;
    public List<Node> pathList = new List<Node>();

    /// <summary>AI找路</summary>
    public List<Node> GetPath( Vector3 startPos )
    {
        //把商店用的阻擋器移到空中 避免妨礙計算
        buyPosMask = new Vector3(0f, 999f, 0f);
        CreateNode();
        Node startnode = GetNodeByPos(startPos);
        Node endnode = GetNodeByPos(endPos.position);
        //求出路徑
        return FindPath(startnode, endnode);
    }

    /// <summary>給商店檢查是否可以走</summary>
    public bool IsPathOk(Vector3 buyPos)
    {
        buyPosMask = buyPos;
        CreateNode();
        Node startNode = GetNodeByPos(startPos.position);
        Node endNode = GetNodeByPos(endPos.position);
        //求出路徑
        pathList = FindPath(startNode, endNode);
        // 如果求出的路徑沒辦法走就傳false
        if( pathList == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void OnDrawGizmos()
    {
        for (int x = 0; x < xMax; x++)
        {
            for (int z = 0; z < zMax; z++)
            {
                Gizmos.color = Color.green;
                if (nodes != null)
                    Gizmos.DrawWireCube(nodes[x, z].pos, new Vector3(1f, 0f, 1f));
            }
        }
        if (pathList != null)
            for (int i = 0; i < pathList.Count; i++)
            {
               Gizmos.DrawCube(pathList[i].pos + Vector3.up, new Vector3(0.5f, 0f, 0.5f));


            }
        for (int j = 0; j < pathList.Count - 1; j++)
        {
            Gizmos.DrawLine(pathList[j].pos + Vector3.up, pathList[j + 1].pos + Vector3.up);
         
            
        }

    }
    
}

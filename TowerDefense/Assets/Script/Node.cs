using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//排序

public class Node : IComparable
{
    /// <summary>走到這個位置的成本</summary>
    public float g;
    /// <summary>到終點的成本(評估值) 距離</summary>
    public float c;
    /// <summary>行</summary>
    public int x;
    /// <summary>列</summary>
    public int z;
    /// <summary>節點的座標</summary>
    public Vector3 pos;
    /// <summary>是否為障礙物(可能簍空)</summary>
    public bool isObstacle;
    /// <summary>父母物件/summary>
    public Node parent;

    public int CompareTo(object obj) //obj = 排序時的下一個對象
    {
        Node nextNode = (Node)obj;
        if ((nextNode.g + nextNode.c) > (g+c)) //對方下一個節點的成本比較大時
        {
            return -1; //排前
        }
        else
        {
            return 1;  //排後
        }
    }

    public Node()
    {
        g = 0;
        c = 0;
        x = 0;
        z = 0;
        isObstacle = true;
        parent = null;
        pos = Vector3.zero;
    }

    public Node(Vector3 pos, int x , int z)
    {
        g = 0;
        c = 0;
        this.x = x;
        this.z = z;
        isObstacle = true;
        parent = null;
        this.pos = pos;
    }
}

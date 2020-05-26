using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeSort 
{
    List<Node> nodes = new List<Node>();

    public int Count
    {
        get { return nodes.Count; }
    }

    /// <summary>如果列表中包含這個節點</summary>
    public bool IsCountains(Node node)
    {
        for (int i =0; i < nodes.Count; i++)
        {
            if (node.pos == nodes[i].pos)
                return true;
        }
        return false;
    }
    /// <summary>取得成本最低的結點</summary>
    public Node GetFirst()
    {
        if(nodes.Count > 0)
        {
            return nodes[0];
        }
        return null;
    }
    /// <summary>增加元素 排序</summary>
    public void Add(Node node)
    {
        nodes.Add(node);
        nodes.Sort();
    }
    /// <summary>減少元素 排序</summary>
    public void Remove(Node node)
    {
        nodes.Remove(node);
        nodes.Sort();
    }
}

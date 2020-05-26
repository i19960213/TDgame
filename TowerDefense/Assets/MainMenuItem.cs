using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuItem : MonoBehaviour
{
    public TowerAsset asset;
    public void ButtonOn()
    {
        //呼叫確認視窗
        AskWindows.instance.Open(asset);
    }
}

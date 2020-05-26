using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUICtrl : MonoBehaviour
{
    Transform myCamera;

    void Start()
    {
        myCamera = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void Update()
    {
        this.transform.LookAt(myCamera.position);
        this.transform.Rotate(0f, 180f, 0f, Space.Self);
    }
}

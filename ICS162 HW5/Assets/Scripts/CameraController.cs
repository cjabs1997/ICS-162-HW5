using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        this.transform.position = GameObject.Find("MrMan").transform.position + offset;
    }
}

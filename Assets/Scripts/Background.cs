using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    private Camera _mainCam;

    void Start()
    {
        _mainCam = Camera.main;
    }

    void Update()
    {
        float angle = -_mainCam.transform.rotation.eulerAngles.y;

        //transform.rotation = Quaternion.Euler(0f, angle, 0f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _rightCam;
    [SerializeField] CinemachineVirtualCamera _leftCam;

    bool _isRightCam;

    void OnEnable()
    {
        PlayerMoveState.DirectionChangeEvent += SwitchCameras;
    }

    void OnDisable()
    {
        PlayerMoveState.DirectionChangeEvent -= SwitchCameras;
    }

    void SwitchCameras(bool isFacingRight)
    {
        if (_isRightCam != isFacingRight)
        {
            _isRightCam = isFacingRight;

            if (_isRightCam)
            {
                _leftCam.Priority = 0;
                _rightCam.Priority = 1;
            }
            else 
            {
                _leftCam.Priority = 1;
                _rightCam.Priority = 0;
            }
        }
    }
}

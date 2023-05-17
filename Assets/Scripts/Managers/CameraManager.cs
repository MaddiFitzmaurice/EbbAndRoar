using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera _rightCam;
    [SerializeField] CinemachineVirtualCamera _leftCam;
    [SerializeField] CinemachineVirtualCamera _leftJumpCam;
    [SerializeField] CinemachineVirtualCamera _rightJumpCam;

    bool _isRightCam;
    bool _isJumpCam;

    void OnEnable()
    {
        PlayerMoveState.DirectionChangeEvent += SwitchDirCameras;
        LionJumpState.JumpEvent += SwitchJumpCam;
    }

    void OnDisable()
    {
        PlayerMoveState.DirectionChangeEvent -= SwitchDirCameras;
        LionJumpState.JumpEvent -= SwitchJumpCam;
    }

    void SwitchDirCameras(bool isFacingRight)
    {
        if (_isRightCam != isFacingRight)
        {
            _isRightCam = isFacingRight;

            DecideCam();
        }
    }

    void SwitchJumpCam(bool isJumping)
    {
        if (_isJumpCam != isJumping)
        {
            _isJumpCam = isJumping;
            DecideCam();
        }
    }

    void DecideCam()
    {
        // If is facing right and is not jumping
        if (_isRightCam && !_isJumpCam)
        {
            _leftCam.Priority = 0;
            _leftJumpCam.Priority = 0;

            _rightCam.Priority = 1;
            _rightJumpCam.Priority = 0;
        }
        // Else if is facing right and is jumping
        else if (_isRightCam && _isJumpCam)
        {
            _leftCam.Priority = 0;
            _leftJumpCam.Priority = 0;

            _rightCam.Priority = 0;
            _rightJumpCam.Priority = 1;
        }
        // Else if is facing left and is not jumping
        else if (!_isRightCam && !_isJumpCam)
        {
            _leftCam.Priority = 1;
            _leftJumpCam.Priority = 0;

            _rightCam.Priority = 0;
            _rightJumpCam.Priority = 0;
        }
        // Else if is facing elft and is jumping
        else 
        {
            _leftCam.Priority = 0;
            _leftJumpCam.Priority = 1;

            _rightCam.Priority = 0;
            _rightJumpCam.Priority = 0;
        }
    }
}

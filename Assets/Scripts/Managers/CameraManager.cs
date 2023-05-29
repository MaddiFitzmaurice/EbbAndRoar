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
    [SerializeField] CinemachineVirtualCamera _narrativeCam;

    bool _isRightCam;
    bool _isJumpCam;
    bool _isNarrativeCam;

    CinemachineTargetGroup _targetGroup;

    void OnEnable()
    {
        PlayerState.DirectionChangeEvent += SwitchDirCameras;
        LionMoveJumpState.JumpEvent += SwitchJumpCam;
        NPC.SendNarrativeDataEvent += ChangeTargetGroup;
        HumanNarrativeState.StartNarrativeEvent += SwitchNarCameras;
    }

    void OnDisable()
    {
        PlayerState.DirectionChangeEvent -= SwitchDirCameras;
        LionMoveJumpState.JumpEvent -= SwitchJumpCam;
        NPC.SendNarrativeDataEvent -= ChangeTargetGroup;
        HumanNarrativeState.StartNarrativeEvent -= SwitchNarCameras;
    }

    private void Start()
    {
        _targetGroup = GetComponentInChildren<CinemachineTargetGroup>();
    }

    void ChangeTargetGroup(NPCEventData npcEventData)
    {
        _targetGroup.m_Targets[1].target = npcEventData.Transform;
    }

    void SwitchDirCameras(bool isFacingRight)
    {
        if (_isRightCam != isFacingRight)
        {
            _isRightCam = isFacingRight;

            DecideCam();
        }
    }

    void SwitchNarCameras(bool isNarrative)
    {
        if (_isNarrativeCam != isNarrative)
        {
            _isNarrativeCam = isNarrative;

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
        if (_isNarrativeCam)
        {
            _narrativeCam.Priority = 1;

            _leftCam.Priority = 0;
            _leftJumpCam.Priority = 0;

            _rightCam.Priority = 0;
            _rightJumpCam.Priority = 0;
        }
        else 
        {
            _narrativeCam.Priority = 0;

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
            // Else if is facing left and is jumping
            else 
            {
                _leftCam.Priority = 0;
                _leftJumpCam.Priority = 1;

                _rightCam.Priority = 0;
                _rightJumpCam.Priority = 0;
            }
        }
    }
}

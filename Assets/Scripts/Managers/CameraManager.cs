using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

public class CameraManager : MonoBehaviour
{
    #region Singleton Things
    private static CameraManager _instance;
    public static CameraManager Instance
    {
        get
        {
            return _instance;
        }
    }


    #endregion

    void Awake()
    {
        if (_instance != null || _instance != this)
        {
            Destroy(_instance);
        }
        _instance = this;
    }

    [SerializeField] private CinemachineVirtualCamera _playerVCam;
    private int _playerVCamDefaultPriority = 12;
    [SerializeField] private CinemachineVirtualCamera _paintVCam;
    private int _paintVCamDefaultPriority = 10;

    [SerializeField] private Transform _playerTrans;
    [SerializeField] private Transform _paintingWarpPointTrans;
    [SerializeField] private Transform _paintAreaTrans;

    private int _priorityDiffValue = 2;
    
    void Start()
    {
        //If player pressed "Reset", button switch to player camera
        RaceManager.Instance.ResetRaceEvent.AddListener(SwitchToPlayerCamera);
        //If player finished race, switch paint camera and look painting platform warp point
        RaceManager.Instance.PlayerFinishedRaceEvent.AddListener(SwitchToPaintCamera);
        RaceManager.Instance.PlayerFinishedRaceEvent.AddListener(LookPaintingWarpPoint);
    }
    
    private void SwitchToPaintCamera()
    {
        _playerVCam.Priority -= _priorityDiffValue;
        _paintVCam.Priority += _priorityDiffValue;
    }

    private void SwitchToPlayerCamera()
    {
        _paintVCam.Priority = _paintVCamDefaultPriority;
        _playerVCam.Priority = _playerVCamDefaultPriority;
    }


    public void LookPaintingWarpPoint()
    {
        _paintVCam.LookAt = _paintingWarpPointTrans;
        _paintVCam.Follow = _paintingWarpPointTrans;
    }

    //public void LookPaintAreaPoint()
    //{
    //    _paintVCam.LookAt = _paintAreaTrans;
    //    _paintVCam.Follow = _paintAreaTrans;
    //}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Racer))]
[RequireComponent(typeof(OpponentNavigation))]
public class Opponent : MonoBehaviour
{
    [SerializeField] private OpponentNavigation _navigation;
    [SerializeField] private Racer _raceId;
    private Coroutine _restartRunCoroutine;
    private float _maxZIndexRandomize = 15f;
    void Start()
    {
        RaceManager.Instance.StartRaceEvent.AddListener(StartRun);
        _raceId.ObstacleCrash.AddListener(CrashReact);
        RaceManager.Instance.ResetRaceEvent.AddListener(ResetRun);
    }


    private void Update()
    {
        if (RaceManager.Instance.IsRaceStarted) 
            _raceId.UpdateCharacterAnimation();
        
    }

    //AI will start running.
    public void StartRun()
    {
        Debug.Log("Run Forest Run! ");
        Vector3 targetPosition = _raceId.RaceInitPosition + ((_navigation.DestinationZIndex + UnityEngine.Random.Range(3,_maxZIndexRandomize) ) * Vector3.forward); 
        _navigation.GoToTargetPosition(targetPosition);

    }
    //When AI hit obstacle or fall, AI will start running again.
    public void RestartRun()
    {
        //_restartRunCoroutine = StartCoroutine(RestartRunRoutine());

        Debug.Log("Restart Run");
        StartRun();

    }
    
    //When User click "Reset" button. AI will teleport init position and reset AI
    public void ResetRun()
    {
        _navigation.ResetAI();
        _navigation.Agent.Warp(_raceId.RaceInitPosition);
        transform.rotation = Quaternion.identity;
        _raceId.IsRacerFall = false;
        _raceId.RaceStatus.IsFinished = false;
        _raceId.RaceStatus.IsChampion = false;
        _raceId.Animator.SetFloat(AnimatorParameter.Speed, 0f);
        _raceId.Animator.SetBool(AnimatorParameter.IsMove, false);
        _raceId.Animator.SetBool(AnimatorParameter.IsCrashed, false);
        
        
    }


    #region CrashReact


    public void CrashReact()
    {
        _navigation.ResetAI();
        _raceId.Animator.SetFloat(AnimatorParameter.Speed, 0f);
        _raceId.Animator.SetBool(AnimatorParameter.IsMove, false);
        _raceId.Animator.SetBool(AnimatorParameter.IsCrashed, true);
        _raceId.IsRacerFall = true;
        StartCoroutine(HitReactRoutine());
    }

    IEnumerator HitReactRoutine()
    {
        _raceId.Animator.Play("Base Layer.CrashReact", 0);
        yield return new WaitForSeconds(_raceId.CrashReactAnim.length);
        if (_raceId.IsRacerFall == true)
            RestartRace();
    }
    public void RestartRace()
    {
        _raceId.Animator.SetBool(AnimatorParameter.IsCrashed, false);
        _navigation.Agent.Warp(_raceId.RaceInitPosition);
        transform.rotation = Quaternion.identity;
        _raceId.IsRacerFall = false;
        RestartRun();
    }
    #endregion




}

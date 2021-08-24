using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Racer))]
public class Player : MonoBehaviour
{
    [SerializeField] private Racer _racerId;
    [SerializeField] private Rigidbody _rigidBody;
    private bool _isPlayerControllable = true;
    private float _warpProcessTime = 4.1f;
    
    public void Start()
    {
        _racerId.ObstacleCrash.AddListener(CrashReact);
        RaceManager.Instance.ResetRaceEvent.AddListener(RestartRace);
        RaceManager.Instance.PlayerFinishedRaceEvent.AddListener(PreventPlayerControl);
    }


    #region Input Variables and Functions

    private Vector2 _inputInitPosition;
    private Vector2 _inputEndPosition;
    private Vector2 _inputPositionDelta;
    [SerializeField] private float positionDeltaMaxMagnitude;
    private Vector2 CalculateInputPositionDelta()
    {

        _inputPositionDelta = Vector2.ClampMagnitude((_inputEndPosition - _inputInitPosition), 150);
        return _inputInitPosition - _inputEndPosition;
    }

    private void ResetInput()
    {
        _inputPositionDelta = _inputEndPosition = _inputInitPosition = Vector2.zero;
    }

    #endregion


    #region Movement

    [SerializeField] private float _movementSpeed = 0.05f;
    private Vector3 _nextMovePosition = Vector3.zero;
    private void Move()
    {
        if (_racerId.IsRacerFall)
            return;
        
        _nextMovePosition = new Vector3((_inputPositionDelta.x * _movementSpeed * Time.deltaTime), 0,
            (_inputPositionDelta.y * _movementSpeed * Time.deltaTime));
        //transform.position += _nextMovePosition;
        _rigidBody.MovePosition(transform.position += _nextMovePosition);
        transform.rotation = Quaternion.LookRotation(_nextMovePosition);
        //_rigidBody.MoveRotation(Quaternion.LookRotation(_nextMovePosition));
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        if (RaceManager.Instance.IsRaceStarted && _isPlayerControllable)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _inputInitPosition = Input.mousePosition;
            }
            else if (Input.GetMouseButton(0))
            {
                _inputEndPosition = Input.mousePosition;
                CalculateInputPositionDelta();
                Move();
            }

            if (Input.GetMouseButtonUp(0))
            {
                ResetInput();
            }
        }
        _racerId.UpdateCharacterAnimation();
    }

    

    #region CrashReact

    
    public void CrashReact()
    {
        _racerId.Animator.SetFloat(AnimatorParameter.Speed, 0f);
        _racerId.Animator.SetBool(AnimatorParameter.IsMove, false);
        _racerId.Animator.SetBool(AnimatorParameter.IsCrashed, true);
        _racerId.IsRacerFall = true;
        StartCoroutine(HitReactRoutine());
    }

    IEnumerator HitReactRoutine()
    {
        _racerId.Animator.Play("Base Layer.CrashReact", 0);
        yield return new WaitForSeconds(_racerId.CrashReactAnim.length);
        RestartRace();
    }
    #endregion



    public void RestartRace()
    {
        transform.position = _racerId.RaceInitPosition;
        transform.rotation = Quaternion.identity;
        _racerId.IsRacerFall = false;
        _racerId.RaceStatus.IsFinished = false;
        _racerId.RaceStatus.IsChampion = false;
        _isPlayerControllable = true;
        _racerId.Animator.SetFloat(AnimatorParameter.Speed, 0f);
        _racerId.Animator.SetBool(AnimatorParameter.IsMove, false);
        _racerId.Animator.SetBool(AnimatorParameter.IsCrashed, false);
    }

    //The character's control mechanics are blocked.
    //For example, character control is blocked when the painting process is started.
    public void PreventPlayerControl()
    {
        _isPlayerControllable = false;
    }
    
    public void WarpPaintingPosition(Vector3 warpPosition)
    {
        StartCoroutine(WarpRoutine(warpPosition));
    }

    private IEnumerator WarpRoutine(Vector3 warpPosition)
    {
        //An average of 3 seconds is needed when changing the camera for Painting.
        //Therefore, after waiting for 4 seconds, the character teleports to the relevant position.
        yield return new WaitForSeconds(_warpProcessTime);
        PaintingManager.Instance.ReadyForPaintingEvent.Invoke();
        transform.position = warpPosition;
        transform.rotation = Quaternion.identity;
    }


    
}

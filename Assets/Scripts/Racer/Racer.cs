using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;


public enum RacerType
{
    AI,
    Player
}

public class RaceStatus
{
    public float FinishlineDistance;
    public string RacerName;
    public int CurrentPosition;
    public bool IsFinished = false;
    public bool IsChampion = false;
    public RaceStatus() { }
    public RaceStatus(float finishlineDistance, string racerName, int currentPosition)
    {
        FinishlineDistance = finishlineDistance;
        RacerName = racerName;
        CurrentPosition = currentPosition;
    }

}

[DefaultExecutionOrder(-1000)]
[RequireComponent(typeof(Animator))]
public class Racer : MonoBehaviour
{

    public RacerType RType;
    [HideInInspector] public Vector3 RaceInitPosition;

    //When any racer hit to obstacle, this event will fire.
    [HideInInspector] public UnityEvent ObstacleCrash = new UnityEvent();

    [SerializeField] public Animator Animator;
    [HideInInspector] public bool IsRacerFall = false;
    
    [HideInInspector] public AnimationClip CrashReactAnim;
    //The racer's general race status information is kept.
    public RaceStatus RaceStatus { get; set; } = new RaceStatus();
    [SerializeField] private TextMeshPro _characterNameTMP;

    public Transform FinishlineTrans;
    void Start()
    {
        if (RType == RacerType.AI)
        {
            string characterName = NameData.Instance.GetRandomName();
            RaceStatus = new RaceStatus(float.PositiveInfinity, characterName, 1);
            _characterNameTMP.SetText(characterName);
        }
        else if (RType == RacerType.Player)
        {
            RaceStatus = new RaceStatus(float.PositiveInfinity, "Player", 1);
        }
        //Cached for reliable animation length
        CrashReactAnim = Animator.runtimeAnimatorController.animationClips.ToList().Find(a => a.name == AnimationName.CrashReact);

        RaceInitPosition = transform.position;
    }


    void Update()
    {
        RaceStatus.FinishlineDistance = Vector3.Distance(transform.position, FinishlineTrans.position);
    }


    public void UpdateCharacterAnimation()
    {
        GetCurrentVelocity();
        if (currentVelocity > 0.03f)
        {
            Animator.SetBool(AnimatorParameter.IsMove, true);
            Animator.SetFloat(AnimatorParameter.Speed, currentVelocity);
        }
        else
        {
            Animator.SetBool(AnimatorParameter.IsMove, false);
            Animator.SetFloat(AnimatorParameter.Speed, 0);
        }

    }


    private Vector3 previousPosition;
    private float currentVelocity;
    private void GetCurrentVelocity()
    {
        Vector3 movementDelta = transform.position - previousPosition;
        currentVelocity = movementDelta.magnitude / Time.deltaTime;
        previousPosition = transform.position;
        //Debug.Log(currentVelocity);
    }



}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HorizontalObstacle : Obstacle
{
    [SerializeField] private Animator _obstacleAnimator;
    //If the same obstacle has more than one animation,
    //the developer can make each obstacle use different animation by
    //giving the appropriate "_animationDecision" from the inspector panel.
    [SerializeField] private int _animationDecision;

    private void Start()
    {
        if (_animationDecision != 0)
        {
            SetAnimationDecision();
        }
    }

    //If same obstacle has more than one animation
    protected void SetAnimationDecision()
    {
        this._obstacleAnimator.SetInteger(AnimatorParameter.AnimDecision, _animationDecision);
    }
}

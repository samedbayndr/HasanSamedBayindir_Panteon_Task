using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SocialPlatforms;

[RequireComponent(typeof(NavMeshAgent))]
public class OpponentNavigation : MonoBehaviour
{

    public NavMeshAgent Agent;

    public Transform FinishTrans;

    [HideInInspector] public float DestinationZIndex;
    private Coroutine _agentArrivedCheckCoroutine;
    private bool _isAgentArrivedCheckCoroutineRunning;

    private void Start()
    {
        DestinationZIndex = FinishTrans.position.z - transform.position.z;
        Agent.autoTraverseOffMeshLink = true;
    }
    public void GoToTargetPosition(Vector3 targetPosition)
    {
        Debug.Log("AI going to " + targetPosition);
        ResetAI();
        Agent.SetDestination(targetPosition);

        if (_isAgentArrivedCheckCoroutineRunning)
            StopCoroutine(_agentArrivedCheckCoroutine);

        _agentArrivedCheckCoroutine = StartCoroutine(AgentArrivedCheckRoutine());

    }

    public void ResetAI()
    {
        Debug.Log("AI Resetted");
        Agent.isStopped = true;
        Agent.ResetPath();
        
    }

    public IEnumerator AgentArrivedCheckRoutine()
    {
        _isAgentArrivedCheckCoroutineRunning = true;
        bool isOnTheWay = false;
        while (!isOnTheWay)
        {
            if (Agent.hasPath)
            {
                if (!Agent.pathPending)
                {
                    if (Agent.remainingDistance <= Agent.stoppingDistance)
                    {
                        Debug.Log("Agent arrived");
                        isOnTheWay = true;
                        ResetAI();
                    }
                }
            }

            yield return new WaitForEndOfFrame();
        }
        _isAgentArrivedCheckCoroutineRunning = false;
    }
}

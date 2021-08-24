using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class FinishlineReferee : MonoBehaviour
{
    #region Singleton Things
    private static FinishlineReferee _instance;
    public static FinishlineReferee Instance
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


    public GameObject PaintPlatform;
    [SerializeField] private Player _player;

    //When the player collide with finish text
    private void OnTriggerEnter(Collider collider)
    {
        Racer racerId = collider.gameObject.GetComponent<Racer>();
        if (racerId)
        {
            if (racerId.RType == RacerType.AI)
            {
                if (!racerId.RaceStatus.IsFinished)
                {
                    racerId.RaceStatus.IsFinished = true;
                    
                }
            }
            else if (racerId.RType == RacerType.Player)
            {
                //Player things...
                racerId.RaceStatus.IsFinished = true;
                if (RaceManager.Instance.IsPlayerFinishedAsChampion())
                {
                    racerId.RaceStatus.IsChampion = true;
                }
                RaceManager.Instance.PlayerFinishedRaceEvent.Invoke();
            }

        }

    }


}

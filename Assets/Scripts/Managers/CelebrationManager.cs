using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CelebrationManager : MonoBehaviour
{
    #region Singleton Things
    private static CelebrationManager _instance;
    public static CelebrationManager Instance
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


    [SerializeField] private GameObject _celebrationConfettiRight;
    [SerializeField] private GameObject _celebrationConfettiLeft;
    void Start()
    {
        RaceManager.Instance.PlayerFinishedRaceEvent.AddListener(ShowRaceResult);
        PaintingManager.Instance.PaintingDoneEvent.AddListener(ShowPaintResult);
    }

    //When player finished race, result of the race is shown to the player.
    public void ShowRaceResult()
    {
        if (RaceManager.Instance.IsPlayerFinishedAsChampion())
        {
            InGameUI.Instance.ShowPlayerRaceResult("You Win!\n Paint the Wall", new Color32(0, 255, 0, 255));
        }
        else
            InGameUI.Instance.ShowPlayerRaceResult("You Lost!\n Paint the Wall", new Color32(255, 0, 0, 255));
        
    }

    //When the player paints the paint wall at 100%, result of the painting is shown to the player.
    public void ShowPaintResult()
    {
        if (RaceManager.Instance.IsPlayerFinishedAsChampion())
        {
            InGameUI.Instance.ShowPlayerPaintingResult("You are LEGEND!", new Color32(0, 255, 0, 255));
            FireConfetti();
        }
        else
            InGameUI.Instance.ShowPlayerPaintingResult("Try Again!\n Be Champion!", new Color32(0, 255, 229, 255));
        
    }

    public void FireConfetti()
    {
        StartCoroutine(FireConfettiRoutine(15f));
    }
    IEnumerator FireConfettiRoutine(float visibilityTime)
    {
        _celebrationConfettiRight.gameObject.SetActive(true);
        _celebrationConfettiLeft.gameObject.SetActive(true);
        yield return new WaitForSeconds(visibilityTime);
        _celebrationConfettiRight.gameObject.SetActive(false);
        _celebrationConfettiLeft.gameObject.SetActive(false);
    }
}

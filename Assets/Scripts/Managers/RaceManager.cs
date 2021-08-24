using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class RaceManager : MonoBehaviour
{
    #region Singleton Things
    private static RaceManager _instance;
    public static RaceManager Instance
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

    public bool IsRaceStarted { get; set; } = false;
    //It is triggered when the player presses the "Start Race" button.
    [HideInInspector] public UnityEvent StartRaceEvent = new UnityEvent();
    //It is triggered when the player presses the "Reset" button.
    [HideInInspector] public UnityEvent ResetRaceEvent = new UnityEvent();
    //It is triggered when racers list updated.
    [HideInInspector] public UnityEvent UpdateRankEvent = new UnityEvent();
    ////It is triggered when the player arrived finish line.
    [HideInInspector] public UnityEvent PlayerFinishedRaceEvent = new UnityEvent();

    private Coroutine _updateRankCoroutine;
    private bool _isUpdateRankCoroutineRunning;
    //If the player finishes the race, this value is set to false because the rank list does not need to be updated.
    private bool _shouldUpdateRank;

    public List<Racer> Racers = new List<Racer>();

    private void Start()
    {
        if ((GameObject.FindObjectsOfType<Opponent>().Length + GameObject.FindObjectsOfType<Player>().Length) > Racers.Count)
            Debug.LogError("Please set all racers to 'racers' list!");
        
        StartUpdateRank();
        ResetRaceEvent.AddListener(StartUpdateRank);
        PlayerFinishedRaceEvent.AddListener(StopUpdateRank);
    }



    private void StartUpdateRank()
    {
        _shouldUpdateRank = true;
        if (_isUpdateRankCoroutineRunning)
        {
            StopCoroutine(_updateRankCoroutine);            
        }

        _updateRankCoroutine = StartCoroutine(UpdateRankRoutine());

    }

    IEnumerator UpdateRankRoutine()
    {
        _isUpdateRankCoroutineRunning = true;

        //For start function priority ambigious prevention
        yield return new WaitForSeconds(3f);

        bool isAllRacerFinished = false;
        while (!isAllRacerFinished && _shouldUpdateRank)
        {
            
            isAllRacerFinished = !IsAnyRacerStillRacing();
            UpdateRank();
            yield return new WaitForSeconds(0.1f);
        }

        _isUpdateRankCoroutineRunning = false;
    }


    public void UpdateRank()
    {
        
        RacerBubbleSort(ref Racers);
        //UpdateRank triggered for update UI rank list, etc.
        UpdateRankEvent.Invoke();
    }

    //The distance of the racers to the finish line is sorted by the bubble sort algorithm. If any racer reaches the finish line, they are not included in the ranking algorithm.
    public void RacerBubbleSort(ref List<Racer> sortableList)
    {
        var itemMoved = false;
        do
        {
            itemMoved = false;
            for (int i = 0; i < sortableList.Count - 1; i++)
            {
                if(sortableList[i].RaceStatus.IsFinished)
                    continue;

                if (sortableList[i].RaceStatus.FinishlineDistance > sortableList[i + 1].RaceStatus.FinishlineDistance)
                {
                    var lowerValue = sortableList[i + 1];
                    sortableList[i + 1] = sortableList[i];
                    sortableList[i] = lowerValue;
                    itemMoved = true;
                }
            }
        } while (itemMoved);
    }


    public bool IsPlayerFinishedAsChampion()
    {
        if (Racers[0].RaceStatus.RacerName == "Player")
        {
            if (Racers[0].RaceStatus.IsFinished)
                return true;
        }

        return false;
    }
    public bool IsAnyRacerStillRacing()
    {
        for (int i = 0; i < Racers.Count; i++)
        {
            if (!Racers[i].RaceStatus.IsFinished) 
                return true;
            
        }

        return false;
    }

    //When the player finishes the race, the ranking list is updated last time and the rank update ends.
    public void StopUpdateRank()
    {
        //Rank updating last one
        UpdateRank();
        _shouldUpdateRank = false;
    }

    //"Start Race"Button OnClick Function
    public void StartRace()
    {
        UIManager.Instance.OpenMenu(MenuName.InGameGeneralUI);
        IsRaceStarted = true;
        StartRaceEvent.Invoke();

    }
    //"Reset" Button OnClick Function
    public void ResetRace()
    {
        UIManager.Instance.OpenMenu(MenuName.StartMenu);
        IsRaceStarted = false;
        ResetRaceEvent.Invoke();
    }
    

}

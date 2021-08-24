using System.Collections;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    #region Singleton Things
    private static InGameUI _instance;
    public static InGameUI Instance
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

    public GameObject RankPanel;
    [SerializeField] private GameObject _rankElementPrefab;


    [SerializeField] private GameObject _playerRaceResultTextGo;

    [SerializeField] private GameObject _paintingResultTextGo;


    private List<RankElement> rankElements = new List<RankElement>();

    void Start()
    {

        RaceManager.Instance.StartRaceEvent.AddListener(GenerateRankPanel);
        RaceManager.Instance.UpdateRankEvent.AddListener(UpdateRankList);
        RaceManager.Instance.ResetRaceEvent.AddListener(CloseAllResultText);
        //RaceManager.Instance.PlayerFinishedRaceEvent.AddListener(CloseRankList);
        
        //Rank list closing when painting start.
        PaintingManager.Instance.ReadyForPaintingEvent.AddListener(CloseRankList);
        

    }

    //The rank list in the interface is created.
    public void GenerateRankPanel()
    {
        //Cleaning the UI if it was created before.
        if (rankElements.Count > 0)
        {
            for (int i = 0; i < rankElements.Count; i++)
            {
                Destroy(rankElements[i].gameObject);
            }
            rankElements.Clear();
        }

        //If the rank list is closed, it opens.
        OpenRankList();

        int racerCount = RaceManager.Instance.Racers.Count;

        float onePanelElementHeight = RankPanel.GetComponent<GridLayoutGroup>().cellSize.y;
        float plusMemberWindow = (onePanelElementHeight * racerCount) + 5;
        //The vertical size of the rank list is adjusted according to the number of players. +5 is buffer
        RankPanel.GetComponent<RectTransform>().sizeDelta = new Vector2(RankPanel.GetComponent<RectTransform>().sizeDelta.x, (plusMemberWindow));

        //Rank elements are created in the ranking list according to the number of racers.
        for (int i = 0; i < racerCount; i++)
        {
            GameObject newRacer = Instantiate(_rankElementPrefab, RankPanel.transform);
            RankElement newRankElement = newRacer.GetComponent<RankElement>();
            //The Element's RankElement instance is referenced to a Racer's Racer instance.
            newRankElement.Racer = RaceManager.Instance.Racers[i];
            rankElements.Add(newRankElement);
        }
    }

    public void UpdateRankList()
    {
        if (rankElements.Count == 0)
            return;

        int racerCount = RaceManager.Instance.Racers.Count;
        for (int i = 0; i < racerCount; i++)
        {
            //If the Racer referenced by the RankElement object has finished the race, the loop continues.
            if (rankElements[i].IsFinished)
                continue;

            //The RankElement list and UI are updating if the Racer in the updated racer rank has finished the race.
            if (RaceManager.Instance.Racers[i].RaceStatus.IsFinished)
            {
                rankElements[i].Racer = RaceManager.Instance.Racers[i];
                rankElements[i].Finish();
            }
            //The RankElement list updating..
            else
                rankElements[i].Racer = RaceManager.Instance.Racers[i];


        }
    }
    

    public void ShowPlayerRaceResult(string resultText, Color32 resultColor)
    {
        PrepareResult(_playerRaceResultTextGo.GetComponent<TextMeshProUGUI>(), resultText, resultColor);
        StartCoroutine(TextShowRoutine(_playerRaceResultTextGo, 4f));
    }

    public void ShowPlayerPaintingResult(string resultText, Color32 resultTextColor)
    {
        PrepareResult(_paintingResultTextGo.GetComponent<TextMeshProUGUI>(), resultText, resultTextColor);
        StartCoroutine(TextShowRoutine(_paintingResultTextGo, 15f));
    }

    public void PrepareResult(TextMeshProUGUI text, string resultText, Color32 resultTextColor)
    {
        text.SetText(resultText);
        text.faceColor = resultTextColor;
    }

    IEnumerator TextShowRoutine(GameObject textGo, float visibilityTime)
    {
        textGo.SetActive(true);
        yield return new WaitForSeconds(visibilityTime);
        textGo.SetActive(false);
    }


    public void CloseAllResultText()
    {
        _paintingResultTextGo.SetActive(false);
        _playerRaceResultTextGo.SetActive(false);
    }


    public void CloseRankList()
    {
        if (RankPanel.activeSelf)
            RankPanel.SetActive(false);
    }

    public void OpenRankList()
    {
        if (!RankPanel.activeSelf)
            RankPanel.SetActive(true);
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This data class is attached to the RankElement prefab. This data class instances is used to update the UI element
/// </summary>
public class RankElement : MonoBehaviour
{
    private Racer _racer;

    public Racer Racer
    {
        get
        {
            return _racer;
        }
        set
        {
            _racer = value;
            RacerName.SetText(_racer.RaceStatus.RacerName);
        }
    }
    [SerializeField] private TextMeshProUGUI RacerName;
    [SerializeField] private Image ElementImage;
    [HideInInspector] public bool IsFinished;
    public void Finish()
    {
        IsFinished = true;
        ElementImage.color = Color.green;
    }
}

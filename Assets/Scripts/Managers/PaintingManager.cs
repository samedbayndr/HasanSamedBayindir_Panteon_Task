using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Es.InkPainter;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using System;

public class PaintingManager : MonoBehaviour
{
    #region Singleton Things
    private static PaintingManager _instance;
    public static PaintingManager Instance
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

    [SerializeField] private GameObject _paintPlatform;
    [SerializeField] private Transform _warpTrans;
    [SerializeField] private GameObject _paintArea;
    [SerializeField] private Brush _brush;
    [SerializeField] private Player _player;
    [SerializeField] private InkCanvas _wallInkCanvas;
    [SerializeField] private Renderer _wallRenderer;
    [SerializeField] private TextMeshPro _wallPaintPercentageText;

    //This event is to start the painting process. Invoke when the player teleports to the painting platform.
    [HideInInspector] public UnityEvent ReadyForPaintingEvent = new UnityEvent();
    //It is triggered when the wall is 100% painted.
    [HideInInspector] public UnityEvent PaintingDoneEvent = new UnityEvent();

    //When the ReadyForPaintingEvent event is triggered, it will be true. When set to true, it allows the user to access paint controls, etc.
    private bool _isPaintable;
    
    private LayerMask _wallLayerMask;
    private LayerMask _playerMask;

    private double _paintPercentage;

    public double PaintPercentage
    {
        get
        {
            return _paintPercentage;
        }
        set
        {
            _paintPercentage = value;
            if (_paintPercentage.Equals(100))
            {
                //Paint done event triggered.
                PaintingDoneEvent.Invoke();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        _wallLayerMask = LayerMask.NameToLayer(Layer.Wall);
        _playerMask = LayerMask.GetMask(Layer.Player);
        
        RaceManager.Instance.PlayerFinishedRaceEvent.AddListener(SetPaintPlatform);
        PaintingManager.Instance.ReadyForPaintingEvent.AddListener(ReadyForPainting);
        PaintingManager.Instance.PaintingDoneEvent.AddListener(PreventPaint);
        RaceManager.Instance.ResetRaceEvent.AddListener(ResetPaintPlatform);
        
    }

    //When the player finishes the race, the paint platform becomes visible and the player is teleported to this area.
    public void SetPaintPlatform()
    { 
        _player.WarpPaintingPosition(_warpTrans.position);
        if (!_paintPlatform.activeSelf)
            _paintPlatform.SetActive(true);

    }

    //If the user presses the reset button, the platform is reset and becomes invisible.
    public void ResetPaintPlatform()
    {

        if (_paintPlatform.activeSelf)
            _paintPlatform.SetActive(false);

        PreventPaint();
        _wallInkCanvas.ResetPaint();
        _wallPaintPercentageText.SetText("0.0%");
    }

    
    public void PreventPaint()
    {
        _isPaintable = false;
    }

    //Once the platform is ready, the user's paint controls are allowed when the ReadyForPaintingEvent is triggered.
    public void ReadyForPainting()
    {
        _isPaintable = true;
        StartCoroutine(ColorPercentageRoutine());
    }

    void Update()
    {
        // If _isPaintable is true, User can paint by pressing the left mouse button.
        if (_isPaintable)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit, 30f, ~_playerMask))
                {
                    _wallInkCanvas.Paint(_brush, raycastHit);
                }
            }

            
        }
    }



    
    IEnumerator ColorPercentageRoutine()
    {
        while (_isPaintable)
        {   //Wall MainTexture caching
            Texture mainTexture = _wallRenderer.material.mainTexture;
            //Creating texture2D object according to mainTexture width and height 
            Texture2D texture2D = new Texture2D(mainTexture.width, mainTexture.height, TextureFormat.RGBA32, false);

            //Creating renderTexture object according to mainTexture width and height 
            RenderTexture renderTexture = new RenderTexture(mainTexture.width, mainTexture.height, 32);
            //Copy mainTexture data to renderTexture
            Graphics.Blit(mainTexture, renderTexture);
            //RenderTexture.active = renderTexture;
            //renderTexture object reading and texture2D filling.
            texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
            texture2D.Apply();
            
            //Wall painting color is Red(255, 0, 0, 255). So, When search pixel color green=0 with linq, we can find colored pixels count
            int redPixelCount = texture2D.GetPixels32().Count(a => a.g == 0);

            //Finding paint percentage 
            double purePercentage = (double) (100 * redPixelCount) / (renderTexture.width * renderTexture.height);
            PaintPercentage = System.Math.Round(purePercentage, 2, MidpointRounding.AwayFromZero);
            _wallPaintPercentageText.SetText(PaintPercentage.ToString("F")+"%");

            yield return new WaitForSeconds(0.5f);
        }
    }
}

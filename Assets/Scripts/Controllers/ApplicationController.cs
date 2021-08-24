using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplicationController : MonoBehaviour
{
    #region Singleton Things
    private static ApplicationController _instance;
    public static ApplicationController Instance
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


    public void Quit()
    {
        Application.Quit();
    }
}

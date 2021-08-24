using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{

    #region Singleton Things
    private static UIManager _instance;
    public static UIManager Instance
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

    
    public GameObject[] Menus;

    // Find and open menu with menu gameobject. 
    public bool OpenMenu(GameObject menu)
    {
        bool isRelatedMenuOpened = false;
        foreach (var subMenu in Menus)
        {
            if (menu == subMenu)
            {
                subMenu.SetActive(true);
                isRelatedMenuOpened = true;
            }
            else
                subMenu.SetActive(false);
        }

        return isRelatedMenuOpened;
    }


    // Find and close menu with menu gameobject. 
    public void CloseMenu(GameObject menu)
    {
        foreach (var subMenu in Menus)
        {
            if (subMenu == menu)
            {
                menu.SetActive(false);
                return;
            }
        }
        Debug.Log("Menu not found!");

    }

    // Find and open menu with menu name. 
    public void OpenMenu(string menuName)
    {
        foreach (var menu in Menus)
        {
            if (menu.name == menuName)
            {
                menu.SetActive(true);
            }
            else
                menu.SetActive(false);
        }
    }

    // Find and open menu with menu name. 
    public void CloseMenu(string menuName)
    {
        foreach (var menu in Menus)
        {
            if (menu.name == menuName)
            {
                menu.SetActive(false);
                return;
            }
        }
        Debug.Log("Menu not found!");
    }

}

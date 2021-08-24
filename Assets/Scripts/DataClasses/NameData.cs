using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class NameData : MonoBehaviour
{

    public static string DataFilePath = "AI/AIName";
    private static NameData _instance;

    public static NameData Instance
    {
        get
        {
            return _instance;
        }
    }

    public void Awake()
    {
        if (_instance != null || _instance != this)
        {
            Destroy(_instance);
        }

        _instance = this;
        //AIName.txt read and store Names string list
        Names = Resources.Load<TextAsset>(DataFilePath).text
            .Split(',').ToList();
        Debug.Log(Names.Count);
    }

    public List<string> Names = new List<string>();
    private List<string> UsedNames = new List<string>();
    public string GetRandomName()
    {
        if (Names.Count != 0)
        {
            for (int i = 0; i < Names.Count; i++)
            {

                string name = Names[UnityEngine.Random.Range(0, Names.Count)];
                //If the name was previously assigned to an AI, a different name is chosen.
                if (!UsedNames.Contains(name))
                {
                    UsedNames.Add(name);
                    return name;
                }
            }

            return "Dorhamin";
        }

        return "";

    }
}


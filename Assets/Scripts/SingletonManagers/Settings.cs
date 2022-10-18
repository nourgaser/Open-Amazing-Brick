using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private static Settings instance;

    public static Settings Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;

            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 144;
        }
        DontDestroyOnLoad(this.gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundaries : MonoBehaviour
{

    private static Boundaries instance;

    public static Boundaries Instance
    {
        get { return instance; }
    }

    public static Vector2 screenBounds;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;

            Camera MainCamera = GameObject.FindObjectOfType<Camera>();
            screenBounds = MainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, MainCamera.transform.position.z));
        }
        DontDestroyOnLoad(this.gameObject);
    }
}

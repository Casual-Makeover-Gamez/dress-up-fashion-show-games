using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class GAManager : MonoBehaviour
{
    // Start is called before the first frame update
    // if(GAManager.Instance)GAManager.Instance.LogDesignEvent("Scene:" + SceneManager.GetActiveScene().name + SceneManager.GetActiveScene().buildIndex);
    public static GAManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
        InitGA();
    }

    void InitGA()
    {
        GameAnalytics.Initialize();
    }

    public void LogDesignEvent(string eventName)
    {
        GameAnalytics.NewDesignEvent(eventName);
    }

}

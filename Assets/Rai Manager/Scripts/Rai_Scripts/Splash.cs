using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Splash : MonoBehaviour
{
    public enum Scenes
    {
        Splash,
        MyMainMenu,
    }
    public Scenes SceneName;
    public Image fillBar;
    [Range(4, 10)]
    public float TimeToLoad;

    // Start is called before the first frame update
    void Start()
    {
        if (GAManager.Instance) GAManager.Instance.LogDesignEvent("Scene:" + SceneManager.GetActiveScene().name + SceneManager.GetActiveScene().buildIndex);
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        StartCoroutine(loadScene(SceneName.ToString()));
    }
    IEnumerator loadScene(string str)
    {
        fillBar.fillAmount = 0;
        while(fillBar.fillAmount < 1)
        {
            fillBar.fillAmount += Time.deltaTime/ TimeToLoad;
            yield return null;
        }
        SceneManager.LoadScene(str);
    }
}

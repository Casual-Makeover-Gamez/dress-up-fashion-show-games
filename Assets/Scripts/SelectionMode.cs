using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectionMode : MonoBehaviour
{
    public GameObject AdPenl;
    public GameObject loadindPanel;
    public Image fillBar;
    public GameObject VsPanel;
    public GameObject StartBtn;
    public Image PlayerAvatar,OpponentAvater;
    public Text PlayerName,OpponentName;
    public Sprite[] AvatarSprites;
    public string[] oppoNames = { "Jackline", "Alex", "Diana", "Muskan", "Jannat", "Iqra", "Dipti", "Maria" };
    int oppoNameNum;
    int oppoIndex;
    string SceneName;


    private void Start()
    {
        if (GAManager.Instance) GAManager.Instance.LogDesignEvent("Scene:" + SceneManager.GetActiveScene().name + SceneManager.GetActiveScene().buildIndex);
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
            Rai_SaveLoad.LoadProgress();
        }
        PlayerName.text = SaveData.Instance.ProfileName;
        PlayerAvatar.sprite = AvatarSprites[SaveData.Instance.PlayerAvatar];

    }
    IEnumerator VSAnim()
    {
        yield return new WaitForSeconds(2.5f);
        for (int i = 0; i < Random.Range(25, 30); i++)
        {
            oppoIndex = Random.Range(0, AvatarSprites.Length);
            OpponentAvater.gameObject.SetActive(false);
            OpponentAvater.gameObject.SetActive(true);
            OpponentAvater.sprite = AvatarSprites[oppoIndex];
            oppoNameNum = Random.Range(0, 7);
            OpponentName.text = oppoNames[oppoNameNum].ToString();
            yield return new WaitForSeconds(0.1f);
        }
        SaveData.Instance.oppoAvatar = GameManager.Instance.OpponentIndex = oppoIndex;
        GameManager.Instance.OpponentName = oppoNameNum.ToString();
        OpponentAvater.sprite = AvatarSprites[oppoIndex];
        OpponentName.text = oppoNames[oppoNameNum].ToString();
        yield return new WaitForSeconds(0.5f);
        StartBtn.SetActive(true);
    }
    // Start is called before the first frame update
    public void Play(string str)
    {
        SceneName = str;
        VsPanel.SetActive(true);
        StartCoroutine(VSAnim());
    }
    public void LoadScene()
    {
        loadindPanel.SetActive(true);
        StartCoroutine(loadScene(SceneName));
        StartCoroutine(ShowInterstitialAD());
    }
    IEnumerator loadScene(string str)
    {
        fillBar.fillAmount = 0;
        while(fillBar.fillAmount < 1)
        {
            fillBar.fillAmount += Time.deltaTime/ 4;
            yield return null;
        }
        SceneManager.LoadScene(str);
    }
    IEnumerator ShowInterstitialAD()
    {
        if (MyAdsManager.Instance)
        {
            if (MyAdsManager.Instance.IsInterstitialAvailable())
            {
                if (AdPenl)
                {
                    AdPenl.SetActive(true);
                    yield return new WaitForSeconds(0.5f);
                    AdPenl.SetActive(false);
                }
                MyAdsManager.Instance.ShowInterstitialAds();
            }
        }
    }
}

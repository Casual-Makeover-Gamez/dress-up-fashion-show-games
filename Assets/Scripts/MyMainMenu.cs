using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MyMainMenu : MonoBehaviour
{
    public GameObject loadindPanel;
    public Image fillBar;
    public Image PlayerAvatarSelected,payerAvatarInTopBar;
    public GameObject AvatarPanel;
    public Sprite[] AvatarSprites;
    private List<ItemInfo> AvatarList = new List<ItemInfo>();
    public AudioSource CategorySfx;
    public InputField editorName;
    public Text mainName,inputFiedText;
    private void Start()
    {
        if (GAManager.Instance) GAManager.Instance.LogDesignEvent("Scene:" + SceneManager.GetActiveScene().name + SceneManager.GetActiveScene().buildIndex);
        if (GameManager.Instance.Initialized == false)
        {
            GameManager.Instance.Initialized = true;
            Rai_SaveLoad.LoadProgress();
        }
        if (SaveData.Instance.ProfileCreated == false)
        {
            AvatarPanel.SetActive(true);
        }
        else
        {
            AvatarPanel.SetActive(false);
        }
        editorName.text = mainName.text = SaveData.Instance.ProfileName;
        PlayerAvatarSelected.sprite = payerAvatarInTopBar.sprite  = AvatarSprites[SaveData.Instance.PlayerAvatar];

        #region Initialing Cat
        if (AvatarPanel)
        {
            var sprayInfo = AvatarPanel.GetComponentsInChildren<ItemInfo>();
            for (int i = 0; i < sprayInfo.Length; i++)
            {
                AvatarList.Add(sprayInfo[i]);
            }
        }
        //Adding Click listeners to btns 
        for (int i = 0; i < AvatarList.Count; i++)
        {
            AvatarList[i].itemBtn.GetComponent<Image>().sprite = AvatarSprites[i];
            if (AvatarList[i].itemBtn)
            {
                int Index = i;
                AvatarList[i].itemBtn.onClick.AddListener(() =>
                {
                    Avatar(Index);
                });
            }
        }
        #endregion
    }
    public void Avatar(int num)
    {
        CategorySfx.Play();
        SaveData.Instance.PlayerAvatar = num;
        PlayerAvatarSelected.gameObject.SetActive(false);
        PlayerAvatarSelected.gameObject.SetActive(true);
        PlayerAvatarSelected.sprite = payerAvatarInTopBar.sprite = AvatarSprites[SaveData.Instance.PlayerAvatar];
        Rai_SaveLoad.SaveProgress();
    }
    public void Save()
    {
        AvatarPanel.SetActive(false);
        SaveData.Instance.ProfileCreated = true;
        SaveData.Instance.ProfileName = inputFiedText.text = mainName.text = editorName.text;
        Rai_SaveLoad.SaveProgress();
    }
    public void Edit()
    {
        AvatarPanel.SetActive(true);
    }
    // Start is called before the first frame update
    public void Play(string str)
    {
        loadindPanel.SetActive(true);
        StartCoroutine(loadScene(str));
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
}

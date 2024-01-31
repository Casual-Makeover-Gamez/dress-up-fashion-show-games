using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class CrossPromotionManager : MonoBehaviour
{
    public int maxLoads = 2;
    private string iconName = "";
    private string serverPath = "";

    public static CrossPromotionManager Instance;
    public delegate void actionCpLoaded(int index);
    public static event actionCpLoaded onCpLoadedEvent;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        if (GamesInfo.Instance != null)
        {
            StartCoroutine(GetText());
        }
        CrossPromotionManager.onCpLoadedEvent += EnableCP;
    }

    IEnumerator GetTexture(int listIndex)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(serverPath);
        yield return request.SendWebRequest();
        if (request.isDone && request.result == UnityWebRequest.Result.Success)
        {
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(request.error);
            else
            {
                Texture2D tex = DownloadHandlerTexture.GetContent(request);
                Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(tex.width / 2, tex.height / 2));
                GamesInfo.Instance.gamesData[listIndex].appIcon = sprite;
                //Debug.Log(listIndex);
                onCpLoadedEvent?.Invoke(listIndex);
            }
        }
        yield return null;
    }

    IEnumerator GetText()
    {
        //You Have To Change the Server link 
        //UnityWebRequest request = UnityWebRequest.Get("Enter Your github link");
        UnityWebRequest request = UnityWebRequest.Get("https://raw.githubusercontent.com/alinawaz1997/crosspromotion/main/CMGjson/DressUpFashionShowGames.json");
        yield return request.SendWebRequest();
        if (request.isDone && request.result == UnityWebRequest.Result.Success)
        {
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
                Debug.Log(request.error);
            else
            {
                string fileContent = request.downloadHandler.text.ToString();
                Debug.Log(fileContent);
                JsonUtility.FromJsonOverwrite(fileContent, GamesInfo.Instance);
                if (GamesInfo.Instance.adsData[0].canShowCP)
                {
                    for (int i = 0; i < GamesInfo.Instance.gamesData.Count; i++)
                    {
                        if (i < maxLoads)
                        {
                            iconName = GamesInfo.Instance.gamesData[i].appName;
                            //serverPath = "Enter Your github link" + iconName + ".png";
                            serverPath = "https://raw.githubusercontent.com/alinawaz1997/crosspromotion/main/CMGicon/" + iconName + ".png";
                            StartCoroutine(GetTexture(i));
                        }
                    }
                }
            }
        }
        yield return null;
    }
    private void OnDestroy()
    {
        CrossPromotionManager.onCpLoadedEvent -= EnableCP;
    }
public void EnableCP(int index)
    {
        cpIcons[index].SetActive(true);
    }

public GameObject[] cpIcons;
}

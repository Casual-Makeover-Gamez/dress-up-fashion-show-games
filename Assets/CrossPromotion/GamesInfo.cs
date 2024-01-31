using System.Collections.Generic;
using UnityEngine;

public class GamesInfo
{
    public static GamesInfo instance;
    public static GamesInfo Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GamesInfo();
            }
            return instance;
        }
    }
    public List<GamesData> gamesData = new List<GamesData>();
    public List<AdsData> adsData = new List<AdsData>();
    public GamesInfo() { }
    public GamesInfo(List<GamesData> _gamesData, List<AdsData> _adsData)
    {
        gamesData = _gamesData;
        adsData = _adsData;
    }
}

[System.Serializable]
public class GamesData
{
    public string appVersion;
    public string appName;
    public string appLink;
    public Sprite appIcon;
}
[System.Serializable]
public class AdsData
{
    public bool canShowCP;
}

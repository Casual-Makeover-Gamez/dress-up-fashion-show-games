using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class PlayerProps
{
    public string playerName;
    public int playerHealth;
    public int playerDamage;
    public int playerRange;
    public bool isLocked = true;
}
[System.Serializable]
public class PartyProps
{
    public List<bool> dressLocked = new List<bool>();
    public List<bool> hairLocked = new List<bool>();
    public List<bool> bangelsLocked = new List<bool>();
    public List<bool> shoesLocked = new List<bool>();
    public List<bool> earRingLocked = new List<bool>();
    public List<bool> eyeShadesLocked = new List<bool>();
    public List<bool> lipsTickLocked = new List<bool>();
    public List<bool> necklaceLocked = new List<bool>();
    public List<bool> noseRingLocked = new List<bool>();
    public List<bool> bagLocked = new List<bool>();
    public List<bool> BlushLocked = new List<bool>();
    public List<bool> BGLocked = new List<bool>();
}
[System.Serializable]
public class CasualProps
{
    public List<bool> dressLocked = new List<bool>();
    public List<bool> hairLocked = new List<bool>();
    public List<bool> bangelsLocked = new List<bool>();
    public List<bool> shoesLocked = new List<bool>();
    public List<bool> earRingLocked = new List<bool>();
    public List<bool> eyeShadesLocked = new List<bool>();
    public List<bool> lipsTickLocked = new List<bool>();
    public List<bool> necklaceLocked = new List<bool>();
    public List<bool> noseRingLocked = new List<bool>();
    public List<bool> bagLocked = new List<bool>();
    public List<bool> BGLocked = new List<bool>();
}
[System.Serializable]
public class ModranProps
{
    public List<bool> dressLocked = new List<bool>();
    public List<bool> hairLocked = new List<bool>();
    public List<bool> bangelsLocked = new List<bool>();
    public List<bool> shoesLocked = new List<bool>();
    public List<bool> earRingLocked = new List<bool>();
    public List<bool> eyeShadesLocked = new List<bool>();
    public List<bool> lipsTickLocked = new List<bool>();
    public List<bool> necklaceLocked = new List<bool>();
    public List<bool> noseRingLocked = new List<bool>();
    public List<bool> bagLocked = new List<bool>();
    public List<bool> BGLocked = new List<bool>();
}
[System.Serializable]
public class SelectionElements
{
    public List<bool> vapeLocked = new List<bool>(); 
}


[System.Serializable]
public class SaveData
{

    public static SaveData instance;
    public static SaveData Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new SaveData();
            }
            return instance;
        }
    }
    public bool RemoveAds = false;
    public int LevelsUnlocked = 1;
    public int EventsUnlocked = 0;
    public int PlayerAvatar = 0;
    public int oppoAvatar = 0;
    public int Selectedvape;
    public string ProfileName = "Player";
    public bool ProfileCreated = false;
    public bool isSound = true, isMusic = true, isVibration = true, isRightControls = true;
    public int Coins = 2000;
    public List<PlayerProps> Players = new List<PlayerProps>();
    public SelectionElements vapeSelectionProps = new SelectionElements();
    public PartyProps partyProps = new PartyProps();
    public CasualProps casualProps = new CasualProps();
    public ModranProps modranProps = new ModranProps();
    public string hashOfSaveData;

    //Constructor to save actual GameData
    public SaveData() { }

    //Constructor to check any tampering with the SaveData
    public SaveData(bool ads, int levelsUnlocked, int eventsUnlocked, int coins, bool soundOn, bool musicOn, bool vibrationOn, bool rightControls, List<PlayerProps> _players, 
        SelectionElements _selectionElements, PartyProps _partyProps, CasualProps _CasualProps, ModranProps _ModranProps)
    {
        RemoveAds = ads;
        LevelsUnlocked = levelsUnlocked;
        EventsUnlocked = eventsUnlocked;
        Coins = coins;
        isSound = soundOn;
        isMusic = musicOn;
        isVibration = vibrationOn;
        isRightControls = rightControls;
        Players = _players;
        vapeSelectionProps = _selectionElements;
        partyProps = _partyProps;
        casualProps = _CasualProps;
        modranProps = _ModranProps;
    }
}
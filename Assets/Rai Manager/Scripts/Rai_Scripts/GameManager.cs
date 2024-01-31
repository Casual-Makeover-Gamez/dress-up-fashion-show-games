using UnityEngine;
using System.Collections;

public class GameManager
{

	private static GameManager instance;

	public static GameManager Instance
	{
		get
		{
			if (instance == null)
			{
				instance = new GameManager ();
			}
			return instance;
		}
	}

	public bool Initialized = false;
    public int selectedMode = 1;
    public int OpponentIndex;
    public string OpponentName;


}
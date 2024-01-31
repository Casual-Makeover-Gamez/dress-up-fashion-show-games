using UnityEngine;
using System.Collections;

public class BgSoundController : MonoBehaviour {

    private static BgSoundController instance = null;
    public static BgSoundController Instance
    {
        get { return instance; }
    }
    void Awake() {
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }		
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Links : MonoBehaviour
{

    public void MoreGames()
    {
        Application.OpenURL("https://www.amazon.com/s?i=mobile-apps&rh=p_4%3AFunLab+Studios");
    }
    public void RateUS()
    {
        Application.OpenURL("http://www.amazon.com/gp/mas/dl/android?p=com.cgsamazon.dressup.fashionshowmakeover.games");
    }
    public void PP()
    {
        Application.OpenURL("https://docs.google.com/document/d/1-dUBBPWGQ31tI9QTQtgfaf5-02KP75ZB8lky39VsUCo/edit");
    }

}

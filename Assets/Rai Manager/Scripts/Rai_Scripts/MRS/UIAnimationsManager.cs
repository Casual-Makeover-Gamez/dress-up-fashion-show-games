using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIAnimationsManager : MonoBehaviour
{
    public List<GameObject> scaleAbleItems;
    public List<GameObject> nonScaleAbleItems;
    public bool isInitialDelay;
    [Range(0, 5)]
    public float initialDelay;
    [Range(0, 5)]
    public float scaleTime = 1f, nextScaleDelay = 0.3f, appearTime = 1f;
    [Range(0, 5)]
    public int noOfRotations = 2;
    public bool alphaChange;
    public bool fromTo;
    public Vector3 scaleFrom, scaleTo;
    public bool ignoreTime;
    public bool inLoop;
    [Range(0, 10)]
    public float loopDelay;
    public enum ScaleType
    {
        linear, easeOutBack, easeInBack, easeInBounce, easeOutBounce, easeOutElastic, easeInElastic
    }
    public enum AnimationType
    {
        firstAnimType, secondAnimType, thirdAnimType, forthAnimType
    }
    public ScaleType scaleType;
    public AnimationType animationType;
    public bool reFresh;
    private string xAxis = "x", yAxis = "y", zAxis = "z", timeHash = "time", easeType = "easeType", rotationCycle = "rotation", ignoreTimeScale = "ignoretimescale";
    private string selectedType;
    private float timeCount;

    private void OnEnable()
    {
        //loopDelay = initialDelay + ((scaleTime + nextScaleDelay) * scaleAbleItems.Count);
        AnimEffect();
    }
    // Start is called before the first frame update
    public void AnimEffect()
    {
        if (scaleAbleItems.Count > 0)
        {
            for (int i = 0; i < scaleAbleItems.Count; i++)
            {
                if (scaleAbleItems[i])
                {
                    if (scaleAbleItems[i].GetComponent<iTween>())
                    {
                        Destroy(scaleAbleItems[i].GetComponent<iTween>());
                    }
                    if(!fromTo) scaleAbleItems[i].transform.localScale = Vector3.zero;
                    else scaleAbleItems[i].transform.localScale = scaleFrom;
                    if (alphaChange)
                    {
                        if (scaleAbleItems[i].GetComponent<Image>())
                        {
                            scaleAbleItems[i].GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
                        }
                    }
                    scaleAbleItems[i].SetActive(false);
                }
            }
            StartCoroutine(AnimEffectAction());
        }
        if (nonScaleAbleItems.Count > 0)
        {
            for (int i = 0; i < nonScaleAbleItems.Count; i++)
            {
                if (nonScaleAbleItems[i])
                    nonScaleAbleItems[i].SetActive(false);
            }
        }
    }
    IEnumerator AnimEffectAction()
    {
        if (isInitialDelay)
        {
            if (ignoreTime)
            {
                yield return new WaitForSecondsRealtime(initialDelay);
            }
            else
            {
                yield return new WaitForSeconds(initialDelay);
            }
        }
        for (int i = 0; i < scaleAbleItems.Count; i++)
        {
            if (scaleAbleItems[i])
            {
                scaleAbleItems[i].SetActive(true);
                if (fromTo)
                {
                    if (ignoreTime)
                    {
                        iTween.ScaleTo(scaleAbleItems[i], iTween.Hash(xAxis, scaleTo.x, yAxis, scaleTo.y, zAxis, scaleTo.z, timeHash, scaleTime, ignoreTimeScale, true, easeType, scaleType.ToString()));
                    }
                    else
                    {
                        iTween.ScaleTo(scaleAbleItems[i], iTween.Hash(xAxis, scaleTo.x, yAxis, scaleTo.y, zAxis, scaleTo.z, timeHash, scaleTime, ignoreTimeScale, false, easeType, scaleType.ToString()));
                    }
                }
                else
                {
                    if (ignoreTime)
                    {
                        iTween.ScaleTo(scaleAbleItems[i], iTween.Hash(xAxis, 1f, yAxis, 1f, zAxis, 1f, timeHash, scaleTime, ignoreTimeScale, true, easeType, scaleType.ToString()));
                    }
                    else
                    {
                        iTween.ScaleTo(scaleAbleItems[i], iTween.Hash(xAxis, 1f, yAxis, 1f, zAxis, 1f, timeHash, scaleTime, ignoreTimeScale, false, easeType, scaleType.ToString()));
                    }
                }
                if (animationType == AnimationType.secondAnimType)
                {
                    iTween.RotateTo(scaleAbleItems[i], iTween.Hash(xAxis, 720 * noOfRotations, timeHash, scaleTime, easeType, iTween.EaseType.linear));
                }
                else if (animationType == AnimationType.thirdAnimType)
                {
                    iTween.RotateTo(scaleAbleItems[i], iTween.Hash(yAxis, 720 * noOfRotations, timeHash, scaleTime, easeType, iTween.EaseType.linear));
                }
                else if (animationType == AnimationType.forthAnimType)
                {
                    iTween.RotateTo(scaleAbleItems[i], iTween.Hash(zAxis, 720 * noOfRotations, timeHash, scaleTime, easeType, iTween.EaseType.linear));
                }
                if (alphaChange)
                {
                    StartCoroutine(TransparencyManager(scaleAbleItems[i]));
                }
                if (ignoreTime)
                {
                    yield return new WaitForSecondsRealtime(nextScaleDelay);
                }
                else
                {
                    yield return new WaitForSeconds(nextScaleDelay);
                }
            }
        }
        if (nonScaleAbleItems.Count > 0)
        {
            for (int i = 0; i < nonScaleAbleItems.Count; i++)
            {
                if (nonScaleAbleItems[i])
                    nonScaleAbleItems[i].SetActive(true);
            }
        }
    }
    IEnumerator TransparencyManager(GameObject refItem)
    {
        float appearDelta = 1f / (appearTime * 60f);
        for (int i = 0; i < appearTime * 60; i++)
        {
            if (ignoreTime)
            {
                yield return new WaitForSecondsRealtime(appearTime / 60f);
            }
            else
            {
                yield return new WaitForSeconds(appearTime / 60f);

            }
            if (refItem.GetComponent<Image>())
            {
                refItem.GetComponent<Image>().color = new Color(1f, 1f, 1f, refItem.GetComponent<Image>().color.a + appearDelta);
            }
        }
    }
    private void Update()
    {
        if (reFresh)
        {
            reFresh = !reFresh;
            AnimEffect();
        }

        if (inLoop)
        {
            timeCount += Time.deltaTime;
            if (timeCount >= loopDelay)
            {
                timeCount = 0f;
                reFresh = !reFresh;
            }
        }
    }
}

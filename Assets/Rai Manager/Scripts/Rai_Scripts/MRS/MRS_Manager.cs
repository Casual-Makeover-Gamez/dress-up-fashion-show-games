using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region MovementProperties
[System.Serializable]
public class MovementProperties
{
    public enum MoveNow
    {
        no, yes
    }
    public enum EaseType
    {
        linear, spring, easeOutBack, easeInBack, easeInBounce, easeOutBounce, easeOutElastic, easeInElastic
    }
    public enum LoopType
    {
        none, loop, pingPong
    }
    public bool isOnEnable;
    public MoveNow moveNow;
    public EaseType easeType;
    public LoopType loopType;
    public bool moveFrom;
    public Vector3 moveFromPosition;
    public Vector3 moveToPosition;
    public bool fromCurrentPosition;
    public Transform targetPoint;
    [Range(0f, 20f)]
    public float moveTime;
    public bool isLocal, ignoreTimeScale;
    public bool startDelay;
    [Range(0.1f, 10f)]
    public float startMoveDelay;
    public bool fixMoveDelay;
    [Range(0.1f, 10f)]
    public float fixMoveDelayValue;
    public bool randomMoveDelay;
    [Range(0f, 1f)]
    public float minMoveDelayValue;
    [Range(1f, 20f)]
    public float maxMoveDelayValue;
    [HideInInspector]
    public bool isInLoop;
    private void OnFixChange()
    {
        randomMoveDelay = false;
    }
    private void OnRandomChange()
    {
        fixMoveDelay = false;
    }

    private void OnMoveFromChanged()
    {
        if (moveFrom) fromCurrentPosition = false;
    }
    private void OnFromCurrentPositionChanged()
    {
        if (fromCurrentPosition) moveFrom = false;
    }
}
#endregion

#region RotationProperties
[System.Serializable]
public class RotationProperties
{
    //[InfoBox("Initial rotation of an object must be (0 ,0 , 0) to work it correctly if not it will be set to (0,0,0) at start" +
    //    " and changing values at run time may show weird behaviour")]
    public enum RotateNow
    {
        no, yes
    }
    public enum EaseType
    {
        linear, spring, easeOutBack, easeInBack, easeInBounce, easeOutBounce, easeOutElastic, easeInElastic
    }
    public enum LoopType
    {
        none, loop, pingPong
    }
    public enum RotationType
    {
        angle, rotations
    }
    public bool isOnEnable;
    public RotateNow rotateNow;
    public EaseType easeType;
    public LoopType loopType;
    public RotationType rotationType;
    public Vector3 rotationAngle;
    [Range(0f, 20f)]
    public float rotationTime;
    public bool isLocal, ignoreTimeScale;
    public bool startDelay;
    [Range(0.1f, 10f)]
    public float startRotationDelay;
    public bool fixRotationDelay;
    [Range(0.1f, 10f)]
    public float fixRotationDelayValue;
    public bool randomRotationDelay;
    [Range(0f, 1f)]
    public float minRotationDelayValue;
    [Range(1f, 20f)]
    public float maxRotationDelayValue;
    [HideInInspector]
    public bool isStyleChanged, isFixOrRandomRotation, isLoop;
    private void OnFixChange()
    {
        randomRotationDelay = false;
    }
    private void OnRandomChange()
    {
        fixRotationDelay = false;
    }
}
#endregion

#region ScaleProperties
[System.Serializable]
public class ScaleProperties
{
    public enum ScaleNow
    {
        no, yes
    }
    public enum EaseType
    {
        linear, spring, easeOutBack, easeInBack, easeInBounce, easeOutBounce, easeOutElastic, easeInElastic
    }
    public enum LoopType
    {
        none, loop, pingPong
    }
    public bool isOnEnable;
    public ScaleNow scaleNow;
    public EaseType easeType;
    public LoopType loopType;
    public bool fromTo;
    public Vector3 scaleFrom;
    public Vector3 scaleTo;
    [Range(0f, 20f)]
    public float scaleTime;
    public bool isLocal, ignoreTimeScale;
    public bool startDelay;
    [Range(0.1f, 10f)]
    public float startScaleDelay;
    public bool fixScaleDelay;
    [Range(0.1f, 10f)]
    public float fixScaleDelayValue;
    public bool randomScaleDelay;
    [Range(0f, 1f)]
    public float minScaleDelayValue;
    [Range(1f, 20f)]
    public float maxScaleDelayValue;
    [HideInInspector]
    public bool isPingPong;
    private void OnFixChange()
    {
        randomScaleDelay = false;
    }
    private void OnRandomChange()
    {
        fixScaleDelay = false;
    }
    //private void OnScaleStyleChange()
    //{
    //    if (easeType == EaseType.onlyOnce)
    //    {
    //        fixScaleDelay = false;
    //        randomScaleDelay = false;
    //        isPingPong = false;
    //    }
    //    else
    //    {
    //        isPingPong = true;
    //    }
    //}
}
#endregion

public class MRS_Manager : MonoBehaviour
{
    #region Variables
    public MovementProperties movementProps;
    public RotationProperties rotationProps;
    public ScaleProperties scaleProps;
    private bool canMove, canRorate, canScale, setMoveDelay, setRotationDelay, setScaleDelay, isSet;
    private string xAxis = "x", yAxis = "y", zAxis = "z", easeType = "easetype", isLocal = "islocal", tweenTime = "time", loopType = "loopType", ignoreTimeScale = "ignoretimescale";
    private iTween moveTween, rotateTween, scaleTween;
    private Vector3 tempPos;
    #endregion

    #region OnEnable
    private void OnEnable()
    {
        if (movementProps.moveNow == MovementProperties.MoveNow.yes && movementProps.isOnEnable)
        {
            InitializeMove();
        }
        if (rotationProps.rotateNow == RotationProperties.RotateNow.yes && rotationProps.isOnEnable)
        {
            InitializeRotate();
        }
        if (scaleProps.scaleNow == ScaleProperties.ScaleNow.yes && scaleProps.isOnEnable)
        {
            InitializeScale();
        }
    }
    #endregion

    #region Start
    void Start()
    {
        if (movementProps.moveNow == MovementProperties.MoveNow.yes && !movementProps.isOnEnable)
        {
            InitializeMove();
        }
        if (rotationProps.rotateNow == RotationProperties.RotateNow.yes && !rotationProps.isOnEnable)
        {
            InitializeRotate();
        }
        if (scaleProps.scaleNow == ScaleProperties.ScaleNow.yes && !scaleProps.isOnEnable)
        {
            InitializeScale();
        }
        var iTweens = GetComponents<iTween>();
        for (int i = 0; i < iTweens.Length; i++)
        {
            if (iTweens[i].type == "move")
            {
                moveTween = iTweens[i];
            }
            else if (iTweens[i].type == "rotate")
            {
                rotateTween = iTweens[i];
            }
            else if (iTweens[i].type == "scale")
            {
                scaleTween = iTweens[i];
            }
        }
    }
    #endregion

    #region MovementHandler
    public void InitializeMove()
    {
        if (movementProps.fromCurrentPosition)
        {
            if (!isSet)
            {
                isSet = true;
                if (movementProps.isLocal)
                {
                    tempPos = transform.localPosition;
                }
                else
                {
                    tempPos = transform.position;
                }
            }
            else
            {
                if (movementProps.isLocal)
                {
                    transform.localPosition = tempPos;
                }
                else
                {
                    transform.position = tempPos;
                }
            }
            if (movementProps.targetPoint != null)
            {
                if (movementProps.isLocal)
                {
                    movementProps.moveToPosition = movementProps.targetPoint.localPosition;
                }
                else
                {
                    movementProps.moveToPosition = movementProps.targetPoint.position;
                }
            }
        }
        else if (movementProps.moveFrom)
        {
            if (movementProps.isLocal)
            {
                transform.localPosition = movementProps.moveFromPosition;
            }
            else
            {
                transform.position = movementProps.moveFromPosition;
            }
        }
        canMove = true;
        setMoveDelay = true;
        StartCoroutine(MovementDelayManager());
    }
    public void MovementHandler()
    {
        iTween.MoveTo(gameObject, iTween.Hash(xAxis, movementProps.moveToPosition.x, yAxis, movementProps.moveToPosition.y,
        zAxis, movementProps.moveToPosition.z, tweenTime, movementProps.moveTime, isLocal, movementProps.isLocal,
        ignoreTimeScale, movementProps.ignoreTimeScale, easeType, movementProps.easeType.ToString(),
        loopType, movementProps.loopType.ToString()));
    }

    public void MoveTo(Transform targetPoint, float moveTime, bool moveLocal = true, bool unscaleTime = false, iTween.EaseType _easeType = iTween.EaseType.linear, iTween.LoopType _loopType = iTween.LoopType.none)
    {
        iTween.MoveTo(gameObject, iTween.Hash(xAxis, targetPoint.position.x, yAxis, targetPoint.position.y,
       zAxis, targetPoint.position.z, tweenTime, moveTime, isLocal, moveLocal,
       ignoreTimeScale, unscaleTime, easeType, _easeType, loopType, _loopType));
    }

    public void MoveTo(Vector3 targetPos, float moveTime, bool moveLocal = true, bool unscaleTime = false, iTween.EaseType _easeType = iTween.EaseType.linear, iTween.LoopType _loopType = iTween.LoopType.none)
    {
        iTween.MoveTo(gameObject, iTween.Hash(xAxis, targetPos.x, yAxis, targetPos.y,
       zAxis, targetPos.z, tweenTime, moveTime, isLocal, moveLocal,
       ignoreTimeScale, unscaleTime, easeType, _easeType, loopType, _loopType));
    }
    public void Move(Vector3 targetPos, float moveDelay, bool _isLocal, bool ignoreTimeScale)
    {
        iTween.MoveTo(gameObject, iTween.Hash(xAxis, targetPos.x, yAxis, targetPos.y, zAxis, targetPos.z, tweenTime, moveDelay, isLocal, _isLocal,
          ignoreTimeScale, ignoreTimeScale, easeType, movementProps.easeType.ToString(),
          loopType, movementProps.loopType.ToString()));
    }

    IEnumerator MovementDelayManager()
    {
        if (movementProps.startDelay)
        {
            if (movementProps.ignoreTimeScale)
            {
                yield return new WaitForSecondsRealtime(movementProps.startMoveDelay);
            }
            else
            {
                yield return new WaitForSeconds(movementProps.startMoveDelay);
            }
        }
        MovementHandler();
        if ((movementProps.fixMoveDelay || movementProps.randomMoveDelay) && !movementProps.ignoreTimeScale)
        {
            while (canMove)
            {
                if (movementProps.fixMoveDelay && movementProps.fixMoveDelayValue > 0)
                {
                    yield return new WaitForSeconds(0.05f);
                    moveTween.delay = movementProps.fixMoveDelayValue;
                    canMove = false;
                }
                else if (movementProps.randomMoveDelay && movementProps.minMoveDelayValue >= 0 && movementProps.maxMoveDelayValue > 0)
                {
                    if (setMoveDelay && moveTween)
                    {
                        setMoveDelay = false;
                        float randomDelay = Random.Range(movementProps.minMoveDelayValue, movementProps.maxMoveDelayValue + 0.01f);
                        moveTween.delay = randomDelay;
                        yield return new WaitForSeconds(randomDelay + movementProps.moveTime);
                        setMoveDelay = true;
                    }
                }
                else
                {
                    canMove = false;
                }
                yield return null;
            }
        }
    }
    #endregion

    #region RotationHandler
    public void InitializeRotate()
    {
        canRorate = true;
        setRotationDelay = true;
        StartCoroutine(RotationDelayManager());
    }
    public void RotationHandler()
    {
        if (rotationProps.rotationType == RotationProperties.RotationType.angle)
        {
            iTween.RotateTo(gameObject, iTween.Hash(xAxis, rotationProps.rotationAngle.x, yAxis, rotationProps.rotationAngle.y,
                zAxis, rotationProps.rotationAngle.z, tweenTime, rotationProps.rotationTime, isLocal, rotationProps.isLocal,
                ignoreTimeScale, rotationProps.ignoreTimeScale, easeType, rotationProps.easeType.ToString(),
                loopType, rotationProps.loopType.ToString()));
        }
        else if (rotationProps.rotationType == RotationProperties.RotationType.rotations)
        {
            iTween.RotateTo(gameObject, iTween.Hash(xAxis, 720 * rotationProps.rotationAngle.x, yAxis, 720 * rotationProps.rotationAngle.y,
            zAxis, 720 * rotationProps.rotationAngle.z, tweenTime, rotationProps.rotationTime, isLocal, rotationProps.isLocal,
            ignoreTimeScale, rotationProps.ignoreTimeScale, easeType, rotationProps.easeType.ToString(),
            loopType, rotationProps.loopType.ToString()));
        }
    }
    public void RotateTo(Vector3 targetRotation, float rotationTime, bool rotateLocal = true, bool unscaleTime = false, iTween.EaseType _easeType = iTween.EaseType.linear, iTween.LoopType _loopType = iTween.LoopType.none)
    {
        iTween.RotateTo(gameObject, iTween.Hash(xAxis, targetRotation.x, yAxis, targetRotation.y,
        zAxis, targetRotation.z, tweenTime, rotationTime, isLocal, rotateLocal,
        ignoreTimeScale, unscaleTime, easeType, _easeType, loopType, _loopType));
    }
    IEnumerator RotationDelayManager()
    {
        if (rotationProps.startDelay)
        {
            if (rotationProps.ignoreTimeScale)
            {
                yield return new WaitForSecondsRealtime(rotationProps.startRotationDelay);
            }
            else
            {
                yield return new WaitForSeconds(rotationProps.startRotationDelay);
            }
        }
        RotationHandler();
        if ((rotationProps.fixRotationDelay || rotationProps.randomRotationDelay) && !rotationProps.ignoreTimeScale)
        {
            while (canRorate)
            {
                if (rotationProps.fixRotationDelay && rotationProps.fixRotationDelayValue > 0)
                {
                    yield return new WaitForSeconds(0.05f);
                    rotateTween.delay = rotationProps.fixRotationDelayValue;
                    canRorate = false;
                }
                else if (rotationProps.randomRotationDelay && rotationProps.minRotationDelayValue >= 0 && rotationProps.maxRotationDelayValue > 0)
                {
                    if (setRotationDelay && rotateTween)
                    {
                        setRotationDelay = false;
                        float randomDelay = Random.Range(rotationProps.minRotationDelayValue, rotationProps.maxRotationDelayValue + 0.01f);
                        rotateTween.delay = randomDelay;
                        yield return new WaitForSeconds(randomDelay + rotationProps.rotationTime);
                        setRotationDelay = true;
                    }
                }
                else
                {
                    canRorate = false;
                }
                yield return null;
            }
        }
    }
    #endregion

    #region ScaleHandler
    public void InitializeScale()
    {
        canScale = true;
        setScaleDelay = true;
        StartCoroutine(ScaleDelayManager());
    }
    public void ScaleHandler()
    {
        if (scaleProps.fromTo)
        {
            transform.localScale = scaleProps.scaleFrom;
        }
        iTween.ScaleTo(gameObject, iTween.Hash(xAxis, scaleProps.scaleTo.x, yAxis, scaleProps.scaleTo.y,
            zAxis, scaleProps.scaleTo.z, tweenTime, scaleProps.scaleTime, isLocal, scaleProps.isLocal,
            ignoreTimeScale, scaleProps.ignoreTimeScale, easeType, scaleProps.easeType.ToString(),
            loopType, scaleProps.loopType.ToString()));
    }
    public void ScaleTo(Vector3 targetScale, float scaleTime, bool scaleLocal = true, bool unscaleTime = false, iTween.EaseType _easeType = iTween.EaseType.linear, iTween.LoopType _loopType = iTween.LoopType.none)
    {
        iTween.ScaleTo(gameObject, iTween.Hash(xAxis, targetScale.x, yAxis, targetScale.y,
        zAxis, targetScale.z, tweenTime, scaleTime, isLocal, scaleLocal,
        ignoreTimeScale, unscaleTime, easeType, _easeType, loopType, _loopType));
    }
    IEnumerator ScaleDelayManager()
    {
        if (scaleProps.startDelay)
        {
            if (scaleProps.ignoreTimeScale)
            {
                yield return new WaitForSecondsRealtime(scaleProps.startScaleDelay);
            }
            else
            {
                yield return new WaitForSeconds(scaleProps.startScaleDelay);
            }
        }
        ScaleHandler();
        if ((scaleProps.fixScaleDelay || scaleProps.randomScaleDelay) && !scaleProps.ignoreTimeScale)
        {
            while (canScale)
            {
                if (scaleProps.fixScaleDelay && scaleProps.fixScaleDelayValue > 0)
                {
                    yield return new WaitForSeconds(0.05f);
                    scaleTween.delay = scaleProps.fixScaleDelayValue;
                    canScale = false;
                }
                else if (scaleProps.randomScaleDelay && scaleProps.minScaleDelayValue >= 0 && scaleProps.maxScaleDelayValue > 0)
                {
                    if (setScaleDelay && scaleTween)
                    {
                        setScaleDelay = false;
                        float randomDelay = Random.Range(scaleProps.minScaleDelayValue, scaleProps.maxScaleDelayValue + 0.01f);
                        scaleTween.delay = randomDelay;
                        yield return new WaitForSeconds(randomDelay + scaleProps.scaleTime);
                        setScaleDelay = true;
                    }
                }
                else
                {
                    canScale = false;
                }
                yield return null;
            }
        }
    }
    #endregion

    #region DestroyTweens
    private void DestroyTweens(string tweenType)
    {
        var iTweens = GetComponents<iTween>();
        for (int i = 0; i < iTweens.Length; i++)
        {
            if (iTweens[i].type == "move")
            {
                Destroy(iTweens[i]);
            }
            else if (iTweens[i].type == "rotate")
            {
                Destroy(iTweens[i]);
            }
            else if (iTweens[i].type == "scale")
            {
                Destroy(iTweens[i]);
            }
        }
    }
    #endregion
}

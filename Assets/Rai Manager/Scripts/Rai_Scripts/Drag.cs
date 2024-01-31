using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Drag : MonoBehaviour
{
    public bool isCanvasObject;
    public AudioSource MouseDownSFX;
    public GameObject MouseDownIndicator, MouseUpIndicator;
    public ParticleSystem triggerParticles;
    public int downSortingOrder, upSortinggOrder;
    public int totalTriggers;
    private Vector2 InitialPosition;
    private Vector2 MousePosition;
    private Vector3 screenPoint;
    private Vector3 offset;
    private float deltaX, deltaY;
    private ScalePingPong pingPong;
    private bool isPosAssigned, restPos;
    private BoxCollider2D boxCollider;
    private List<BoxCollider2D> boxColliders = new List<BoxCollider2D>();
    private Animator m_Animator;
    private int didTrigger;
    private bool inTrigger = false;
    // Start is called before the first frame update
    void Start()
    {
        restPos = true;
        boxCollider = GetComponent<BoxCollider2D>();
        pingPong = GetComponent<ScalePingPong>();
        m_Animator = GetComponentInChildren<Animator>();
        if (m_Animator)
            m_Animator.enabled = false;
        var _BoxColliders2D = FindObjectsOfType<BoxCollider2D>();
        for(int i = 0; i < _BoxColliders2D.Length; i++)
        {
            boxColliders.Add(_BoxColliders2D[i]);
        }
        if (MouseUpIndicator)
        {
            MouseUpIndicator.SetActive(true);
        }
    }
    private void ColliderManager(bool isTrue)
    {
        for(int i = 0; i < boxColliders.Count; i++)
        {
            if(boxColliders[i] != boxCollider)
            {
                boxColliders[i].enabled = isTrue;
            }
        }
    }

    void OnMouseDown()
    {
        var _Renderers = GetComponentsInChildren<Renderer>();
        for(int i = 0; i < _Renderers.Length; i++)
        {
            _Renderers[i].sortingOrder = downSortingOrder;
        }
        if (pingPong) pingPong.enabled = false;
        if (MouseDownIndicator) MouseDownIndicator.SetActive(true);
        if (MouseUpIndicator) MouseUpIndicator.SetActive(false);
        if (m_Animator) m_Animator.enabled = true;
        if (triggerParticles) triggerParticles.Play();
        if (MouseDownSFX) MouseDownSFX.Play();
        if (!isPosAssigned)
        {
            isPosAssigned = true;
            InitialPosition = transform.localPosition;
        }
        if (isCanvasObject)
        {
            screenPoint = Camera.main.WorldToScreenPoint(Input.mousePosition); // I removed this line to prevent centring 
            offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
        else
        {
            deltaX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x - transform.localPosition.x;
            deltaY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - transform.localPosition.y;
        }
    }

    void OnMouseDrag()
    {
        if (isCanvasObject)
        {
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
        }
        else
        {
            MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.localPosition = new Vector2(MousePosition.x - deltaX, MousePosition.y - deltaY);
        } 
    }

    void OnMouseUp()
    {
        var _Renderers = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < _Renderers.Length; i++)
        {
            _Renderers[i].sortingOrder = upSortinggOrder;
        }
        if (pingPong) pingPong.enabled = true;
        if (MouseDownIndicator) MouseDownIndicator.SetActive(false);
        if (m_Animator) m_Animator.enabled = false;
        if (triggerParticles) triggerParticles.Stop();
        if (restPos)
        {
            transform.localPosition = InitialPosition;
            if (MouseUpIndicator)
            {
                MouseUpIndicator.SetActive(true);
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {


    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        inTrigger = true;
        if (MouseDownIndicator) MouseDownIndicator.SetActive(false);
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        inTrigger = false;
        if (MouseDownIndicator) MouseDownIndicator.SetActive(true);
    }

    private void OnEnable()
    {
        didTrigger = 0;
        if (boxCollider) boxCollider.enabled = true;
        if (m_Animator) m_Animator.enabled = false;
        if (pingPong) pingPong.enabled = true;
        if (MouseDownIndicator)
        {
            if (MouseDownIndicator.GetComponentInChildren<Image>())
            {
                MouseDownIndicator.GetComponentInChildren<Image>().enabled = true;
            }
        }
        if (MouseUpIndicator)
        {
            if (MouseUpIndicator.GetComponentInChildren<Image>())
            {
                MouseUpIndicator.GetComponentInChildren<Image>().enabled = true;
            }
        }
    }
    private void OnDisable()
    {
        DisableObjects();
        if (MouseDownIndicator)
        {
            if (MouseDownIndicator.GetComponentInChildren<Image>())
            {
                MouseDownIndicator.GetComponentInChildren<Image>().enabled = false;
            }
        }
        if (MouseUpIndicator)
        {
            if (MouseUpIndicator.GetComponentInChildren<Image>())
            {
                MouseUpIndicator.GetComponentInChildren<Image>().enabled = false;
            }
        }
        if (isPosAssigned) transform.localPosition = InitialPosition;
    }

    private void DisableObjects()
    {
        if (boxCollider) boxCollider.enabled = false;
        if (MouseDownIndicator) MouseDownIndicator.SetActive(false);
        if (MouseUpIndicator) MouseUpIndicator.SetActive(false);
    }
}

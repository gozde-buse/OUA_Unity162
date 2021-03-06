using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectButton : MonoBehaviour
{
    public enum Name { Strawberry, Apple, Orange, Cherry, Banana, Broccoli, Pepper, Tomato, Carrot, Onion }

    [SerializeField] private Name nameOfButton;
    [SerializeField] private GameObject objectToDragPrefab;
    [SerializeField] private Sprite dragPlaceholderSprite;
    [SerializeField] private GameObject correctObject;

    //private float timeofTouch = 0;
    private Vector3 initialTouch;
    private bool touching = false;
    private bool dragging = false;

    private Sprite normalSprite;

    void Awake()
    {
        normalSprite = GetComponent<Image>().sprite;
        initialTouch.x = Mathf.Infinity;
    }

    void Update()
    {
        if (touching)
        {
            Vector3 newTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newTouch.z = 0;

            if (Vector3.Distance(initialTouch, newTouch) >= .6f)
            {
                StartObjectDragging();
            }

            //timeofTouch += Time.deltaTime;
        }

        /*if (timeofTouch > .5f)
            StartObjectDragging();*/
    }

    public void Touch()
    {
#if UNITY_ANDROID

        if(Input.touchCount > 1)   
            return;

#endif
        if (!dragging)
            AudioController.instance.Play("Info", nameOfButton.ToString());

        initialTouch.x = Mathf.Infinity;
        touching = false;
    }

    public void StartTouching()
    {
#if UNITY_ANDROID

        if(Input.touchCount > 1)   
            return;

#endif

        initialTouch = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        initialTouch.z = 0;
        touching = true;
    }

    public void StopTouching()
    {
#if UNITY_ANDROID

        if(Input.touchCount > 1)   
            return;

#endif

        initialTouch.x = Mathf.Infinity;
        touching = false;
    }

    public void StartObjectDragging()
    {
#if UNITY_ANDROID

        if(Input.touchCount > 1)   
            return;

#endif

        initialTouch.x = Mathf.Infinity;
        touching = false;
        dragging = true;

        GetComponent<Image>().sprite = dragPlaceholderSprite;

        foreach (Transform face in transform)
            face.gameObject.SetActive(false);

#if UNITY_EDITOR

        Vector3 sPanposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

#elif UNITY_ANDROID

        Vector3 sPanposition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

#endif

        sPanposition.z = 0;
        GameObject objectToDrag = Instantiate(objectToDragPrefab, sPanposition, objectToDragPrefab.transform.rotation);
        objectToDrag.GetComponent<Object>().SetButton(this);
    }

    public void LoadBack()
    {
        dragging = false;
        LevelController.instance.DecreaseStarCount();
        LevelController.instance.ChangeSide(0);

        GetComponent<Image>().sprite = normalSprite;

        foreach (Transform face in transform)
            face.gameObject.SetActive(true);
    }

    public void CorrectPlacement()
    {
        dragging = false;
        LevelController.instance.ChangeSide(0);
        StartCoroutine(CorrectPlacementAnimation());
    }

    private IEnumerator CorrectPlacementAnimation()
    {
        float animationTime = .3f;
        float elapsedTime = 0;

        Vector3 initialScale = Vector3.zero;
        Vector3 endScale = correctObject.transform.localScale;

        correctObject.transform.localScale = initialScale;
        correctObject.SetActive(true);

        while (elapsedTime <= animationTime)
        {
            correctObject.transform.localScale = Vector3.Lerp(initialScale, endScale, elapsedTime / animationTime);
            elapsedTime += Time.fixedDeltaTime;

            yield return new WaitForFixedUpdate();
        }

        correctObject.transform.localScale = endScale;
        LevelController.instance.IncreaseStarCount();
        LevelController.instance.DecreaseButtonCount();

        Destroy(gameObject);
    }

    public Vector3 GetCorrectObjectPosition()
    {
        return correctObject.transform.position;
    }
}

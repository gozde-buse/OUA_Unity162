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

    private float timeofTouch = 0;
    private bool touching = false;
    private bool dragging = false;

    private Sprite normalSprite;

    void Awake()
    {
        normalSprite = GetComponent<Image>().sprite;
    }

    void Update()
    {
        if (touching)
            timeofTouch += Time.deltaTime;

        if (timeofTouch > .5f)
            StartObjectDragging();
    }

    public void Touch()
    {
        if(!dragging)
            AudioController.instance.Play("Info", nameOfButton.ToString());

        timeofTouch = 0;
        touching = false;
    }

    public void StartTouching()
    {
        touching = true;
    }

    public void StopTouching()
    {
        timeofTouch = 0;
        touching = false;
    }

    public void StartObjectDragging()
    {
        timeofTouch = 0;
        touching = false;
        dragging = true;

        GetComponent<Image>().sprite = dragPlaceholderSprite;

        foreach (Transform face in transform)
            face.gameObject.SetActive(false);

        Vector3 sPanposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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

        foreach(Transform face in transform)
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

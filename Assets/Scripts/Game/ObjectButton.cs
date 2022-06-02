using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectButton : MonoBehaviour
{
    [SerializeField] private GameObject objectToDragPrefab;
    [SerializeField] private Sprite dragPlaceholderSprite;
    [SerializeField] private GameObject correctObject;

    private float timeofTouch = 0;
    private bool touching = false;

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
        //Ses Çal
        Debug.Log("Ses çalýyorum.");

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

        GetComponent<Image>().sprite = dragPlaceholderSprite;

        Vector3 sPanposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        sPanposition.z = 0;
        GameObject objectToDrag = Instantiate(objectToDragPrefab, sPanposition, objectToDragPrefab.transform.rotation);
        objectToDrag.GetComponent<Object>().SetButton(this);
    }

    public void LoadBack()
    {
        GetComponent<Image>().sprite = normalSprite;
    }

    public void CorrectPlacement()
    {
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

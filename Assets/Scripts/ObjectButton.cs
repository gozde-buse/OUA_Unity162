using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectButton : MonoBehaviour
{
    [SerializeField] private GameObject objectToDragPrefab;

    private float timeofTouch = 0;
    private bool touching = false;

    void Update()
    {
        if (touching)
            timeofTouch += Time.deltaTime;

        if (timeofTouch > 1f)
            StartObjectDragging();
    }

    public void Touch()
    {
        //Ses �al
        Debug.Log("Ses �al�yorum.");

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

        //Sprite de�i�ecek

        Vector3 sPanposition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        sPanposition.z = 0;
        GameObject objectToDrag = Instantiate(objectToDragPrefab, sPanposition, objectToDragPrefab.transform.rotation);
        objectToDrag.GetComponent<Object>().SetButton(gameObject);
    }

    public void LoadBack()
    {
        //Sprite geri y�kleecek
    }

    public void CorrectPlacement()
    {
        Destroy(gameObject);
    }
}

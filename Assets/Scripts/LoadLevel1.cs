using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel1 : MonoBehaviour
{
    [SerializeField] private GameObject bottomUIPanel;
    [SerializeField] private GameObject topUIPanel;

    public static float xLimit;
    public static float yLimit;

    void Start()
    {
        xLimit = Screen.width / 2;
        yLimit = bottomUIPanel.GetComponent<RectTransform>().sizeDelta.y;
    }
}

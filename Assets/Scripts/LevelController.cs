using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject bottomUIPanel;
    [SerializeField] private GameObject inSide;

    public float xLimit;
    public float yLimit;

    public static LevelController instance;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }


    void Start()
    {
        xLimit = Screen.width / 2;
        yLimit = bottomUIPanel.GetComponent<RectTransform>().sizeDelta.y;
    }

    public void ChangeSide(int side)
    {
        switch (side)
        {
            case 1:
                inSide.SetActive(true);
                inSide.transform.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);

                break;

            case -1:
                inSide.SetActive(true);
                inSide.transform.GetComponent<RectTransform>().localScale = new Vector3(-1, 1, 1);

                break;

            case 0:
            default:
                inSide.SetActive(false);

                break;

        }
    }
}

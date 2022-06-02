using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject inSide;
    [SerializeField] private GameObject objectButtonParent;

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
        yLimit = objectButtonParent.GetComponent<RectTransform>().sizeDelta.y;

        ShuffleObjectButtons();
    }

    private void ShuffleObjectButtons()
    {
        List<GameObject> shuffled = new List<GameObject>();
        List<GameObject> willshuffle = new List<GameObject>();

        foreach (Transform objectButton in objectButtonParent.transform)
            willshuffle.Add(objectButton.gameObject);

        while(willshuffle.Count > 0)
        {
            int id = Random.Range(0, willshuffle.Count);
            shuffled.Add(willshuffle[id]);
            willshuffle.RemoveAt(id);
        }

        for(int i = 0; i < shuffled.Count; i++)
        {
            shuffled[i].transform.SetSiblingIndex(i);
        }
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

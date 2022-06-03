using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] private GameObject inSideFruit;
    [SerializeField] private GameObject inSideVegetable;
    [SerializeField] private GameObject objectButtonParent;
    [SerializeField] private GameObject starParent;
    [SerializeField] private Sprite starSprite;
    [SerializeField] private Sprite starGraySprite;
    [SerializeField] private Sprite happyFace;
    [SerializeField] private Sprite sadFace;
    [SerializeField] private GameObject endGamePanel;

    public float xLimit;
    public float yLimit;

    public static LevelController instance;

    private int totalButton;
    private int startCount;
    private int wrongCount;

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

        for(int i = 4; i < objectButtonParent.transform.childCount; i++)
        {
            objectButtonParent.transform.GetChild(i).gameObject.SetActive(false);
        }

        startCount = 0;
        wrongCount = 0;
        totalButton = objectButtonParent.transform.childCount;
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

    public void DecreaseButtonCount()
    {
        totalButton--;

        if(totalButton >= 4)
        {
            objectButtonParent.transform.GetChild(4).gameObject.SetActive(true);
        }

        if(totalButton == 0)
        {
            endGamePanel.SetActive(true);
            PlayerStats.SetLevelStar(0, startCount);
            SaveManagement.Save();
        }
    }

    public void IncreaseStarCount()
    {
        if (startCount < 3)
        {
            starParent.transform.GetChild(startCount).GetComponent<Image>().sprite = starSprite;
            Transform face = starParent.transform.GetChild(startCount).GetChild(0);
            face.GetComponent<Image>().sprite = happyFace;
            face.GetComponent<Image>().color = Color.white;
            startCount++;
        }

        if(wrongCount > 0)
            wrongCount--;
    }

    public void DecreaseStarCount()
    {
        wrongCount++;

        if(wrongCount > 4)
        {
            wrongCount = 0;

            if(startCount > 0)
            {
                startCount--;
                starParent.transform.GetChild(startCount).GetComponent<Image>().sprite = starGraySprite;
                Transform face = starParent.transform.GetChild(startCount).GetChild(0);
                face.GetComponent<Image>().sprite = sadFace;
                face.GetComponent<Image>().color = new Color32(118, 118, 118, 150);
            }
        }
    }

    public void ChangeSide(int side)
    {
        switch (side)
        {
            case 1:
                inSideVegetable.SetActive(true);
                inSideFruit.SetActive(false);

                break;

            case -1:
                inSideVegetable.SetActive(false);
                inSideFruit.SetActive(true);

                break;

            case 0:
            default:
                inSideVegetable.SetActive(false);
                inSideFruit.SetActive(false);

                break;

        }
    }

    public void BackToLevels()
    {
        SceneController.LoadScene("LevelSelection");
    }
}

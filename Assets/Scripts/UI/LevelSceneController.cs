using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSceneController : MonoBehaviour
{
    [SerializeField] private GameObject levelList;
    [SerializeField] private Sprite starSprite;

    void Awake()
    {
        for (int i = 0; i < levelList.transform.childCount; i++)
        {
            Transform levelButton = levelList.transform.GetChild(i);

            levelButton.GetComponentInChildren<Text>().text = i.ToString();
            Transform starPanel = levelButton.transform.GetChild(0);

            for(int j = 0; j < PlayerStats.levelStars[i]; j++)
            {
                starPanel.GetChild(j).GetComponent<Image>().sprite = starSprite;
            }
        }
    }

    public void LoadLevel(int levelId)
    {
        SceneController.LoadScene("Level" + levelId.ToString());
    }
}
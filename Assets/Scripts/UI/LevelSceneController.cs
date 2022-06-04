using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSceneController : MonoBehaviour
{
    [SerializeField] private GameObject levelList;
    [SerializeField] private Sprite starSprite;
    [SerializeField] private Sprite happyFace;

    void Awake()
    {
        for (int i = 0; i < levelList.transform.childCount; i++)
        {
            Transform levelButton = levelList.transform.GetChild(i);

            levelButton.GetComponentInChildren<Text>().text = (i + 1).ToString();
            Transform starPanel = levelButton.transform.GetChild(0);

            for(int j = 0; j < PlayerStats.levelStars[i]; j++)
            {
                starPanel.GetChild(j).GetComponent<Image>().sprite = starSprite;
                Transform face = starPanel.GetChild(j).GetChild(0);
                face.GetComponent<Image>().sprite = happyFace;
                face.GetComponent<Image>().color = Color.white;
            }
        }

        AudioController.instance.Play("Bgm", "Menu");
    }

    public void LoadLevel(int levelId)
    {
        AudioController.instance.Click();
        SceneController.LoadScene("Level" + levelId.ToString());
    }

    public void Quit()
    {
        AudioController.instance.Click();
        Application.Quit();
    }
}

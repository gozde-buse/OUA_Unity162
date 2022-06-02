using UnityEngine;

public class Init : MonoBehaviour
{
    void Awake()
    {
        if(!SaveManagement.ControlData())
            SaveManagement.CreateData();

        SaveManagement.Load();
        SceneController.LoadScene("LevelSelection");
    }
}

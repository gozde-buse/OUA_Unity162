using UnityEngine;

public class Init : MonoBehaviour
{
    void Awake()
    {
        /*SaveManagement.DeleteData();

        return;*/

        if (!SaveManagement.ControlData())
            SaveManagement.CreateData();

        SaveManagement.Load();
        SceneController.LoadScene("LevelSelection");
    }
}

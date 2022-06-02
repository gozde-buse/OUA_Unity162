using UnityEngine.SceneManagement;

public class SceneController
{
    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
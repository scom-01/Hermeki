using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerController : MonoBehaviour
{
    public void LoadAsyncString(string SceneName)
    {
        if (SceneName == "")
            return;

        SceneManager.LoadSceneAsync(SceneName);
    }
    public void LoadAsyncInt(int SceneIdx)
    {
        SceneManager.LoadSceneAsync(SceneIdx);
    }
}

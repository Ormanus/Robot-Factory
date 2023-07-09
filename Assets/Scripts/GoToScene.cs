using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToScene : MonoBehaviour
{
    public string scene;

    public void EnterScene()
    {
        SceneManager.LoadScene(scene);
    }
}
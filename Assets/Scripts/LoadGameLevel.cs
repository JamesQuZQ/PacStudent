using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGameLevel : MonoBehaviour
{
    public void NextScene()
    {
        SceneManager.LoadScene("GameLevelScene");
    }
}
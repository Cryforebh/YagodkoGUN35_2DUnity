using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager
{
    public void FinishGame()
    {
        //Time.timeScale = 0;
        Debug.Log("End Game!");
        TestLoadSceneNext();
    }

    private void TestLoadSceneNext()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}

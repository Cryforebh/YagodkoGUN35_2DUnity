using UnityEngine;

public class GameManager
{
    public void FinishGame()
    {
        Time.timeScale = 0;
        Debug.Log("End Game!");
    }
}

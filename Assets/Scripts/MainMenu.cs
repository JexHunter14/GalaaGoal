using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   public void PlayGame()
    {
        SceneManager.LoadSceneAsync(2);
    }
    public void PlayOnlineGame()
    {
      SceneManager.LoadSceneAsync(3);
    }
    public void LoadLeaderBoard()
    {
      SceneManager.LoadSceneAsync(4);
    }
}

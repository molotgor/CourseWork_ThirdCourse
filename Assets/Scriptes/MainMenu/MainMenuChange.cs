using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuChange : MonoBehaviour
{
    public void ChangeTo(string sceneName)
    {
        //Change scene
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}

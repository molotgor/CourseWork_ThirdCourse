using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinScreen : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] protected TMPro.TMP_Text BlueMessage;
    [SerializeField] protected TMPro.TMP_Text RedMessage;
    [SerializeField] protected Button RestartBtn;
    [SerializeField] protected Button MainMenuBtn;

    void Start()
    {
        //Hide win screen
        RestartBtn.interactable = false;
        MainMenuBtn.interactable = false;
        GetComponent<CanvasGroup>().alpha = 0f;
    }

    virtual public void ShowScreen(int color)
    {
        //Show win screen
    }

    public void Restart()
    {
        //Restart current Scene
        print("Restart");
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void MainMenu()
    {
        //Go back to main menu
        print("MainMenu");
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}

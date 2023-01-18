using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour

{
    public void LoginPassMethod()
    {
        SceneManager.LoadScene(5);
    }

    public void BacktoMainMenu()
    {
        SceneManager.LoadScene(6);
    }

    public void DigitalCodeMethod()
    {
        SceneManager.LoadScene(3);
    }

    public void CAPTCHASwitcher()
    {
        SceneManager.LoadScene(1);
    }

    public void SoundUsage()
    {
        SceneManager.LoadScene(2);
    }

    public void IngameInteraction()
    {
        SceneManager.LoadScene(4);
    }

    public void ClosePopUp()
    {
        SceneManager.LoadPreviousScene();
    }
    public void QRCode()
    {
        SceneManager.LoadScene(0);
    }
}

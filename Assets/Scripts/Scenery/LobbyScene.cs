using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LobbyScene : MonoBehaviour
{

    public static LobbyScene Instance { set; get; }

    private void Start()
    {
        Instance = this;
    }

    public void OnClickCreateAccount()
    {
        DisableInputs();

        string username = GameObject.Find("CreateUsername").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("CreatePassword").GetComponent<TMP_InputField>().text;
        string email = GameObject.Find("CreateEmail").GetComponent<TMP_InputField>().text;

        Client1.Instance.SendCreateAccount(username, password, email);
    }
    public void OnClickLoginRequest()
    {
        DisableInputs();

        string usernameOrEmail = GameObject.Find("LoginUsernameEmail").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("LoginPassword").GetComponent<TMP_InputField>().text;

        Client1.Instance.SendLoginRequest(usernameOrEmail, password);
    }

    public void OnSliderValueAdjust()
    {
       // DisableInputs();

        float MinRange = GameObject.Find("MinRangeSlider").GetComponent<Slider>().value; ;
        float MaxRange = GameObject.Find("MaxRangeSlider").GetComponent<Slider>().value; ;
        float Feather = GameObject.Find("OpacitySlider").GetComponent<Slider>().value; ;

        Client1.Instance.SendSliderValue(MinRange, MaxRange, Feather);
    }

   /* public void OnClickCatSound()
    {
        //DisableInputs();

        string CatSound = GameObject.Find("LoginUsernameEmail").GetComponent<TMP_InputField>().text;
        string password = GameObject.Find("LoginPassword").GetComponent<TMP_InputField>().text;

        Client.Instance.SendLoginRequest(usernameOrEmail, password);
    } */

    public void ChangeWelcomeMessage(string msg)
    {
        GameObject.Find("WelcomeMessageText").GetComponent<TextMeshProUGUI>().text = msg;
    }
    public void ChangeAuthenticationMessage(string msg)
    {
        GameObject.Find("AuthenticationMessageText").GetComponent<TextMeshProUGUI>().text = msg;
    }

    public void EnableInputs()
    {
        GameObject.Find("Canvas").GetComponent<CanvasGroup>().interactable = true;

    }
    public void DisableInputs()
    {
        GameObject.Find("Canvas").GetComponent<CanvasGroup>().interactable = false;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using RiptideNetworking;

public class SwitchToCAPTCHA : MonoBehaviour
{
    [MessageHandler((ushort)ServerToClientId.SresponsetoC)]
    private static void SwitchScene(ushort fromServerId, Message message)
    {
        string messageText = message.GetString();
        Debug.Log(messageText);
        if (messageText == "load_CAPTCHA_scene")
        {
            SceneManager.LoadScene(1);
        }
    }
}

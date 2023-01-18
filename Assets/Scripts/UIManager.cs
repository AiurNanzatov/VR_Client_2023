using RiptideNetworking;
using RiptideNetworking.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;

    public static UIManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            } else if (_singleton != value)
            {
                Debug.Log($"{nameof(UIManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    [Header("Connect")]
    [SerializeField] private GameObject connectUI;
    [SerializeField] private TMP_InputField ipField;

    private void Awake()
    {
        Singleton = this;
    }

    public void SendButtonDownMessage()
    {
        Debug.Log("Button down");
        if (NetworkManager.Singleton.Client.IsConnected == false) return;
        
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.name);
        message.AddString("Button down");
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendButtonUpMessage()
    {
        if (NetworkManager.Singleton.Client.IsConnected == false) return;

        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.name);
        message.AddString("Button up");
        NetworkManager.Singleton.Client.Send(message);
    }

    public void ConnectServer() {
        NetworkManager.Singleton.Connect(ipField.text);
    }
}

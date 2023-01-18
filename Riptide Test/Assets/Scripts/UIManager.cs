using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _singleton;
    private static bool connected = false;

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

    private void Awake()
    {
        Singleton = this;
    }

    public void SendButtonDownMessage()
    {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.name);
        message.AddString("Button down");
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendButtonUpMessage()
    {
        // connect to the server the first time the button is pressed
        if (!connected)
        {
            NetworkManager.Singleton.Connect();
            connected = true;
        } else {
            Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.name);
            message.AddString("Button up");
            NetworkManager.Singleton.Client.Send(message);
        }
    }
}

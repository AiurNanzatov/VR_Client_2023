using RiptideNetworking;
using RiptideNetworking.Utils;
using UnityEngine;
using UnityEngine.UI;
using System;


public enum ServerToClientId : ushort
{
    SresponsetoC = 1,
}

public enum ClientToServerId : ushort
{
    name = 1,
}

public class NetworkManager : MonoBehaviour
{
    private static NetworkManager _singleton;

    public static NetworkManager Singleton
    {
        get => _singleton;
        private set
        {
            if (_singleton == null)
            {
                _singleton = value;
            } else if (_singleton != value)
            {
                Debug.Log($"{nameof(NetworkManager)} instance already exists, destroying duplicate!");
                Destroy(value);
            }
        }
    }

    public Client Client {get; private set;}

    [SerializeField] private ushort port;
    [SerializeField] private Image ConnectButton;
    [SerializeField] private Image QRButton;

    private void Awake()
    {
        Singleton = this;
    }

    private void Start()
    {
        RiptideLogger.Initialize(Debug.Log, Debug.Log, Debug.LogWarning, Debug.LogError, false);

        Client = new Client();

        Client.Connected += DidConnect;
        Client.ConnectionFailed += FailedToConnect;
        Client.Disconnected += DidDisconnect;
    }

    private void FixedUpdate()
    {
        Client.Tick();
    }

    private void OnApplicationQuit()
    {
        Client.Disconnect();
    }

    public void Connect(string ip)
    {
        Client.Connect($"{ip}:{port}");
        Debug.Log("connecting...");
    }

    private void DidConnect(object sender, EventArgs e)
    {
        Debug.Log("connected");
        // UIManager.Singleton.SendButtonUpMessage();
        ConnectButton.gameObject.SetActive(false);
        QRButton.gameObject.SetActive(true);
    }

    private void FailedToConnect(object sender, EventArgs e)
    {
        Debug.Log("Failed to connect");
        ConnectButton.gameObject.SetActive(true);
        QRButton.gameObject.SetActive(false);
    }

    private void DidDisconnect(object sender, EventArgs e)
    {
        Debug.Log("Disconnected");
        ConnectButton.gameObject.SetActive(true);
        QRButton.gameObject.SetActive(false);
    }
}

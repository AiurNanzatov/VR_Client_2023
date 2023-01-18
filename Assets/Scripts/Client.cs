using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Networking;

public class Client1 : MonoBehaviour
{
    public static Client1 Instance { private set; get; }
    private const int MAX_USER = 100;
    private const int PORT = 26000;
    private const int WEB_PORT = 26001;
    private const int BYTE_SIZE = 1024;
    private const string SERVER_IP = "127.0.0.1";
    // 192.168.137.1


    private byte reliableChannel;
    private int connectionId;
    private int hostId;
    private byte error;

    private bool isStarted;

    #region Monobehaviour
    private void Start()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
        Init();
    }
    private void Update()
    {
        UpdateMessagePump();
    }
    #endregion
    public void Init()
    {
        NetworkTransport.Init();

        ConnectionConfig cc = new ConnectionConfig();
        reliableChannel = cc.AddChannel(QosType.Reliable);

        HostTopology topo = new HostTopology(cc, MAX_USER);

        // Client only
        hostId = NetworkTransport.AddHost(topo, 0);
#if UNITY_WEBGL && !UNITY_EDITOR
        // Web Client
        connectionId = NetworkTransport.Connect(hostId, SERVER_IP, WEB_PORT, 0, out error);
        Debug.Log("Connecting from Web");
#else
        // Standalone Client
        connectionId = NetworkTransport.Connect(hostId, SERVER_IP, PORT, 0, out error);
        Debug.Log("Connecting from standalone");
#endif

        Debug.Log(string.Format("Attempting to connect on {0}...", SERVER_IP));
        isStarted = true;
    }
    public void Shutdown()
    {
        isStarted = false;
        NetworkTransport.Shutdown();
    }

    public void UpdateMessagePump()
    {
        if (!isStarted)
            return;

        int recHostId;      //web or standalone
        int connectionId;   //Who sends this
        int channelId;      //Which channel is he sending the message from

        byte[] recBuffer = new byte[BYTE_SIZE];
        int dataSize;

        NetworkEventType type = NetworkTransport.Receive(out recHostId, out connectionId, out channelId, recBuffer, BYTE_SIZE, out dataSize, out error);
        switch (type)
        {
            case NetworkEventType.Nothing:
                break;

            case NetworkEventType.ConnectEvent:
                Debug.Log("We have connected to the server");
                break;

            case NetworkEventType.DisconnectEvent:
                Debug.Log("We have been disconnected");
                break;

            case NetworkEventType.DataEvent:
                BinaryFormatter formatter = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(recBuffer);
                NetMsg msg = (NetMsg)formatter.Deserialize(ms);

                OnData(connectionId, channelId, recHostId, msg);
                break;

            default:
            case NetworkEventType.BroadcastEvent:
                Debug.Log("Unexpected network event type");
                break;
        }
    }

    #region OnData
    private void OnData(int cnnId, int channelId, int recHostId, NetMsg msg)
    {
        switch (msg.OP)
        {
            case NetOP.None:
                Debug.Log("Unexpected NetOP");
                break;

            case NetOP.OnCreateAccount:
                OnCreateAccount((Net_OnCreateAccount)msg);
                break;

            case NetOP.OnLoginRequest:
                OnLoginRequest((Net_OnLoginRequest)msg);
                break;

        }
    }

    private void OnCreateAccount(Net_OnCreateAccount oca)
    {
        LobbyScene.Instance.EnableInputs();
        LobbyScene.Instance.ChangeAuthenticationMessage(oca.Information);

    }

    private void OnLoginRequest(Net_OnLoginRequest olr)
    {
        LobbyScene.Instance.ChangeAuthenticationMessage(olr.Information);

        if(olr.Success != 1)
        {
            LobbyScene.Instance.EnableInputs();
        }
        else
        {
            

        }
    }
    #endregion

    #region Send
    public void SendServer(NetMsg msg)
    {
        byte[] buffer = new byte[BYTE_SIZE];

        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream ms = new MemoryStream(buffer);
        formatter.Serialize(ms, msg);

        NetworkTransport.Send(hostId, connectionId, reliableChannel, buffer, BYTE_SIZE, out error);
    }
    public void SendCreateAccount(string username, string password, string email)
    {
        Net_CreateAccount ca = new Net_CreateAccount();

        ca.Username = username;
        ca.Password = password;
        ca.Email = email;

        SendServer(ca);
    }
    public void SendLoginRequest(string usernameOrEmail, string password)
    {
        Net_LoginRequest lr = new Net_LoginRequest();

        lr.UsernameOrEmail = usernameOrEmail;
        lr.Password = password;

        SendServer(lr);
    }

    public void SendSliderValue(float MinRange, float MaxRange, float Feather)
    {
        Net_SliderValue sv = new Net_SliderValue();

        sv.MinRange = MinRange;
        sv.MaxRange = MaxRange;
        sv.Feather = Feather;

        SendServer(sv);
    }
    #endregion


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
using RiptideNetworking;



public class GestureDetection : MonoBehaviour {
    public Vector2 pos;
    public float borderLimitDistance; // 250
    public float borderLeft;
    public float borderRight;
    public string movingDirection = "";
    public int movingCount = 0;
    public float movingTimeInterval; // 0.5f

    private Vector2 lastPos;
    private bool moving = false;
    private float timeSinceLastMove = 0.0f;

    private string CMD_LEFT = "left", CMD_RIGHT = "right", CMD_STOP = "stop";
    private bool out_of_boarder = false;

    void OnEnable() {
        EnhancedTouchSupport.Enable();
    }
    void Start() {
        moving = false;
    }

    // Update is called once per frame
    void Update() {
        if (Touch.activeTouches.Count == 0) return;
        Debug.Log(NetworkManager.Singleton.Client.IsConnected);
        if (!NetworkManager.Singleton.Client.IsConnected) return; 

        Touch touch = Touch.activeTouches[0];
        switch (touch.phase) {
            case TouchPhase.Began:
                timeSinceLastMove = 0.0f;
                moving = true;
                lastPos = touch.screenPosition;
                borderLeft = lastPos.x - borderLimitDistance;
                borderRight = lastPos.x + borderLimitDistance;
                SendButtonDownMessage();
                break;
            case TouchPhase.Stationary:
                lastPos = touch.screenPosition;
                if (out_of_boarder && lastPos.x >= borderLeft && lastPos.x <= borderRight) {
                    out_of_boarder = false;
                    SendRotationMessage(CMD_STOP);
                } else if (!out_of_boarder && lastPos.x < borderLeft) {
                    SendRotationMessage(CMD_LEFT);
                    out_of_boarder = true;
                } else if (!out_of_boarder && lastPos.x > borderRight) {
                    SendRotationMessage(CMD_RIGHT);
                    out_of_boarder = true;
                }
                break;
            case TouchPhase.Ended:
                out_of_boarder = false;
                SendRotationMessage(CMD_STOP);
                SendButtonUpMessage();
                break;
        }
    }

    public void SendButtonDownMessage() {
        Debug.Log("Button down");
        if (NetworkManager.Singleton.Client.IsConnected == false) return;

        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.name);
        message.AddString("Button down");
        NetworkManager.Singleton.Client.Send(message);
    }

    public void SendButtonUpMessage() {
        if (NetworkManager.Singleton.Client.IsConnected == false) return;

        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.name);
        message.AddString("Button up");
        NetworkManager.Singleton.Client.Send(message);
    }

    private void SendRotationMessage(string cmd) {
        Message message = Message.Create(MessageSendMode.reliable, (ushort)ClientToServerId.name);
        message.AddString(cmd);
        NetworkManager.Singleton.Client.Send(message);
    }
}

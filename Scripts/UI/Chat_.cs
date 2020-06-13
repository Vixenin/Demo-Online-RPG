using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Mirror;

public class Chat_ : NetworkBehaviour
{
    [SerializeField] private GameObject UI_ = null;
    [SerializeField] private TMP_Text chatText = null;
    [SerializeField] private TMP_InputField inputField = null;

    private static event Action<string> OnMessage;

    //User Name 
    [SerializeField] private TMP_Text playerNameBubble = null;

    public override void OnStartAuthority()
    {
        UI_.SetActive(true);

        OnMessage += HandleNewMessage;
    }

    [Client]
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            inputField.ActivateInputField();
        }
    }

    [ClientCallback]
    private void OnDestroy()
    {
        if (!hasAuthority) { return; }

        OnMessage -= HandleNewMessage;
    }

    private void HandleNewMessage(string message)
    {
        chatText.text += message;
    }


    [Client]
    public void Send(string message)
    {
        if (!Input.GetKeyDown(KeyCode.Return)) { return; }

        if (string.IsNullOrWhiteSpace(message)) { return; }

        CmdSendMessage(message);

        inputField.text = string.Empty;
    }

    [Client]
    public void Send_BT()
    {
        Input.GetKeyDown(KeyCode.Return);
    }

    [Command]
    private void CmdSendMessage(string message)
    {
        RpcHandleMessage($"[{playerNameBubble.text}]: {message}");
    }

    [ClientRpc]
    private void RpcHandleMessage(string message)
    {
        OnMessage?.Invoke($"\n{message}");
    }
}
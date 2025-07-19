using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using UnityEngine;
using WeChatWASM;
using Random = UnityEngine.Random;

public class NetClient : Singleton<NetClient>
{
    private int Port = 2048;
    private Socket socket;
    private byte[] buffer = new byte[20];
    private WXTCPSocket wxtcpSocket;
    public AI_Enemy Enemy;

    private void Start()
    {
        StartConnect();
#if !UNITY_EDITOR       
        if (WX.GetLaunchOptionsSync().query.TryGetValue("id", out string id))
        {
            Join(id);
        }
#endif
    }

    public void StartConnect()
    {
        if (socket != null || wxtcpSocket != null) return;
        Debug.Log("Start Connect");
        var host = "120.78.190.140";
#if !UNITY_EDITOR
        wxtcpSocket = WX.CreateTCPSocket();
        wxtcpSocket.Connect(new TCPSocketConnectOption()
        {
            address = host,
            port = Port
        });
        wxtcpSocket.OnConnect((_) =>
        {
            Debug.Log("Connect success");
        });
        WXSDKManagerHandler.WX_RegisterTCPSocketOnMessageCallback(WXSDKManagerHandler._TCPSocketOnMessageCallback);
        wxtcpSocket.OnMessage((res) =>
        {
            var message = Encoding.Default.GetString(res.message);
            Debug.Log("message is: " + message);
            HandleOperation(message);
        });
        wxtcpSocket.OnError((err) =>
        {
            Debug.Log(err);
        });
        Debug.Log(wxtcpSocket);
#else
        var address = Dns.GetHostEntry(host).AddressList[0];
        socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
        socket.Blocking = false;
        socket.ConnectAsync(address, Port); 
#endif
    }

    public void Invite()
    {
        StartConnect();
        string id = Time.deltaTime + "" + Random.Range(0.0f, 100.0f);
        WX.ShareAppMessage(new ShareAppMessageOption()
        {
            query = $"id={id}"
        });
        socket.Send(Encoding.UTF8.GetBytes($"Invite/{id}"));
    }
    
    public void Match()
    {
        StartConnect();
#if !UNITY_EDITOR
        wxtcpSocket.Write("Match");
#else
        socket.Send(Encoding.UTF8.GetBytes("Match"));
#endif
    }

    void Join(string id)
    {
        StartConnect();
#if !UNITY_EDITOR
        wxtcpSocket.Write($"Join/{id}");
#else
        socket.Send(Encoding.UTF8.GetBytes($"Join/{id}"));
#endif
    }

    private void Update()
    {
#if !!UNITY_EDITOR
        if (socket != null && socket.Connected)
        {
            if (socket.Poll(0, SelectMode.SelectRead))
            {
                buffer = new byte[20];
                socket.Receive(buffer);
                
                HandleOperation(Encoding.UTF8.GetString(buffer));
            }
        }
#endif
    }

    void Exit()
    {
#if !UNITY_EDITOR
        wxtcpSocket?.Close();
        wxtcpSocket = null;
#else
        socket?.Close();
        socket = null;
#endif
    }
    
    public void SendOperation(OperatorOption op)
    {
        var str = ((int)op).ToString();
#if !UNITY_EDITOR
        wxtcpSocket.Write(str);
#else
        socket.Send(Encoding.UTF8.GetBytes(str));
#endif
    }

    void HandleOperation(string result)
    {
        if (result.Contains("WIN"))
        {
            Debug.Log("YOU WIN!");
            GameManager.Instance.GameOver(true);
            GameManager.Instance.isMultiPlayer = false;
            Exit();
            return;
        } else if (result.Contains("LOSE"))
        {
            Debug.Log("YOU LOSE!");
            GameManager.Instance.GameOver(false);
            GameManager.Instance.isMultiPlayer = false;
            Exit();
            return;
        }

        if (result.Contains("IDLE"))
        {
            Debug.Log("Wait for player");
            return;
        }
        else if (result.Contains("START"))
        {
            Debug.Log("Start");
            GameManager.Instance.isMultiPlayer = true;
            GameManager.Instance.StartGame();
            return;
        }

        Type opType = typeof(OperatorOption);
        var opName = Enum.GetNames(opType)[int.Parse(result)];
        Enum.TryParse(opType, opName, out var op);
        Enemy.Operator((OperatorOption)op);
        Debug.Log("Enemy choose: " + (OperatorOption)op);
        
        GameManager.Instance.RoundSettle();
    }

    private void OnDestroy()
    {
        Exit();
    }
}

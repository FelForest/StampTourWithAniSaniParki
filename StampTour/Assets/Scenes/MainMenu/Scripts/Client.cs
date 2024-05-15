using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;

public class Client : MonoBehaviour
{
    public string serverIP = "127.0.0.1"; // 서버의 IP 주소
    public int serverPort = 12000;        // 서버의 포트 번호

    private TcpClient client;

    void Start()
    {

    }

    async void ConnectToServer()
    {
        try
        {
            // 서버에 연결
            client = new TcpClient();
            await client.ConnectAsync(serverIP, serverPort);
            Debug.Log("Connected to server.");

            // 서버로 메시지 보내기
            string message = "Hello, server!";
            SendMessageToServer(message);
        }
        catch (Exception e)
        {
            Debug.LogError("Error connecting to server: " + e.Message);
        }
    }

    async void SendMessageToServer(string message)
    {
        try
        {
            // 메시지를 바이트 배열로 변환
            byte[] data = Encoding.ASCII.GetBytes(message);

            // 서버로 메시지 보내기
            NetworkStream stream = client.GetStream();
            await stream.WriteAsync(data, 0, data.Length);
            Debug.Log("Message sent to server: " + message);
        }
        catch (Exception e)
        {
            Debug.LogError("Error sending message to server: " + e.Message);
        }
    }

    void OnDestroy()
    {
        if (client != null)
        {
            client.Close();
            Debug.Log("Connection closed.");
        }
    }

    public void StartClient()
    {
        ConnectToServer();
    }
}

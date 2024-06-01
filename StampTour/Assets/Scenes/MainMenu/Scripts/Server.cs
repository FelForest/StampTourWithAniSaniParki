using System.Collections;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
public class Server : MonoBehaviour
{
    public int port = 12000;

    private bool isRunning = false;
    private TcpListener server;

    // Start is called before the first frame update
    void Start()
    {
        StartServer();
    }

    private async void StartServer()
    {
        try
        {
            string ipAddress = "127.0.0.1";
            server = new TcpListener(IPAddress.Parse(ipAddress), port);
            server.Start();
            Debug.Log("Server started");
            Debug.Log(IPAddress.Parse(ipAddress) + " : " +port);
            isRunning = true;
            await StartListening();
        }
        catch (Exception e)
        {
            Debug.Log("Server error : " + e.Message);
        }
    }

    private async Task StartListening()
    {
        while (isRunning)
        {
            try
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                Debug.Log("client connect");
                HandleClient(client);
            }
            catch (Exception e)
            {
                Debug.Log("Accepting client error : " + e.Message);
            }
        }
    }

    private async void HandleClient(TcpClient client)
    {
        try
        {
            // Get NetworkStream for Communications
            NetworkStream stream = client.GetStream();

            byte[] buffer = new byte[1024];
            int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

            string dataReceived = Encoding.ASCII.GetString(buffer, 0, bytesRead);
            //int dataReceived = BitConverter.ToInt32(buffer, 0);
            Debug.Log("Received from client: " + dataReceived);
        }
        catch (Exception e)
        {
            Debug.LogError("Handling client error: " + e.Message);
        }
        finally
        {
            client.Close();
            Debug.Log("Client connection closed");
        }
    }

    private void OnDestroy()
    {
        isRunning = false;
        if (server != null)
        {
            server.Stop();
            Debug.Log("Server Close");
        }
    }
}

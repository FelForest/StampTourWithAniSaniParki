using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Net.Sockets;
using System.Text;
using System.IO;

namespace MainMenu
{
    public class Client : MonoBehaviour
    {
        TcpClient client;
        string serverIP = "127.0.0.1";
        int port = 8000;
        byte[] receivedBuffer;
        StreamReader reader;
        StreamWriter writer;
        bool socketReady = false;
        NetworkStream stream;

        bool canSend = true;
        public CameraManager cameraManager; // CameraManager 스크립트 참조

        public float sendTime = 2.0f;
        void Start()
        {
            CheckReceive();
        }

        void Update()
        {
            if (socketReady)
            {
                if (stream.DataAvailable)
                {
                    receivedBuffer = new byte[100];
                    stream.Read(receivedBuffer, 0, receivedBuffer.Length); // stream에 있던 바이트배열 내려서 새로 선언한 바이트배열에 넣기
                    string msg = Encoding.UTF8.GetString(receivedBuffer, 0, receivedBuffer.Length); // byte[] to string
                    Debug.Log(msg);
                }

                if (cameraManager.isCamera)
                {
                    if(canSend)
                    {
                        canSend = false;
                        StartCoroutine(SendCameraFrameToServer());
                    }
                }
            }
        }

        void SetisCamera()
        {
            canSend = true;
        }
        void CheckReceive()
        {
            if (socketReady) return;
            try
            {
                client = new TcpClient(serverIP, port);

                if (client.Connected)
                {
                    stream = client.GetStream();
                    reader = new StreamReader(stream);
                    writer = new StreamWriter(stream);
                    writer.AutoFlush = true;
                    Debug.Log("Connect Success");
                    socketReady = true;
                }
            }
            catch (Exception e)
            {
                Debug.Log("On client connect exception " + e);
            }
        }

        void OnApplicationQuit()
        {
            CloseSocket();
        }

        void CloseSocket()
        {
            if (!socketReady) return;

            reader.Close();
            writer.Close();
            client.Close();
            socketReady = false;
        }


        public void SendMessageToServer(string message)
        {
            if (socketReady)
            {
                byte[] msgBuffer = Encoding.UTF8.GetBytes(message);
                stream.Write(msgBuffer, 0, msgBuffer.Length);
                Debug.Log("Message Sent: " + message);
            }
        }

        IEnumerator SendCameraFrameToServer()
        {
            byte[] frame = cameraManager.GetCameraFrame();
            if (frame != null && socketReady && stream != null)
            {
                // 이미지 크기를 전송
                byte[] frameLength = BitConverter.GetBytes(frame.Length);
                Debug.Log(frame.Length);
                stream.Write(frameLength, 0, frameLength.Length);

                // 이미지 데이터를 전송
                stream.Write(frame, 0, frame.Length);
                Debug.Log("Camera Frame Sent");
            }
            yield return new WaitForSeconds(sendTime);
            canSend = true;
        }
    }
}

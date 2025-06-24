//dotnet build
//dotnet run

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Net;
using System.Net.Sockets;


public class TxtFileRead : MonoBehaviour
{
    private UdpClient client;
    private IPEndPoint endPoint;
    private bool exitFlag;
    public int recv;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = 60;

        client = new UdpClient(12345);
        endPoint = new IPEndPoint(IPAddress.Any, 0);
        exitFlag = false;

        StartCoroutine(ReceiveDataCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            exitFlag = true;
        }
    }

    private IEnumerator ReceiveDataCoroutine()
    {
        while (!exitFlag)
        {
            if (client.Available > 0)
            {
                byte[] receivedData = client.Receive(ref endPoint);

                // 受信データがint型の整数値からなる場合
                if (receivedData.Length == sizeof(int))
                {
                    recv = BitConverter.ToInt32(receivedData);

                    // 受信データの処理
                    Debug.Log("Received data: " + recv);
                }
                else
                {
                    Debug.LogError("Invalid received data size.");
                }
            }

            yield return null;
        }

        client.Close();
    }
}

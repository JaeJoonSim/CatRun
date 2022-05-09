using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ��Ʈ��ũ ���� �߰�
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    GameObject player;
    Socket clntSocket;
    EndPoint serverEP;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        clntSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        serverEP = new IPEndPoint(IPAddress.Loopback, 10200);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SendData(int dir)
    {
        byte[] buf = new byte[16];
        byte[] recvBytes = new byte[16];

        if (dir == -1)
            buf = Encoding.UTF8.GetBytes("-1");
        else if (dir == 1)
            buf = Encoding.UTF8.GetBytes("1");
        else
            buf = Encoding.UTF8.GetBytes("0");

        clntSocket.SendTo(buf, serverEP);

        int nRecv = clntSocket.ReceiveFrom(recvBytes, ref serverEP);
        string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);

        // ���� ������ ������ ����̸� �̵���Ű�� �޼ҵ带 Ȱ��
        player.GetComponent<PlayerController>().transformCat(txt);

        Debug.Log(txt);
    }
}

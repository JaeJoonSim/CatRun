// 이 예제는 앞 쪽의 UDP 서버와 함께 작동합니다.
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;


class Program
{
    static void Main(string[] args)
    {
        Socket clntSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        byte[] buf = Encoding.UTF8.GetBytes(DateTime.Now.ToString());

        EndPoint serverEP = new IPEndPoint(IPAddress.Loopback, 10200);

        clntSocket.SendTo(buf, serverEP);

        byte[] recvBytes = new byte[1024];
        int nRecv = clntSocket.ReceiveFrom(recvBytes, ref serverEP);
        string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);

        Console.WriteLine(txt);
    }
}
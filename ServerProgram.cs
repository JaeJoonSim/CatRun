using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
// MySQL 용 헤더 파일을 추가 합니다.
using MySql.Data.MySqlClient;

class ServerProgram
{
    static void Main(string[] args)
    {
        // 서버 소켓이 동작하는 스레드
        Thread serverThread = new Thread(serverFunc);
        serverThread.IsBackground = true;
        serverThread.Start();
        Thread.Sleep(500); // 소켓 서버용 스레드가 실행될 시간을 주기 위해

        Console.WriteLine("*** 고양이 게임용 서버 입니다. ***");
        Console.WriteLine("종료하려면 아무 키나 누르세요...");
        Console.ReadLine();
    }

    private static void serverFunc(object obj)
    {
        Socket srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 10200);

        srvSocket.Bind(endPoint);
        Console.WriteLine("고양이 게임을 위한 UDP 서버가 동작을 시작했습니다.");

        byte[] recvBytes = new byte[1024];
        EndPoint clientEP = new IPEndPoint(IPAddress.None, 0);
        while (true)
        {
            int nRecv = srvSocket.ReceiveFrom(recvBytes, ref clientEP);
            string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);
            Console.WriteLine("- 게임 유저가 보내온 기록 : " + txt);

            string[] Data = txt.Split(',');

            switch (Data[0])
            {
                case "Database":
                    try
                    {
                        // 데이터 베이스 실행
                        String strConn = "Server=localhost; Database = ckgame; Uid=root;pwd=root;";
                        MySqlConnection conn = new MySqlConnection(strConn);

                        conn.Open();
                        String sql = "select *, rank() over(order by score asc) as ranking from userinfo limit 10;";

                        MySqlCommand cmd = new MySqlCommand(sql, conn);
                        MySqlDataReader table = cmd.ExecuteReader();

                        while (table.Read())
                        {
                            Console.WriteLine("{0} : {1}위 기록 = {2}", table["name"], table["ranking"], table["score"]);
                        }
                        Console.WriteLine("\n");

                        table.Close();
                        conn.Close();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.StackTrace);
                    }
                    break;
            }
            

            byte[] sendBytes = Encoding.UTF8.GetBytes("- 서버가 승인한 기록 : " + txt);
            srvSocket.SendTo(sendBytes, clientEP);
        }
    }    
}

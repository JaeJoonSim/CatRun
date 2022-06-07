using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;
//데이터 베이스
using MySql.Data.MySqlClient;

class Program
{
    static List<EndPoint> PlayerIP = new List<EndPoint>();
    static Dictionary<string, string> playerData = new Dictionary<string, string>();
    static Dictionary<string, string> playerpos = new Dictionary<string, string>();
    static Dictionary<string, string> playerHP = new Dictionary<string, string>();

    static Socket srvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
    static IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 10200);

    static int random_num;
    static void Main(string[] args)
    {
        // 서버 소켓이 동작하는 스레드
        Thread serverThread = new Thread(serverFunc);
        serverThread.IsBackground = true;
        serverThread.Start();
        Thread.Sleep(500); // 소켓 서버용 스레드가 실행될 시간을 주기 위해

        Console.WriteLine("*** 고양이 게임용 서버 입니다. ***");
        Console.WriteLine("종료하려면 아무 키나 누르세요...");
        Thread thread = new Thread(() => RackStep());
        thread.Start();
        Console.ReadLine();

    }

    public static void RackStep()
    {
        Random randomobj = new Random();
        while (true)
        {
            string txt;
            random_num = randomobj.Next(-7,7);

            Thread.Sleep(500);
            txt = string.Format("Random,{0}", random_num);
            sendAll(txt);

            //Console.WriteLine("Random: " + random_num);
            if (playerData == null) return;
            foreach (KeyValuePair<string, string> item in playerData)
            {
                 txt = string.Format("R,{0},{1}", item.Key, item.Value);
                sendAll(txt);
            }
        }
    }

    static void sendAll(string txt)
    {
        if (PlayerIP == null) return;
        //string txt;
        byte[] sendBytes;
        
        foreach (EndPoint client in PlayerIP)
        {
            sendBytes = Encoding.UTF8.GetBytes(txt);
            srvSocket.SendTo(sendBytes, client);
        }
    }

    private static void serverFunc(object obj)
    {

  

        srvSocket.Bind(endPoint);
        Console.WriteLine("고양이 게임을 위한 UDP 서버가 동작을 시작했습니다.");

        byte[] recvBytes = new byte[1024];
        EndPoint clientEP = new IPEndPoint(IPAddress.None, 0);  

        while (true)
        {
            int nRecv = srvSocket.ReceiveFrom(recvBytes, ref clientEP);
            string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);

            //Console.WriteLine("ip: " + clientEP);

            string[] Data = txt.Split(',');
            byte[] sendBytes ;
            switch (Data[0]) 
            {
                case "Exit":
                    playerData.Remove(Data[1]);
                    PlayerIP.Remove(clientEP);
                    sendAll(txt);
                    break;
                case "hp":
                    if (playerHP.ContainsKey(Data[1]))
                    {
                        playerHP[Data[1]] = Data[3];
                    }
                    else
                    {
                        playerHP.Add(Data[1], Data[3]);
                    }
                    playerData[Data[1]] = string.Format("{0},{1}", playerHP[Data[1]], playerpos[Data[1]]);
                    txt = Data[2];
                    break;
                case "Pos":
                    if (playerHP.ContainsKey(Data[1]))
                    {
                        playerHP[Data[1]] = Data[3];
                    }
                    else
                    {
                        playerHP.Add(Data[1], Data[3]);
                    }
                    playerData[Data[1]] = string.Format("{0},{1}", playerHP[Data[1]], playerpos[Data[1]]);
                    txt = Data[2];
                    break;
                case "pos":
                    if (playerpos.ContainsKey(Data[1]))
                    {
                        playerpos[Data[1]] = Data[2];
                    }
                    else
                    {
                        playerpos.Add(Data[1], Data[2]);
                    }
                    playerData[Data[1]] = string.Format("{0},{1}", playerHP[Data[1]], playerpos[Data[1]]);
                    txt = Data[2];
                    break;
                case "Check":
                    txt = "Check,";
                    if (playerData.ContainsKey(Data[1]))
                    {
                        txt += "X";
                    }
                    else
                    {
                        txt += "O";
                        playerHP.Add(Data[1], "10");
                        playerpos.Add(Data[1], "0");
                        PlayerIP.Add(clientEP);
                    }              
                    break;
                case "Database":
                    //Database
                    if (Data[0] == "Database")
                    {
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
                    }
                    break;
            }

            //Console.WriteLine("send: " + txt);
            sendBytes = Encoding.UTF8.GetBytes(txt);
            srvSocket.SendTo(sendBytes, clientEP);

        }
    }
}
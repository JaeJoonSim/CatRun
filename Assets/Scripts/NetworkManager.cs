using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
// 네트워크 관련 추가
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

public class NetworkManager : MonoBehaviour
{
    [SerializeField]
    GameObject player , player2, Game_End;
    Socket clntSocket;
    EndPoint serverEP;
    Dictionary<string, GameObject> playerList = new Dictionary<string, GameObject>();

    public bool Netstart = false;
    public int RandominServer;
    public int HP;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
        clntSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        serverEP = new IPEndPoint(IPAddress.Loopback, 10200);
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!Netstart) return;
    
        try
        {
            PosCheck();
            Random();
            HPCheck();
            //RackStep();
        }
        catch (Exception e)
        {
            //GameManager.start = false;
            Debug.Log(e);
        }

    }
    void Random()
    {
        string Data = SendData("Random");
        RandominServer = int.Parse(Data);
    }
    void RackStep()
    {
        byte[] recvBytes = new byte[16];
        int nRecv = clntSocket.ReceiveFrom(recvBytes, ref serverEP);
        Debug.Log(nRecv);
        string txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);
        string[] Data = txt.Split(',');
        switch (Data[0])
        {
            case "R":
                Debug.Log(Data[1]);
                if (Data[1] != GameManager.playerName)
                    if (playerList.ContainsKey(Data[1]))
                    {
                        playerList[Data[1]].GetComponent<OtherPlayerController>().Pos(Data[3]);
                        //playerList[Data[1]].GetComponent<OtherPlayerController>().HPUI(Data[2]);
                    }
                    else
                    {
                        GameObject newOBJ = Instantiate(player2);
                        newOBJ.transform.position = new Vector3(0, -3.6f, 0);
                        newOBJ.GetComponent<OtherPlayerController>().Name(Data[1]);
                        playerList.Add(Data[1], newOBJ);
                    }
                break;
            case "Random":
                RandominServer = int.Parse(Data[1]);
                break;
            case "Exit":
                Destroy(playerList[Data[1]]);
                playerList.Remove(Data[1]);
                break;
        }
    }

    void PosCheck()
    {
        float posx = player.transform.position.x;
        double rd3 = Math.Round(posx, 1);
        string BufData = string.Format("pos,{0},{1}", GameManager.playerName, rd3);
        BufData = SendData(BufData);
        string[] str = BufData.Split(':');

        for (int i = 0; i < str.Length-1; i++)
        {
            string[] Data = str[i].Split(',');
            Debug.Log(Data[0]);
            if (Data[0] != GameManager.playerName)
                if (playerList.ContainsKey(Data[0]))
                {
                    playerList[Data[0]].GetComponent<OtherPlayerController>().Pos(Data[1]);
                }
                else
                {
                    GameObject newOBJ = Instantiate(player2);
                    newOBJ.transform.position = new Vector3(0, -3.6f, 0);
                    newOBJ.GetComponent<OtherPlayerController>().Name(Data[0]);
                    playerList.Add(Data[0], newOBJ);
                }
        }
        string Del = null;
        if (playerList != null)
            foreach (KeyValuePair<string, GameObject> item in playerList)
            {
                bool have = false;
                for (int i = 0; i < str.Length - 1; i++)
                {
                    string[] Data = str[i].Split(',');
           
                    if (Data[0] == item.Key)
                    {
                        have = true;
                        break;
                    }
                }
                if (!have)
                {
                    Destroy(item.Value);
                    Del = item.Key;
                }
            }
        if(Del != null)
        {
            Debug.Log(Del);
            playerList.Remove(Del);
        }
        if(playerList.Count > 0)
        {
            GameManager.start = true;
        }
        else
        {
            GameManager.start = false;
        }
    }

    void HPCheck()
    {
        int hp = HP;
        string BufData = string.Format("hp,{0},{1}", GameManager.playerName, hp);
        BufData = SendData(BufData);

        Debug.Log(BufData);
        if (hp < 1)
        {
            GameEnd(true);
        }
        string[] str = BufData.Split(':');

        for (int i = 0; i < str.Length - 1; i++)
        {
            string[] Data = str[i].Split(',');
            if (Data[0] != GameManager.playerName)
                if (playerList.ContainsKey(Data[0]))
                {
                    int _hp = int.Parse(Data[1]);
                    if (_hp < 1)
                    {
                        GameEnd(false);
                    }
                    playerList[Data[0]].GetComponent<OtherPlayerController>().HPUI(_hp);
                }
        }
    }

    void GameEnd(bool who)
    {
        Debug.Log("End");
        Game_End.SetActive(true);
        GameManager.start = false;
        if (who)
        {
            Game_End.transform.Find("Text").transform.GetComponent<Text>().text = "You lose";
        }
        else
        {
            Game_End.transform.Find("Text").transform.GetComponent<Text>().text = "You Win";
        }

    }

    public string SendData(string BufData)
    {
        string txt = "";
        try
        {
            byte[] buf;
            byte[] recvBytes = new byte[16];

            buf = Encoding.UTF8.GetBytes(BufData);
            clntSocket.SendTo(buf, serverEP);

            //BufData = string.Format("P,{0},{1}", playerName, rd3);
            //buf = Encoding.UTF8.GetBytes(BufData);
            //clntSocket.SendTo(buf, serverEP);

            int nRecv = clntSocket.ReceiveFrom(recvBytes, ref serverEP);
            txt = Encoding.UTF8.GetString(recvBytes, 0, nRecv);      
        }
        catch (Exception e)
        {
            //GameManager.start = false;
            Debug.Log(e);
        }
        return txt;
    }
    void OnApplicationPause(bool pauseStatus)
    {
        string BufData = string.Format("Exit,{0}", GameManager.playerName);
        SendData(BufData);
    }
    void OnApplicationQuit()
    {
        string BufData = string.Format("Exit,{0}", GameManager.playerName);
        SendData(BufData);
    }
}

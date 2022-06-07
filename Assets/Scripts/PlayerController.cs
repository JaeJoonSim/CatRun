using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameObject NetworkManager;

    [Header("플레이어 이동속도")]
    public float speed;


    Vector3 pos;

    private void Start()
    {
        NetworkManager = GameObject.Find("NetworkManager");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.start) return;

            //moveX = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        //transform.Translate(new Vector2(moveX, 0));

        int dir = (int)Input.GetAxisRaw("Horizontal");

        string BufData;
        if (dir == -1)
            BufData = "-1";
        else if (dir == 1)
            BufData = "1";
        else
            BufData = "0";
        float posx = transform.position.x;
        double rd3 = Math.Round(posx, 1);
        BufData = string.Format("pos,{0},{1},{2}", GameManager.playerName, BufData, rd3);

        BufData = NetworkManager.GetComponent<NetworkManager>().SendData(BufData);
        string[] Data = BufData.Split(',');
        transformCat(Data[0]);


        // 화면 밖으로 못나가게 하기
        pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }

    // 네트워크를 위한 메소드 추가
    public void transformCat(string txt)
    {
        if (txt == "-1")
            transform.Translate(new Vector2(-1 * speed * Time.deltaTime, 0));
        else if (txt == "1")
            transform.Translate(new Vector2(1 * speed * Time.deltaTime, 0));
    }
}

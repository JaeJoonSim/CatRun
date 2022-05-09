using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    GameObject NetworkManager;

    [Header("플레이어 이동속도")]
    public float speed;

    float moveX;
    Vector3 pos;

    private void Start()
    {
        NetworkManager = GameObject.Find("NetworkManager");
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        //transform.Translate(new Vector2(moveX, 0));
        NetworkManager.GetComponent<NetworkManager>().SendData((int)Input.GetAxisRaw("Horizontal"));

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

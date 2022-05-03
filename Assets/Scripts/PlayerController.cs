using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("�÷��̾� �̵��ӵ�")]
    public float speed;

    float moveX;
    Vector3 pos;

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxisRaw("Horizontal") * speed * Time.deltaTime;
        transform.Translate(new Vector2(moveX, 0));

        // ȭ�� ������ �������� �ϱ�
        pos = Camera.main.WorldToViewportPoint(transform.position);
        if (pos.x < 0f) pos.x = 0f;
        if (pos.x > 1f) pos.x = 1f;
        transform.position = Camera.main.ViewportToWorldPoint(pos);
    }
}

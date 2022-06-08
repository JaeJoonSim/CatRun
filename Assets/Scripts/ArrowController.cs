using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    float SPEED = -5f;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("player");
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.start) Destroy(gameObject);

        float speed = SPEED * Time.deltaTime;
        transform.Translate(0, speed, 0);

        if(transform.position.y < -5.0f)
        {
            Destroy(gameObject);
        }

        Vector2 p1 = transform.position;
        Vector2 p2 = player.transform.position;
        Vector2 dir = p1 - p2;
        float d = dir.magnitude;
        float r1 = 0.5f;
        float r2 = 1.0f;

        if(d < r1 + r2)
        {
            Destroy(gameObject);

            GameObject director = GameObject.Find("GameManager");
            director.GetComponent<GameManager>().DecreaseHP();
        }
    }
}

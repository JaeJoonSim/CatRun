using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowGenerator : MonoBehaviour
{
    public GameObject arrowPrefab;
    float span = 0.4f;
    float delta = 0;

    GameObject NetworkManager;

    private void Start()
    {
        NetworkManager = GameObject.Find("NetworkManager");
    }


    // Update is called once per frame
    void Update()
    {
        if (!GameManager.start) return;
        delta += Time.deltaTime;
        if(delta > span)
        {
            delta = 0;
            GameObject go = Instantiate(arrowPrefab);

            //string Data = NetworkManager.GetComponent<NetworkManager>().SendData("Random");

            int px = NetworkManager.GetComponent<NetworkManager>().RandominServer;
            go.transform.position = new Vector3(px, 7, 0);
        }
    }
}

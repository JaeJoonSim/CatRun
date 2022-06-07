using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OtherPlayerController : MonoBehaviour
{
    public GameObject hpGage;
    public Text text;
    public int HP;

    void Start()
    {
        
    }
    public void Name(string name)
    {
        text.text = name;
    }
    public void Pos(string P)
    {
        float x = float.Parse(P);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }
    public void HPUI(string P)
    {
        HP = int.Parse(P);
        hpGage.GetComponent<Image>().fillAmount = HP * 0.1f;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    GameObject NetworkManager;

    public GameObject Name_UI, Warning_UI;

    [SerializeField]
    InputField Name_input;
    public static string playerName = "name";
    public static bool start = false;

    public GameObject hpGage;

    public int HP = 10;

    private void Start()
    {
        NetworkManager = GameObject.Find("NetworkManager");
        SetResolution();
        Application.runInBackground = true;
    }

    public void NameChack()
    {
        playerName = Name_input.text;
        if (playerName == "Check")
            Warning();

        string Data = "Check," + playerName;
        Data = NetworkManager.GetComponent<NetworkManager>().SendData(Data);
        string[] txt = Data.Split(',');
        if (txt[0] == "Check")
        {
            if(txt[1] == "O")
            {
                Name_UI.SetActive(false);
                start = true;
            }
            else
            {
                Warning();
            }
        }
    }

    public void Warning()
    {
        Warning_UI.SetActive(true);
        return;
    }

    public void DecreaseHP()
    {
        HP--;
        hpGage.GetComponent<Image>().fillAmount = HP * 0.1f;
    }
    public void SetResolution()
    {
        int setWidth = 1920; // 화면 너비
        int setHeight = 1080; // 화면 높이

        //해상도를 설정값에 따라 변경
        //3번째 파라미터는 풀스크린 모드를 설정 > true : 풀스크린, false : 창모드
        Screen.SetResolution(setWidth, setHeight, false);
    }
}

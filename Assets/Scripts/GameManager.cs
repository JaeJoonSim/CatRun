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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            NameChack();
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
                NetworkManager.GetComponent<NetworkManager>().Netstart = true;
                NetworkManager.GetComponent<NetworkManager>().HP = HP;
                //start = true;
            }
            else
            {
                Warning();
            }
        }
        if (!start)
        {
            HP = 10;
        }
    }

    public void Warning()
    {
        Warning_UI.SetActive(true);
        return;
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void DecreaseHP()
    {
        HP--;
        NetworkManager.GetComponent<NetworkManager>().HP = HP;
        hpGage.GetComponent<Image>().fillAmount = HP * 0.1f;
    }
    public void SetResolution()
    {
        int setWidth = 1920; // ȭ�� �ʺ�
        int setHeight = 1080; // ȭ�� ����

        //�ػ󵵸� �������� ���� ����
        //3��° �Ķ���ʹ� Ǯ��ũ�� ��带 ���� > true : Ǯ��ũ��, false : â���
        Screen.SetResolution(setWidth, setHeight, false);
    }
}

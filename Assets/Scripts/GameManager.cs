using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject hpGage;

    public void DecreaseHP()
    {
        hpGage.GetComponent<Image>().fillAmount -= 0.1f;
    }
}

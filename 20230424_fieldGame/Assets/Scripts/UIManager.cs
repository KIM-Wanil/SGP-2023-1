using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI jewelText;
    public int jewelCount = 0;
    public int allJewelCount = 3;
    public int timer = 0;

    private void Start()
    {
        StartCoroutine(TimerCoroution());
        jewelText.text = jewelCount.ToString() +  "/" + allJewelCount.ToString();
    }

    IEnumerator TimerCoroution()
    {
        timer += 1;

        timerText.text =  (timer / 60 % 60).ToString("D2") + ":" + (timer % 60).ToString("D2");

        yield return new WaitForSeconds(1f);

        StartCoroutine(TimerCoroution());
    }
}
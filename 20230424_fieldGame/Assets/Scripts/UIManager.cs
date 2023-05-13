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
    public int life = 3;

    public GameObject heartImage;
    public GameObject[] hearts;
    Vector2[] heartsPos;
    public GameObject canvas;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").gameObject;
        jewelText = canvas.transform.Find("JewelText").GetComponent<TextMeshProUGUI>();
        timerText = canvas.transform.Find("TimerText").GetComponent<TextMeshProUGUI>();
        StartCoroutine(TimerCoroution());       
        heartImage = Resources.Load<GameObject>("Images/HeartImage");
        hearts = new GameObject[3];//index0~3:spawn skill //index4~7:combat skill
        heartsPos = new Vector2[3];
        Vector2 direction = new Vector2(0f, 1f);
        heartImage.GetComponent<RectTransform>().anchorMin = direction;
        heartImage.GetComponent<RectTransform>().anchorMax = direction;
        heartImage.GetComponent<RectTransform>().pivot = direction;
        for (int i = 0; i < 3; i++)
        {
            heartsPos[i] = new Vector2(100 * i, -200f);
        }
        for (int i = 0; i < 3; i++)
        {
            MakeSkillButton(i);
        }
    }
    public void Update()
    {
        jewelText.text = "Goals : " + jewelCount.ToString() + "/" + allJewelCount.ToString();
    }
    public void MakeSkillButton(int num)
    {
        hearts[num] = (GameObject)Instantiate(heartImage);
        hearts[num].transform.SetParent(canvas.transform);
        hearts[num].transform.GetComponent<RectTransform>().anchoredPosition = heartsPos[num];
        hearts[num].transform.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
    }
    IEnumerator TimerCoroution()
    {
        timer += 1;

        timerText.text =  "Time : " + (timer / 60 % 60).ToString("D2") + ":" + (timer % 60).ToString("D2");

        yield return new WaitForSeconds(1f);

        StartCoroutine(TimerCoroution());
    }
}
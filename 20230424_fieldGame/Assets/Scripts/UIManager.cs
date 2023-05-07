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

    public GameObject skillButton;
    public GameObject[] skillButtons;
    Vector2[] skillButtonsPos;
    public GameObject canvas;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").gameObject;
        jewelText = canvas.transform.Find("JewelText").GetComponent<TextMeshProUGUI>();
        timerText = canvas.transform.Find("TimerText").GetComponent<TextMeshProUGUI>();

        StartCoroutine(TimerCoroution());       
        skillButton = Resources.Load<GameObject>("Images/HeartImage");
        skillButtons = new GameObject[3];//index0~3:spawn skill //index4~7:combat skill
        skillButtonsPos = new Vector2[3];
        Vector2 direction = new Vector2(0f, 1f);
        skillButton.GetComponent<RectTransform>().anchorMin = direction;
        skillButton.GetComponent<RectTransform>().anchorMax = direction;
        skillButton.GetComponent<RectTransform>().pivot = direction;
        for (int i = 0; i < 3; i++)
        {
            skillButtonsPos[i] = new Vector2(100 * i, -200f);
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
        skillButtons[num] = (GameObject)Instantiate(skillButton);
        skillButtons[num].transform.SetParent(canvas.transform);
        skillButtons[num].transform.GetComponent<RectTransform>().anchoredPosition = skillButtonsPos[num];
    }
    IEnumerator TimerCoroution()
    {
        timer += 1;

        timerText.text =  "Time : " + (timer / 60 % 60).ToString("D2") + ":" + (timer % 60).ToString("D2");

        yield return new WaitForSeconds(1f);

        StartCoroutine(TimerCoroution());
    }
}
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
    public GameObject canvas;
    Vector2[] skillButtonsPos;

    private void Start()
    {
        canvas = GameObject.Find("Canvas").gameObject;
        StartCoroutine(TimerCoroution());       
        skillButton = Resources.Load<GameObject>("Prefabs/Image/HeartImage");
        skillButtons = new GameObject[3];//index0~3:spawn skill //index4~7:combat skill
        skillButtonsPos = new Vector2[3];
        for (int i = 0; i < 3; i++)
        {
            skillButtonsPos[i] = new Vector2(-750f + 100 * i, 300f);          
        }
        //for(int i=0;i<3;i++)
        //{
        //    MakeSkillButton(i);
        //}
    }
    //public void MakeSkillButton(int num)
    //{
    //    skillButtons[num] = (GameObject)Instantiate(skillButton);
    //    skillButtons[num].transform.SetParent(canvas.transform);
    //    skillButtons[num].transform.GetComponent<RectTransform>().anchoredPosition = skillButtonsPos[num];
    //}
    private void Update()
    {
        jewelText.text = jewelCount.ToString() + "/" + allJewelCount.ToString();
    }
    IEnumerator TimerCoroution()
    {
        timer += 1;

        timerText.text =  (timer / 60 % 60).ToString("D2") + ":" + (timer % 60).ToString("D2");

        yield return new WaitForSeconds(1f);

        StartCoroutine(TimerCoroution());
    }
}
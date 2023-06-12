using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI jewelText;
    public TextMeshProUGUI talismanCountText;
    public TextMeshProUGUI systemText;
    public TextMeshProUGUI warningText;
    public TextMeshProUGUI warningText2;
    public TextMeshProUGUI explainText;
    public Image dashGaugeBar;
    public Image dashBackground;
    public float currentDashGauge;
    public bool canDash;
    public int life;
    public int timer = 0;


    public GameObject heartImage;
    public GameObject[] hearts;
    Vector2[] heartsPos;
    public GameObject canvas;
    public GameObject player;

    

    private void Start()
    {
        Debug.Log("SceneStart");
        life = GameManager.instance.life;
        player = GameObject.Find("Player").gameObject;
        canvas = GameObject.Find("Canvas").gameObject;
        jewelText = canvas.transform.Find("JewelText").GetComponent<TextMeshProUGUI>();
        timerText = canvas.transform.Find("TimerText").GetComponent<TextMeshProUGUI>();
        talismanCountText = canvas.transform.Find("talismanIcon").Find("CountText").GetComponent<TextMeshProUGUI>();
        systemText = canvas.transform.Find("SystemText").GetComponent<TextMeshProUGUI>();
        warningText = canvas.transform.Find("WarningText").GetComponent<TextMeshProUGUI>();
        warningText2 = canvas.transform.Find("WarningText2").GetComponent<TextMeshProUGUI>();
        explainText = canvas.transform.Find("ExplainText").GetComponent<TextMeshProUGUI>();
        dashGaugeBar = canvas.transform.Find("DashGaugeBar").GetComponent<Image>();
        dashBackground = canvas.transform.Find("DashBackground").GetComponent<Image>();
        currentDashGauge = player.transform.GetComponent<PlayerControl>().currentDashGauge;
        canDash = player.transform.GetComponent<PlayerControl>().canDash;
        StartCoroutine(TimerCoroution());       
        heartImage = Resources.Load<GameObject>("Images/HeartImage");
        hearts = new GameObject[3];//index0~3:spawn skill //index4~7:combat skill
        heartsPos = new Vector2[3];
        Vector2 direction = new Vector2(0f, 1f);
        heartImage.GetComponent<RectTransform>().anchorMin = direction;
        heartImage.GetComponent<RectTransform>().anchorMax = direction;
        heartImage.GetComponent<RectTransform>().pivot = direction;
        for (int i = 0; i < life; i++)
        {
            heartsPos[i] = new Vector2(100 * i, -200f);
        }
        for (int i = 0; i < life; i++)
        {
            MakeHeartIcon(i);
        }
    }
    public void Update()
    {
        jewelText.text = "Goals : " + GameManager.instance.jewelCount.ToString() + "/" + GameManager.instance.allJewelCount.ToString();
        talismanCountText.text = GameManager.instance.talismanCount.ToString();
        currentDashGauge = player.transform.GetComponent<PlayerControl>().currentDashGauge;
        dashGaugeBar.fillAmount = currentDashGauge / 1f;
        canDash = player.transform.GetComponent<PlayerControl>().canDash;
        if (canDash) dashBackground.color = Color.blue;
        else dashBackground.color = Color.white;
    }
    public void MakeHeartIcon(int num)
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
    public void RemoveHearts(int num)
    {
        Destroy(hearts[num].gameObject);
    }
}
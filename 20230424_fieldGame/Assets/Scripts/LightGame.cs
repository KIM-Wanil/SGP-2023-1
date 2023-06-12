using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LightGame : MonoBehaviour
{
    private const int COUNT_X = 4;
    private const int COUNT_Y = 4;
    private const float TIME = 7f;

    private float timeLimit = TIME;     // 제한 시간
    //public AudioClip correctSound;  // 올바른 입력 소리
    //public AudioClip wrongSound;    // 잘못된 입력 소리
    private bool[] correctBulbStates;    // 현재 알파벳 6개
    private bool[] currentBulbStates;       
    private float timeLeft;                         // 남은 시간
    private bool isStart=false;
    private GameObject bulb;
    private GameObject[] bulbs;
    private Vector2[] bulbPos;
    private GameObject canvas;
    private GameObject gauge;
    private Image bar;

    // 2023/05/14 장진혁추가
    PlayerControl player;
    PlayerInteractive interactive;
    UIManager uiManager;

    // 알파벳을 저장할 배열
    //private char[] alphabets = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    // 랜덤으로 알파벳 6개를 생성하는 함수
    private void GenerateBulbs()
    {
        bulbs = new GameObject[COUNT_X * COUNT_Y];
        for (int i = 0; i < COUNT_X* COUNT_Y; i++)
        {
            MakeBulbButton(i);
            int randomIndex = Random.Range(0, 2);
            if (randomIndex == 0)
            {
                bulbs[i].GetComponent<Bulb>().SetOn();
                correctBulbStates[i] = true;
            }
            else if (randomIndex == 1)
            {
                bulbs[i].GetComponent<Bulb>().SetOff();
                correctBulbStates[i] = false;
            }
        }
    }
    private void Start()
    {
        correctBulbStates = new bool[COUNT_X * COUNT_Y];
        currentBulbStates = new bool[COUNT_X * COUNT_Y];
        canvas = GameObject.Find("Canvas").gameObject;
        bulb = Resources.Load<GameObject>("Prefabs/Bulb");
        gauge = Resources.Load<GameObject>("Prefabs/Gauge");
        bulbs = new GameObject[COUNT_X*COUNT_Y];
        bulbPos = new Vector2[COUNT_X*COUNT_Y];
        Vector2 direction = new Vector2(0.5f, 0.5f);
        bulb.GetComponent<RectTransform>().anchorMin = direction;
        bulb.GetComponent<RectTransform>().anchorMax = direction;
        bulb.GetComponent<RectTransform>().pivot = direction;
        gauge.GetComponent<RectTransform>().anchorMin = direction;
        gauge.GetComponent<RectTransform>().anchorMax = direction;
        gauge.GetComponent<RectTransform>().pivot = direction;
        gauge = (GameObject)Instantiate(gauge);
        gauge.transform.SetParent(canvas.transform);
        gauge.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -300f);
        gauge.transform.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
        bar = gauge.transform.Find("Bar").GetComponent<Image>();

        for (int i = 0; i < COUNT_Y; i++)
        {
            float y = -150 + i * 100;
            for (int j = 0; j < COUNT_X; j++)
            {
                float x = -150 + j * 100;
                bulbPos[i*COUNT_Y+j] = new Vector2(x,y);
            }
       
        }
        timeLeft = timeLimit;
        SetGame();
        Invoke("StartGame", 1f);

        //2023/05/14 장진혁추가
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        interactive = GameObject.Find("Player").GetComponent<PlayerInteractive>();
        uiManager = GameObject.Find("EventSystem").GetComponent<UIManager>();
    }
    public void MakeBulbButton(int num)
    {
        bulbs[num] = (GameObject)Instantiate(bulb);
        bulbs[num].transform.SetParent(canvas.transform);
        bulbs[num].transform.GetComponent<RectTransform>().anchoredPosition = bulbPos[num];
        bulbs[num].transform.GetComponent<RectTransform>().localScale = new Vector2(0.7f, 0.7f);
        bulbs[num].transform.GetComponent<Bulb>().num = num;
        bulbs[num].transform.GetComponent<Button>().interactable = false;
        //bulbs[num].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = currentAlphabets[num].ToString();
    }

    // 키 입력 처리
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            GameManager.instance.escapedFromMinigame();
            GameOver();
            Destroy(gameObject);

            return;
        }
        // 죽었는데 게임중이었다면 게임 삭제
        if (GameManager.instance.playMinigame && GameManager.instance.dead)
        {
            GameOver();
            Destroy(gameObject);

            return;
        }
        // 게임이 종료되었으면 입력을 받지 않음       
        if (!isStart)
        {
            return;
        }
        uiManager.explainText.text = "화면에 나온 빛의 모양을 그대로 누르세요!";

        for (int i = 0; i < COUNT_X * COUNT_Y; i++)
        {
            currentBulbStates[i] = bulbs[i].GetComponent<Bulb>().isOn;
        }
        for (int i = 0; i < COUNT_X * COUNT_Y; i++)
        {
            if (currentBulbStates[i] == correctBulbStates[i])
            {
                if (i == COUNT_X * COUNT_Y - 1)
                {
                    GameClear();
                }
                else
                {
                    continue;
                }
            }
            else
            {
                break;
            }
        }
        timeLeft -= Time.deltaTime;
        bar.fillAmount = timeLeft / timeLimit;

        if (timeLeft <= 0f)
        {
            GameOver();
        }
    }

    // 게임 종료 처리
    private void GameOver()
    {
        //2023/05/14 장진혁추가
        player.isActionProgress = false;
        GameManager.instance.dead = false;
        interactive.gameGenerating = false;

        uiManager.explainText.text = "";

        foreach (GameObject key in bulbs)
        {
            Destroy(key);
        }
        Destroy(gauge);
        Destroy(gameObject);
        Debug.Log("GameOver");
    }

    // 게임 시작 처리
    public void SetGame()
    {
        if (bulbs != null)
        {
            foreach(GameObject bulb in bulbs)
            Destroy(bulb);
        } 
        GenerateBulbs();
    }
    public void StartGame()
    {
        for (int i = 0; i < COUNT_X * COUNT_Y; i++)
        {
            bulbs[i].GetComponent<Bulb>().SetOff();
            bulbs[i].transform.GetComponent<Button>().interactable = true;
        }
        isStart = true;
    }

    // 게임 성공 처리
    private void GameClear ()
    {
        //2023/05/14 장진혁추가
        player.isActionProgress = false;
        interactive.gameClear = true;
        interactive.gameGenerating = false;

        uiManager.explainText.text = "";

        foreach (GameObject bulb in bulbs)
        { 
            Destroy(bulb); 
        }
        Destroy(gauge);
        Destroy(transform.parent.gameObject);
        Debug.Log("GameClear");
        //alphabetText.text = "Game Clear!";
    }
}

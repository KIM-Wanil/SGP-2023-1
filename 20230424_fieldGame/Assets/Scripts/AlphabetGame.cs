using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AlphabetGame : MonoBehaviour
{
    private const int COUNT = 7;
    private const float TIME = 5f;

    private float timeLimit = TIME;     // 제한 시간
    //public AudioClip correctSound;  // 올바른 입력 소리
    //public AudioClip wrongSound;    // 잘못된 입력 소리
    private char[] currentAlphabets;    // 현재 알파벳 6개
    private int currentOrder;                       // 현재 알파벳의 순서
    private float timeLeft;                         // 남은 시간
    private GameObject key;
    private GameObject[] keys;
    private Vector2[] keyPos;
    private GameObject canvas;
    private GameObject gauge;
    private Image bar;

    // 알파벳을 저장할 배열
    private char[] alphabets = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

    //2023/05/14 장진혁추가
    PlayerControl player;

    // 랜덤으로 알파벳 6개를 생성하는 함수
    private void GenerateAlphabets()
    {
        for (int i = 0; i < COUNT; i++)
        {
            int randomIndex = Random.Range(0, alphabets.Length);
            currentAlphabets[i] = alphabets[randomIndex];
        }
        currentOrder = 0;
        //UpdateAlphabetText();
    }
    private void Start()
    {
        canvas = GameObject.Find("Canvas").gameObject;
        key = Resources.Load<GameObject>("Prefabs/Key");
        gauge = Resources.Load<GameObject>("Prefabs/Gauge");
        keys = new GameObject[COUNT];//index0~3:spawn skill //index4~7:combat skill
        keyPos = new Vector2[COUNT];
        Vector2 direction = new Vector2(0.5f, 0.5f);
        key.GetComponent<RectTransform>().anchorMin = direction;
        key.GetComponent<RectTransform>().anchorMax = direction;
        key.GetComponent<RectTransform>().pivot = direction;
        gauge.GetComponent<RectTransform>().anchorMin = direction;
        gauge.GetComponent<RectTransform>().anchorMax = direction;
        gauge.GetComponent<RectTransform>().pivot = direction;
        gauge = (GameObject)Instantiate(gauge);
        gauge.transform.SetParent(canvas.transform);
        gauge.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -200f);
        gauge.transform.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
        bar = gauge.transform.Find("Bar").GetComponent<Image>();
        for (int i = 0; i < COUNT; i++)
        {
            keyPos[i] = new Vector2(-300 + 100 * i, 0f);
        }       
        currentAlphabets = new char[COUNT+1];
        timeLeft = timeLimit;
        StartGame();

        //2023/05/14 장진혁 추가
        player = GameObject.Find("Player").GetComponent<PlayerControl>();

    }
    public void MakeKeyButton(int num)
    {
        keys[num] = (GameObject)Instantiate(key);
        keys[num].transform.SetParent(canvas.transform);
        keys[num].transform.GetComponent<RectTransform>().anchoredPosition = keyPos[num];
        keys[num].transform.GetComponent<RectTransform>().localScale = new Vector2(0.7f, 0.7f);
        keys[num].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = currentAlphabets[num].ToString();
    }

    // 키 입력 처리
    private void Update()
    {
        // 게임이 종료되었으면 입력을 받지 않음
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameOver();
            return;
        }
            // 현재 입력된 키와 현재 알파벳이 일치하면 다음 알파벳으로 넘어감
        if (Input.anyKeyDown)
        {
            if (Input.inputString.ToUpper() == currentAlphabets[currentOrder].ToString())
            {
                //currentOrder++;
                Destroy(keys[currentOrder++]);
                if (currentOrder == COUNT)
                {
                    // 모든 알파벳을 입력했으면 다음 라운드로 넘어감
                    //GenerateAlphabets();
                    GameClear();
                    Debug.Log("Clear");
                }
            }
            else
            {
                StartGame();
                // 잘못된 키 입력 처리
                //AudioSource.PlayClipAtPoint(wrongSound, transform.position);
            }
        }

        // 제한 시간이 지났으면 게임 종료
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

        foreach (GameObject key in keys)
        {
            Destroy(key);
        }
        Destroy(gauge);
        Destroy(this);
        Debug.Log("GameOver");
        //alphabetText.text = "Game Over!";
    }

    // 게임 시작 처리
    public void StartGame()
    {
        if (keys != null)
        {
            foreach(GameObject key in keys)
            Destroy(key);
        }

        keys = new GameObject[COUNT];
        GenerateAlphabets();
        for (int i = 0; i < COUNT; i++)
        {
            MakeKeyButton(i);
        }
    }

    // 게임 성공 처리
    private void GameClear ()
    {
        //2023/05/14 장진혁추가
        player.isActionProgress = false;
        player.gameClear = true;

        foreach (GameObject key in keys)
        { 
            Destroy(key); 
        }
        Destroy(gauge);
        Debug.Log("GameClear");
        //alphabetText.text = "Game Clear!";
    }
}

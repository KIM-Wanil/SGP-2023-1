using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class TimingGame : MonoBehaviour
{

    private const float TIME = 5f;
    private const int COUNT = 3;
    private float speed = 300f;
    private int score = 0;
    private float timeLimit = TIME;     // 제한 시간
    //public AudioClip correctSound;  // 올바른 입력 소리
    //public AudioClip wrongSound;    // 잘못된 입력 소리
    private float timeLeft;                         // 남은 시간

    private GameObject ring;
    private GameObject clock;
    private GameObject hand;
    private GameObject[] partition;
    //private GameObject[] bulbs;
    //private Vector2[] bulbPos;
    private GameObject canvas;
    private GameObject gauge;
    private Image bar;

    //2023/05/14 장진혁추가
    PlayerControl player;
    PlayerInteractive interactive;
    UIManager uiManager;

    private void GeneratePartition()
    {
        
        //float randomSize = Random.Range(0.05f, 0.08f);

        partition = new GameObject[COUNT];
        for (int i = 0; i < COUNT; i++)
        {
            partition[i] = (GameObject)Instantiate(ring);
            partition[i].transform.SetParent(clock.transform);
            partition[i].transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            partition[i].transform.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
            partition[i].GetComponent<Image>().color = new Color(0f, 0f, 255f);
            partition[i].GetComponent<Image>().fillAmount = 0.1f;// randomSize;
            //float randomRotation = Random.Range(10f, 20f);
            partition[i].transform.Rotate(0f, 0f, 30f - i * 120f);
        }
    }
    private void Start()
    {
        canvas = GameObject.Find("Canvas").gameObject;
        ring = Resources.Load<GameObject>("Prefabs/Ring");
        hand = Resources.Load<GameObject>("Prefabs/Hand");
        gauge = Resources.Load<GameObject>("Prefabs/Gauge");
        bar = gauge.transform.Find("Bar").GetComponent<Image>();

        Vector2 direction = new Vector2(0.5f, 0.5f);
        ring.GetComponent<RectTransform>().anchorMin = direction;
        ring.GetComponent<RectTransform>().anchorMax = direction;
        ring.GetComponent<RectTransform>().pivot = direction;

        clock = (GameObject)Instantiate(ring);
        clock.transform.SetParent(canvas.transform);
        clock.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        clock.transform.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);

        gauge.GetComponent<RectTransform>().anchorMin = direction;
        gauge.GetComponent<RectTransform>().anchorMax = direction;
        gauge.GetComponent<RectTransform>().pivot = direction;
        gauge = (GameObject)Instantiate(gauge);
        gauge.transform.SetParent(canvas.transform);
        gauge.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, -300f);
        gauge.transform.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);

        hand = (GameObject)Instantiate(hand);
        hand.transform.SetParent(canvas.transform);
        hand.transform.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
        hand.transform.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);

        
        timeLeft = timeLimit;
        GeneratePartition();

        //2023/05/14 장진혁추가
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        interactive = GameObject.Find("Player").GetComponent<PlayerInteractive>();
        uiManager = GameObject.Find("EventSystem").GetComponent<UIManager>();
    }

    private void StopSpeed()
    {
        speed = 0f;
    }
    private void ResetSpeed()
    {
        speed = 300f;
    }
    // 키 입력 처리
    private void Update()
    {
        uiManager.explainText.text = "타이밍에 맞춰서 스페이스바를 누르세요!";
        hand.transform.Rotate(0f, 0f, -speed * Time.deltaTime);
        
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            float handZ = (hand.transform.eulerAngles.z + 360f) % 360;
            foreach (GameObject part in partition)
            {      
                float partZ = (part.transform.eulerAngles.z + 360f) % 360;
                if(partZ<36f)
                {
                    if ( (( handZ < partZ+10f && handZ > 0) ||  (handZ < 360 && handZ >360f-partZ-10f))   && part.activeSelf)
                    {
                        Debug.Log(part.transform.eulerAngles.z);
                        score++;
                        part.SetActive(false);
                        Invoke("StopSpeed", 0f);
                        Invoke("ResetSpeed", 0.5f);
                        return;
                    }
                }
                else 
                {
                    if (handZ <= partZ+10f &&  handZ >= partZ - 46f && part.activeSelf)
                    {
                        score++;
                        part.SetActive(false);
                        Invoke("StopSpeed", 0f);
                        Invoke("ResetSpeed", 0.5f);
                        return;
                    }
                }
               
            }
            GameOver();
        }
        if(score==COUNT)
        {
            GameClear();
        }
        timeLeft -= Time.deltaTime;
        bar.fillAmount = timeLeft / timeLimit;
        if (timeLeft <= 0f)
        {
            GameOver();
        }
    }

    //게임 실패 처리
    private void GameOver()
    {
        //2023/05/14 장진혁추가
        player.isActionProgress = false;
        GameManager.instance.dead = false;
        interactive.gameGenerating = false;
        uiManager.explainText.text = "";

        foreach (GameObject part in partition)
        {
            Destroy(part);
        }
        Destroy(gauge);
        Destroy(clock);
        Destroy(hand);
        Destroy(gameObject);
        Debug.Log("GameOver");
    }


    // 게임 성공 처리
    private void GameClear()
    {
        //2023/05/14 장진혁추가
        player.isActionProgress = false;
        interactive.gameClear = true;
        interactive.gameGenerating = false;

        uiManager.explainText.text = "";

        foreach (GameObject part in partition)
        {
            Destroy(part);
        }
        Destroy(gauge);
        Destroy(clock);
        Destroy(hand);
        Destroy(transform.parent.gameObject);
        Debug.Log("GameClear");
    }
}

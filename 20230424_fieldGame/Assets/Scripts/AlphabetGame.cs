using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class AlphabetGame : MonoBehaviour
{
    private const int COUNT = 8;
    private const float TIME = 7f;

    private float timeLimit = TIME;     // ���� �ð�
    //public AudioClip correctSound;  // �ùٸ� �Է� �Ҹ�
    //public AudioClip wrongSound;    // �߸��� �Է� �Ҹ�
    private char[] currentAlphabets;    // ���� ���ĺ� 6��
    private int currentOrder;                       // ���� ���ĺ��� ����
    private float timeLeft;                         // ���� �ð�
    private GameObject key;
    private GameObject[] keys;
    private Vector2[] keyPos;
    private GameObject canvas;
    private GameObject gauge;
    private Image bar;

    // ���ĺ��� ������ �迭
    private char[] alphabets = { 'E', 'F', 'Q', 'R', 'S', 'W','A','D' };

    //2023/05/14 �������߰�
    PlayerControl player;
    PlayerInteractive interactive;
    UIManager uiManager;

    // �������� ���ĺ� 6���� �����ϴ� �Լ�
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
            keyPos[i] = new Vector2(-350 + 100 * i, 0f);
        }       
        currentAlphabets = new char[COUNT+1];
        timeLeft = timeLimit;
        StartGame();

        //2023/05/14 ������ �߰�
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        interactive = GameObject.Find("Player").GetComponent<PlayerInteractive>();
        uiManager = GameObject.Find("EventSystem").GetComponent<UIManager>();
    }
    public void MakeKeyButton(int num)
    {
        keys[num] = (GameObject)Instantiate(key);
        keys[num].transform.SetParent(canvas.transform);
        keys[num].transform.GetComponent<RectTransform>().anchoredPosition = keyPos[num];
        keys[num].transform.GetComponent<RectTransform>().localScale = new Vector2(0.7f, 0.7f);
        keys[num].transform.Find("Text").GetComponent<TextMeshProUGUI>().text = currentAlphabets[num].ToString();
    }

    // Ű �Է� ó��
    private void Update()
    {
        uiManager.explainText.text = "ȭ�鿡 ���� Ű�� ������� �Է��ϼ���!";
        // ������ ����Ǿ����� �Է��� ���� ����
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameManager.instance.escapedFromMinigame();
            GameOver();
            Destroy(gameObject);

            return;
        }
        // �׾��µ� �������̾��ٸ� ���� ����
        if (GameManager.instance.playMinigame && GameManager.instance.dead)
        {
            GameOver();
            Destroy(gameObject);

            return;
        }
        // ���� �Էµ� Ű�� ���� ���ĺ��� ��ġ�ϸ� ���� ���ĺ����� �Ѿ
        if (Input.anyKeyDown)
        {
            if (Input.inputString.ToUpper() == currentAlphabets[currentOrder].ToString())
            {
                //currentOrder++;
                Destroy(keys[currentOrder++]);
                if (currentOrder == COUNT)
                {
                    // ��� ���ĺ��� �Է������� ���� ����� �Ѿ
                    //GenerateAlphabets();
                    GameClear();
                }
            }
            else
            {
                StartGame();
                // �߸��� Ű �Է� ó��
                //AudioSource.PlayClipAtPoint(wrongSound, transform.position);
            }
        }

        // ���� �ð��� �������� ���� ����
        timeLeft -= Time.deltaTime;
        bar.fillAmount = timeLeft / timeLimit;
        if (timeLeft <= 0f)
        {
            GameOver();
        }
    }

    // ���� ���� ó��
    private void GameOver()
    {
        //2023/05/14 �������߰�
        player.isActionProgress = false;
        GameManager.instance.dead = false;
        uiManager.explainText.text = "";

        foreach (GameObject key in keys)
        {
            Destroy(key);
        }
        Destroy(gauge);
        Destroy(gameObject);
        Debug.Log("GameOver");
        //alphabetText.text = "Game Over!";
    }

    // ���� ���� ó��
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

    // ���� ���� ó��
    private void GameClear ()
    {
        //2023/05/14 �������߰�
        player.isActionProgress = false;
        interactive.gameClear = true;
        uiManager.explainText.text = "";

        foreach (GameObject key in keys)
        { 
            Destroy(key); 
        }
        Destroy(gauge);
        Destroy(transform.parent.gameObject);

        Debug.Log("GameClear");
        //alphabetText.text = "Game Clear!";
    }
}

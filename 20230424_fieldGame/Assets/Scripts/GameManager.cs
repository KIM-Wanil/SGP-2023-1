using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
public class GameManager : Singleton<GameManager>
{
    private UIManager uiManager;
    public int life { get; set; } = 3;
    public int jewelCount { get; set; } = 0;
    public int allJewelCount { get; set; } = 3;
    public float spawnTime { get; set; } = 20f;
    public int talismanCount { get; set; } = 1;
    public bool hide;
    public bool lanternOn;
    private void Start()
    {
        initScene();
    }
    public void initScene()
    {
        uiManager = GameObject.Find("EventSystem").GetComponent<UIManager>();
        life = 3;
        jewelCount = 0;
        allJewelCount = 3;
        spawnTime = 20f;
        talismanCount = 1;
        hide = false;
        lanternOn = false;
    }
    public void GetJewel()
    {
        jewelCount++;
        Debug.Log(jewelCount);
        if (jewelCount >= allJewelCount)
        {
            SceneManager.LoadScene("Clear_Stage");
        }
    }
    public void lostLife()
    {
        life--;
        uiManager.RemoveHearts(life);
        if (life <= 0)
        {
            SceneManager.LoadScene("Fail_Stage");
        }
    }

    public void decreaseSpawnTime()
    {
        if (spawnTime > 5.0f)
            spawnTime -= 0.1f;

        Debug.Log("SpawnTime: " + spawnTime);
    }

    ////////////Scene �ε� ������ ȣ��
    void OnEnable()
    {
        // �� �Ŵ����� sceneLoaded�� ü���� �Ǵ�.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // ü���� �ɾ �� �Լ��� �� ������ ȣ��ȴ�.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        initScene();
        Debug.Log("OnSceneLoaded: " + scene.name);
        //Debug.Log(mode);
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    ////////////
}
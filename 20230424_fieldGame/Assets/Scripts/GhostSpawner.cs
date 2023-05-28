using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    private GameObject ghostPrefab;
    public GameObject ghost;
    private GameObject player;
    private UIManager uiManager;
    public float spawnTime;
    private float spawnTimeLeft;

    void Start()
    {
        player = GameObject.Find("Player");
        uiManager = GameObject.Find("EventSystem").GetComponent<UIManager>();
        ghostPrefab = Resources.Load<GameObject>("Prefabs/Ghost");
        spawnTime = GameManager.instance.spawnTime;
        spawnTimeLeft = spawnTime;
    }
    void Update()
    {
        if (ghost!=null) return;
        if (GameManager.instance.lanternOn)
            spawnTimeLeft -= 2.0f * Time.deltaTime;
        else
            spawnTimeLeft -= Time.deltaTime;

        if(spawnTimeLeft<0)
        {
            Debug.Log("Spawn Ghost");
            SpawnGhost();
            spawnTimeLeft = spawnTime;
        }
    }
    private void SpawnGhost()
    {
        uiManager.warningText.text = "�ͽ� ����!";
        ghost = Instantiate(ghostPrefab, SetPosition(),transform.rotation);
        Invoke("initWarningText", 3f);
    }
    void initWarningText()
    {
        uiManager.warningText.text = "";
    }
    public Vector3 SetPosition()
    {
        float len = 10f;
        Vector3 playerPos = player.transform.position;
        playerPos.y = 0f;
        float angle;
        Vector3 spawnPos;
        if (playerPos.x > 15 && playerPos.x < 35 && playerPos.z > 15 && playerPos.z < 35)
        {
            angle = Random.Range(0f, Mathf.PI * 2);
            spawnPos = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * len;
            spawnPos += playerPos;
            return spawnPos;
        }
        else
        {
            return new Vector3(25f, 0f, 25f);
        }
    }
}

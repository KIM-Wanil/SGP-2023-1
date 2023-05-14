using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpawner : MonoBehaviour
{
    private GameObject ghostPrefab;
    private GameObject ghost;
    private GameObject player;
    private bool isGhostOn;
    private float spawnTime = 3f;
    private float spawnTimeLeft;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        ghostPrefab = Resources.Load<GameObject>("Prefabs/Ghost");
        //ghost = Instantiate(ghostPrefab,this.transform);
        //ghost.SetActive(false);
        //isGhostOn = false;
        spawnTimeLeft = spawnTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (ghost!=null) return;
        spawnTimeLeft -= Time.deltaTime;
        if(spawnTimeLeft<0)
        {
            SpawnGhost();
            spawnTimeLeft = spawnTime;
        }
    }
    private void SpawnGhost()
    {
        ghost = Instantiate(ghostPrefab, SetPosition(),transform.rotation);
    }

    public Vector3 SetPosition()
    {
        float len = 15f;
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

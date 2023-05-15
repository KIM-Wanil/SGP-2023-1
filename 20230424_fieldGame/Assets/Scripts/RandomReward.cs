using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomReward : MonoBehaviour
{
    // box 보상 리스트 생성
    List<int> boxList = new List<int>();
    int box = 16;
    int jewel = 3;
    int curse = 5;
    //int tailsman = 8;

    GameObject jewelObj;
    PlayerControl player;
    PlayerInteractive interactive;



    // Start is called before the first frame update
    void Start()
    {
        jewelObj = Resources.Load<GameObject>("Prefabs/Jewel");
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        interactive = GameObject.Find("Player").GetComponent<PlayerInteractive>();

        for(int i = 0; i < box; i++)
        {
            if (i < jewel)
                boxList.Add(0);
            else if (i < jewel + curse)
                boxList.Add(1);
            else
                boxList.Add(2);
        }
        ShuffleBoxes();

        for (int i = 0; i < box; i++)
            Debug.Log(boxList[i]);
    }

    // Update is called once per frame
    void Update()
    {
        if (interactive.gameClear)
            GetReward();
    }

    void ShuffleBoxes()
    {
        //boxlist random shuffle
        for(int i = 0; i < boxList.Count; i++)
        {
            int temp = boxList[i];
            int randomIndex = 0;
            randomIndex = Random.Range(i, boxList.Count);
            boxList[i] = boxList[randomIndex];
            boxList[randomIndex] = temp;
        }

        // 섞인 리스트에서 jewel의 index가 10을 넘는 것들을 3에서 10사이의 idx로 스왑
        for(int i = 0; i < boxList.Count; i++)
        {
            int temp = boxList[i];
            int randomIndex = 0;
            if(boxList[i] == 0 && i > 9)
            {
                randomIndex = Random.Range(3, 10);
                while(boxList[randomIndex] == 0)
                    randomIndex = Random.Range(3, 10);
                boxList[i] = boxList[randomIndex];
                boxList[randomIndex] = temp;
            }
        }

        // 스왑하는 과정에서 jewel의 idx차이가 1밖에 안나는것들을 떨어트려놓기
        for(int i = 0; i < 10; i++)
        {
            int temp = boxList[i];
            int randomIndex = 0;
            if(boxList[i] == 0 && boxList[i+1] == 0)
            {
                randomIndex = Random.Range(3, 10);
                while(boxList[randomIndex] == 0)
                    randomIndex = Random.Range(3, 10);
                boxList[i] = boxList[randomIndex];
                boxList[randomIndex] = temp;
            }
        }
    }

    void GetReward()
    {
        int box = boxList[interactive.openBoxCount];
        if (box == 0)
        {
            Debug.Log("you get jewel");
            interactive.openBoxCount++;
            Instantiate(jewelObj, player.transform.position + player.transform.forward, player.transform.rotation);
            interactive.gameClear = false;
        }
        else if (box == 1)
        {
            Debug.Log("you are cursed");
            interactive.openBoxCount++;
            player.getCurse = true;
            interactive.gameClear = false;
        }
        else
        {
            Debug.Log("you get talisman");
            interactive.openBoxCount++;
            interactive.talismanCount++;
            interactive.gameClear = false;
        }
    }
}

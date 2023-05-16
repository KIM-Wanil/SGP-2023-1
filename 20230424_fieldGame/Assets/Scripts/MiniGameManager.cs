using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MiniGameManager : MonoBehaviour
{

    PlayerControl player;
    PlayerInteractive interactive;
    GameObject alphabetgameObj;
    GameObject timinggameObj;
    GameObject lightgameObj;
    DungeonGeneratorByBinarySpacePartitioning.MapGenerator mapgenerator;

    int randomNum;
    //public bool isOn = false;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        interactive = GameObject.Find("Player").GetComponent<PlayerInteractive>();
        alphabetgameObj = Resources.Load<GameObject>("Prefabs/AlphabetGameGenerator");
        timinggameObj = Resources.Load<GameObject>("Prefabs/TimingGameGenerator");
        lightgameObj = Resources.Load<GameObject>("Prefabs/LightGameGenerator");
        mapgenerator = GameObject.Find("EventSystem").GetComponent<DungeonGeneratorByBinarySpacePartitioning.MapGenerator>();
    }

    void Update()
    {
        //if(interactive.check_box)
        //{
        //    RandomGameGenerate();
        //}
    }

    public void RandomGameGenerate()
    {
        player.isActionProgress = true;
        randomNum = Random.Range(0, 3);

        if (randomNum == 0)
        {
            GameObject go = Instantiate(alphabetgameObj);
            go.transform.SetParent(this.transform);
        }
        else if (randomNum == 1)
        {
            GameObject go = Instantiate(timinggameObj);
            go.transform.SetParent(this.transform);
        }
        else if (randomNum == 2)
        {
            GameObject go = Instantiate(lightgameObj);
            go.transform.SetParent(this.transform);
        }

        interactive.check_box = false;
    }
}

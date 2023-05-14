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
        if(interactive.check_box)
        {
            RandomGameGenerate();
        }
    }

    void RandomGameGenerate()
    {
        player.isActionProgress = true;
        randomNum = Random.Range(0, 3);

        if (randomNum == 0)
            Instantiate(alphabetgameObj);
        else if (randomNum == 1)
            Instantiate(timinggameObj);
        else if (randomNum == 2)
            Instantiate(lightgameObj);

        interactive.check_box = false;
    }
}

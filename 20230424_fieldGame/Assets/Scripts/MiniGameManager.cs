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

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        interactive = GameObject.Find("Player").GetComponent<PlayerInteractive>();
        alphabetgameObj = Resources.Load<GameObject>("Prefabs/AlphabetGameGenerator");
        timinggameObj = Resources.Load<GameObject>("Prefabs/TimingGameGenerator");
        lightgameObj = Resources.Load<GameObject>("Prefabs/LightGameGenerator");
    }

    void Update()
    {
        if(interactive.check_box)
            RandomGameGenerate();

        if(player.gameClear)
            Clear();
    }

    void RandomGameGenerate()
    {
        interactive.check_box = false;
        player.isActionProgress = true;

        int randomNum = Random.Range(1, 3);

        if (randomNum == 1)
            Instantiate(alphabetgameObj);
        else if (randomNum == 2)
            Instantiate(timinggameObj);
        else
            Instantiate(lightgameObj);
    }

    void Clear()
    {
        Destroy(gameObject);
    }

    void Fail()
    {
        Destroy(this);
    }
}

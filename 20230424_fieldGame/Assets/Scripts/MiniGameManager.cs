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

    int randomNum;

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
    }

    public void RandomGameGenerate()
    {
        player.isActionProgress = true;
        //interactive.gameGenerating = false;
        GameManager.instance.playMinigame = true;
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

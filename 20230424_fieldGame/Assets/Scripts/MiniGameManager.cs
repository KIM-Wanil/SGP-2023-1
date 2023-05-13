using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public bool gameClear;

    GameObject alphabetgameObj;
    GameObject timinggameObj;
    GameObject lightgameObj;

    void Start()
    {
        alphabetgameObj = Resources.Load<GameObject>("Prefabs/AlphabetGameGenerator");
        timinggameObj = Resources.Load<GameObject>("Prefabs/TimingGameGenerator");
        lightgameObj = Resources.Load<GameObject>("Prefabs/LightGameGenerator");
        gameClear = false;
    }

    void Update()
    {
        RandomGameGenerate();
        if(gameClear)
        {
            Clear();
        }
        else
        {
            Fail();
        }
    }

    void RandomGameGenerate()
    {
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

    }

    void Fail()
    {

    }
}

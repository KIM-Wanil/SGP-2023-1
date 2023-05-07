using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractive : MonoBehaviour
{
    PlayerControl player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

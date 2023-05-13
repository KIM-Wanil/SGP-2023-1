using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractive : MonoBehaviour
{
    public bool hide, check_box;

    PlayerControl player;
    GameObject Hideimg;
    float interact_distance;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        interact_distance = 5f;
        hide = false;
        check_box = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckRay();
    }

    void CheckRay()
    {
        RaycastHit hit;

        if(Physics.Raycast(player.transform.position, player.transform.forward, out hit, interact_distance))
        {
            if(hit.collider.CompareTag("Box"))
            {
                if(Input.GetKeyDown(KeyCode.F))
                {
                    check_box = true;
                    Debug.Log("box");
                }    
            }

            if(hit.collider.CompareTag("Hideout"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    hide = true;
                    Debug.Log("hideout");
                }
            }
        }
    }
}

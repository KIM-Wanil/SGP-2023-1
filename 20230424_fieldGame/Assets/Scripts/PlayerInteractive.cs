using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractive : MonoBehaviour
{
    public bool hide, check_box;

    PlayerControl player;
    float interact_distance;
    GameObject Hideimg;
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
        Debug.DrawRay(player.FPScam.transform.position, player.FPScam.forward * 20f, Color.red);
        CheckRay();
        if (check_box)
            CheckBox();
        if (hide)
            Hide();
    }

    void CheckRay()
    {
        RaycastHit hit;
        
        if(Physics.Raycast(player.FPScam.transform.position,player.FPScam.forward,out hit, interact_distance))
        {
            if(hit.collider.CompareTag("Box"))
            {
                if(Input.GetKeyDown(KeyCode.F))
                    check_box = true;
            }

            if(hit.collider.CompareTag("Hideout"))
            {
                if (Input.GetKeyDown(KeyCode.F))
                    hide = true;
            }
        }
    }

    void Hide()
    {
        

    }

    void CheckBox()
    {
        Debug.Log(check_box);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideout : MonoBehaviour
{
    PlayerControl player;
    PlayerInteractive interactive;
    Transform hideimg;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        interactive = GameObject.Find("Player").GetComponent<PlayerInteractive>();
        hideimg = GameObject.Find("Canvas").transform.Find("Hideoutimg");
    }

    // Update is called once per frame
    void Update()
    {
        if(interactive.hide)
        {
            player.gameObject.SetActive(false);

            Vector3 lookForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            Vector3 temp = player.cam.transform.forward;
            player.cam.transform.forward = lookForward;
            player.cam.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            hideimg.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                interactive.hide = false;
                hideimg.gameObject.SetActive(false);
                player.gameObject.SetActive(true);
                player.cam.transform.forward = temp;
            }
        }

    }
}

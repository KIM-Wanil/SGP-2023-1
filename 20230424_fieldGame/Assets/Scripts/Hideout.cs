using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideout : MonoBehaviour
{
    PlayerInteractive player;
    GameObject FPScam;
    Transform hideimg;
    Vector3 temp;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerInteractive>();
        FPScam = GameObject.Find("FPScam");
        hideimg = GameObject.Find("Canvas").transform.Find("Hideoutimg");
    }

    // Update is called once per frame
    void Update()
    {
        if(player.hide)
        {
            player.gameObject.SetActive(false);

            Vector3 lookForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;
            temp = FPScam.transform.forward;
            FPScam.transform.forward = lookForward;
            FPScam.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            hideimg.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                player.hide = false;
                hideimg.gameObject.SetActive(false);
                player.gameObject.SetActive(true);
                FPScam.transform.forward = temp;
            }
        }

    }
}

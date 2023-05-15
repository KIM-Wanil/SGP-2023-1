using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideout : MonoBehaviour
{
    PlayerControl player;
    PlayerInteractive interactive;
    Transform hideimg;
    public GameObject ghost;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        interactive = GameObject.Find("Player").GetComponent<PlayerInteractive>();
        hideimg = GameObject.Find("Canvas").transform.Find("Hideoutimg");
        ghost = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(interactive.hide)
        {
            Vector3 temp = player.cam.transform.forward;
            Vector3 lookForward = new Vector3(0f,0f,0f);
            //try
            //{
            //    ghost = GameObject.FindWithTag("Ghost").gameObject;
            //}
            //catch
            //{
            //    ghost = null;
            //}
            //if(ghost!=null)
            //{
            //    lookForward = new Vector3(ghost.transform.position.x-transform.position.x, 0, ghost.transform.position.z - transform.position.z).normalized;
            //}
            //else
            //{
            //    lookForward = new Vector3(transform.forward.x, 0, transform.forward.z).normalized;     
            //}
            lookForward = new Vector3(-player.transform.forward.x, 0, -player.transform.forward.z).normalized;
            player.cam.transform.forward = lookForward;
            player.gameObject.SetActive(false);

            player.cam.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            if (!player.cam.GetComponent<CameraRotation>().isOn) player.cam.GetComponent<CameraRotation>().isOn = true;
            hideimg.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.F))
            {
                player.cam.GetComponent<CameraRotation>().isOn = false;
                interactive.hide = false;
                hideimg.gameObject.SetActive(false);
                player.gameObject.SetActive(true);
                player.cam.transform.forward = temp;
            }
        }

    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ghost"))
        {
            if (interactive.hide)
            {
                //Destroy(other.gameObject);
                other.GetComponent<GhostAI>().DestroyGhost();
                Debug.Log("Destroy ghost by hide");
            }
        }
    }
}

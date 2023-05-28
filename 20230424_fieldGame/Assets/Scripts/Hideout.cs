using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hideout : MonoBehaviour
{
    PlayerInteractive interactive;
    Transform hideimg;
    public GameObject ghost;

    void Start()
    {
        interactive = GameObject.Find("Player").GetComponent<PlayerInteractive>();
        hideimg = GameObject.Find("Canvas").transform.Find("Hideoutimg");
        ghost = null;
    }

    void Update()
    {
        if (GameManager.instance.hide)
        {
            //카메라 시점 전환
            //Vector3 temp = transform.forward;
            //Vector3 lookPos = new Vector3(0f, 0f, 0f);

            //try { ghost = GameObject.FindWithTag("Ghost"); }
            //catch
            //{
            //    ghost = null;
            //    return;
            //}

            //if (ghost == null)
            //{
            //    lookPos = interactive.gameObject.transform.position - interactive.closestHideout.transform.position;
            //    lookPos.y = 0;
            //}
            //else
            //{
            //    lookPos = ghost.transform.position - interactive.closestHideout.transform.position;
            //    lookPos.y = 0;
            //}
            //transform.rotation = Quaternion.LookRotation(lookPos, Vector3.up);

            transform.position = interactive.closestHideout.gameObject.transform.position;
            GetComponent<CameraRotation>().enabled = true;
            interactive.gameObject.SetActive(false);
            hideimg.gameObject.SetActive(true);

            //은신처에서 탈출
            if (Input.GetKeyDown(KeyCode.F))
            {
                GameManager.instance.hide = false;
                hideimg.gameObject.SetActive(false);
                interactive.gameObject.SetActive(true);
                GetComponent<CameraRotation>().enabled = false;
            }
        }
        else
            return;
    }
}

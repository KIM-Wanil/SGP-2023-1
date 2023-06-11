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
            transform.position = interactive.closestHideout.gameObject.transform.position;
            GetComponent<CameraRotation>().enabled = true;
            interactive.gameObject.SetActive(false);
            hideimg.gameObject.SetActive(true);

            //은신처에서 탈출
            if (Input.GetKeyDown(KeyCode.Space))
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

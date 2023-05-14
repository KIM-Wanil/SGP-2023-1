using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour 
{

    public GameObject cam;
    public bool isActionProgress;
    public bool gameClear;

    GameObject lantern;
    float speed;

	void Start () 
    {
        Cursor.lockState = CursorLockMode.Locked;

        cam = Resources.Load<GameObject>("Prefabs/Cam");
        cam = Instantiate(cam);

        lantern = Resources.Load<GameObject>("Prefabs/Lantern");

        isActionProgress = false;
        gameClear = false;
        speed = 3f;
	}
	
	// Update is called once per frame
    void Update () 
    {
        AttachCam();
        if(!isActionProgress)
        {
            Move();
            UseLantern();
        }
    }

    void AttachCam()
    {
        cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+2f, this.transform.position.z-2f);
        cam.transform.rotation = Quaternion.Euler(45, 0, 0);
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float rotation_speed = 5f;
        Vector3 dir = new Vector3(h, 0, v);
        if (!(h == 0 && v == 0))
        {
            this.transform.position += dir * speed * Time.deltaTime;
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * rotation_speed);
        }
    }

    void UseLantern()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            Instantiate(lantern);
    }
}

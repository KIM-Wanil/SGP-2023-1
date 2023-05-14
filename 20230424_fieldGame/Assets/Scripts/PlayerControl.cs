using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour 
{

    public GameObject cam;
    public bool isActionProgress;
    public bool getCurse;
    public int life;
    
    [SerializeField] float speed;
    //슬로우를 위해 만든 speed저장 변수
    float tempSpeed;
    float slowTimer;
    GameObject lattern;
	void Start () 
    {
        cam = Resources.Load<GameObject>("Prefabs/Cam");
        cam = Instantiate(cam);

        isActionProgress = false;
        getCurse = false;

        life = 3;
        speed = 3f;
        tempSpeed = speed;
        slowTimer = 0f;

        lattern = GameObject.Find("Player").transform.Find("Lantern").gameObject;
	}
	
	// Update is called once per frame
    void Update () 
    {
        AttachCam();
        if(!isActionProgress)
        {
            Move();
            Lantern();
        }

        if (getCurse)
            Cursed();
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

    void Lantern()
    {
        if(Input.GetKeyDown(KeyCode.Q))
        {
            if (!lattern.activeSelf)
                lattern.SetActive(true);
            else
                lattern.SetActive(false);
        }
    }

    void Cursed()
    {
        float slowSpeed = 1f;

        if (slowTimer < 3.0f)
        {
            slowTimer += Time.deltaTime;
            speed = slowSpeed;
        }
        else if (slowTimer > 3.0f)
        {
            speed = tempSpeed;
            slowTimer = 0f;
            getCurse = false;
        }
    }
}

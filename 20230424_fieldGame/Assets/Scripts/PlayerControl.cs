using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class PlayerControl : MonoBehaviour 
{

    public GameObject cam;
    public bool isActionProgress;
    public bool getCurse;
    
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
        speed = 3f;
        tempSpeed = speed;
        slowTimer = 0f;

        lattern = GameObject.Find("Player").transform.Find("Lantern").gameObject;
	}
	
	// Update is called once per frame
    void Update () 
    {
        AttachCam();
        if(!isActionProgress && !GameManager.instance.usedEscape)
        {
            Move();
            Lantern();
        }

        if (getCurse)
            Cursed();

        //if (Input.GetKeyDown(KeyCode.Z))
        //{
        //    GameManager.instance.GetJewel();
        //}

    }

    void AttachCam()
    {
        cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+5f, this.transform.position.z-2.5f);
        cam.transform.rotation = Quaternion.Euler(60, 0, 0);
    }

    void Move()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float rotation_speed = 7.5f;
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
            {
                lattern.SetActive(true);
                GameManager.instance.lanternOn = true;
            }
            else
            {
                lattern.SetActive(false);
                GameManager.instance.lanternOn = false;
            }
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

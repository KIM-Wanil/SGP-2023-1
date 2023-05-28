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
    Rigidbody rigid;

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
        rigid = GameObject.Find("Player").GetComponent<Rigidbody>();
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

    private void FixedUpdate()
    {
        rigid.angularVelocity = Vector3.zero;
    }

    void AttachCam()
    {
        cam.transform.position = new Vector3(this.transform.position.x, this.transform.position.y+5f, this.transform.position.z-2.5f);
        cam.transform.rotation = Quaternion.Euler(60, 0, 0);
    }

    void Move()
    {
        Vector3 moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        moveDir *= speed * Time.deltaTime;
        transform.position += moveDir;

        float rotationSpeed = 0.1f;

        if (moveDir.magnitude > 0.005f)
        {
            Quaternion q = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, rotationSpeed);
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

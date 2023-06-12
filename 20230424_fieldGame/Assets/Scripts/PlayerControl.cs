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
    float dashSpeed;
    public float currentDashGauge = 1f;
    //public float maxDashGauge = 0f;
    float tempSpeed;
    float slowTimer;
    float slowSpeed = 1f;
    public bool canDash = false;
    [SerializeField] public GameObject lattern;
    Rigidbody rigid;
    Animator animator;
    void Start () 
    {
        cam = Resources.Load<GameObject>("Prefabs/Cam");
        cam = Instantiate(cam);

        isActionProgress = false;
        getCurse = false;
        speed = 2.8f;
        dashSpeed = 4.5f;
        tempSpeed = speed;
        slowTimer = 0f;

        //lattern = GameObject.Find("Player").transform.Find("Lantern").gameObject;
        //lattern.SetActive(false);
        rigid = GameObject.Find("Player").GetComponent<Rigidbody>();
        animator = GetComponentInChildren<Animator>();
    }
	
	// Update is called once per frame
    void Update () 
    {
        if(currentDashGauge>=0f && currentDashGauge <= 1f && animator.GetInteger("State")!= 2)
        {
            currentDashGauge -= 0.05f * Time.deltaTime;
            if (currentDashGauge < 0f) currentDashGauge = 0f;
            else if (currentDashGauge < 0.5f) canDash = true;
        }
        
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
        float rotationSpeed = 0.1f;
        if (moveDir.magnitude > 0.005f)
        {
            Quaternion q = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Lerp(transform.rotation, q, rotationSpeed);       
            if ((Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) && currentDashGauge<=1f && currentDashGauge >= 0f &&canDash)
            {
                
                animator.SetInteger("State",2);
                speed = dashSpeed;
                currentDashGauge += 0.25f * Time.deltaTime;
                if (currentDashGauge > 1f)
                {
                    currentDashGauge = 1f;
                    canDash = false;
                }
            }
            else
            {
                if (currentDashGauge >= 0.5f)
                {
                    canDash = false;
                }
                animator.SetInteger("State", 1);
                speed = tempSpeed;
            }
        }
        else
        {
            animator.SetInteger("State", 0);
        }
        if(getCurse)
        {
            speed = slowSpeed;
        }
        moveDir *= speed * Time.deltaTime;
        transform.position += moveDir;
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

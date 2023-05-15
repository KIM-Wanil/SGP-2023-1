using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    GameObject player;
    NavMeshAgent nav;
    public GameObject talismanEffect;
    UIManager uimanager;
    // Start is called before the first frame update
    void Start()
    {
        uimanager = GameObject.Find("EventSystem").GetComponent<UIManager>();
        talismanEffect = Resources.Load<GameObject>("Prefabs/TalismanEffect");
        player = GameObject.Find("Player");
        nav = GetComponent<NavMeshAgent>();
        if (uimanager.jewelCount == 0) nav.speed = 3f;
        else if (uimanager.jewelCount == 1) nav.speed = 3.25f;
        else if (uimanager.jewelCount == 2) nav.speed = 3.25f;
        //StartCoroutine(ChasePlayer());
        StartChase();

    }

    // Update is called once per frame
    void Update()
    {
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    Destroy(this.gameObject);
        //}
    }
    
    public void DestroyGhost()
    {
        GameObject effect = Instantiate(talismanEffect);
        effect.transform.position = this.gameObject.transform.position;
        Destroy(this.gameObject);
    }
    public void InvokeDestroyGhost()
    {
        Invoke("DestroyGhost", 2f);
    }
    IEnumerator ChasePlayer()
    {
        while (true)
        {         
            player = GameObject.Find("Player");
            if (player != null && this.gameObject.activeSelf)
            {
                if (nav.destination != player.transform.position)
                {
                    nav.isStopped = false;
                    nav.SetDestination(player.transform.position);
                }
            }
            yield return new WaitForSeconds(1.0f);
        }
    }
    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        //other.GetComponent<PlayerControl>().life -= 1;
    //        other.transform.position = new Vector3(25f, 1f, 25f);
    //        Destroy(this.gameObject);
    //    }
    //}
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.GetComponent<PlayerControl>().life -= 1;
            Destroy(collision.collider.GetComponent<PlayerControl>().uimanager.hearts[collision.collider.GetComponent<PlayerControl>().life]);
            collision.collider.gameObject.transform.position = new Vector3(23f, 1f, 23f);
            Destroy(this.gameObject);
        }
    }
    public void StartChase()
    {
        
        
        StartCoroutine(ChasePlayer());
        //nav.ResetPath();
    }
    public void SpeedUp()
    {
        nav.speed += 0.25f;
    }

}

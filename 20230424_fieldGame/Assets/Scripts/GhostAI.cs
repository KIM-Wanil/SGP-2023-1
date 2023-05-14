using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    GameObject player;
    NavMeshAgent nav;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
        nav = GetComponent<NavMeshAgent>();
        //StartCoroutine(ChasePlayer());
        StartChase();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Destroy(this.gameObject);
        }
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
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //other.GetComponent<PlayerControl>().life -= 1;
            other.transform.position = new Vector3(25f, 0f, 25f);
            Destroy(this.gameObject);
        }
    }
    public void StartChase()
    {
        
        
        StartCoroutine(ChasePlayer());
        //nav.ResetPath();
    }

}

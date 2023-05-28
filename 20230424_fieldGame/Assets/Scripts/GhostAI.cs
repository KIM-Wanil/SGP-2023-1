using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostAI : MonoBehaviour
{
    GameObject player;
    NavMeshAgent nav;
    public GameObject talismanEffect;

    void Start()
    {
        talismanEffect = Resources.Load<GameObject>("Prefabs/TalismanEffect");
        player = GameObject.Find("Player");
        nav = GetComponent<NavMeshAgent>();

        //보석 갯수에 따른 귀신 스피드 조절
        if (GameManager.instance.jewelCount == 0) nav.speed = 3.0f;
        else if (GameManager.instance.jewelCount == 1) nav.speed = 3.5f;
        else if (GameManager.instance.jewelCount == 2) nav.speed = 4.0f;
        else nav.speed = 3.0f;
        StartChase();
    }

    void Update()
    {
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.gameObject.transform.Find("Lantern").gameObject.SetActive(false);
            GameManager.instance.dead = true;
            GameManager.instance.lostLife();
            collision.collider.gameObject.transform.position = new Vector3(23f, 1f, 23f);
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Hideout"))
        {
            if (GameManager.instance.hide)
                DestroyGhost();
        }
    }

    public void StartChase()
    {
        StartCoroutine(ChasePlayer());
    }

    public void SpeedUp()
    {
        nav.speed += 0.25f;
    }
}

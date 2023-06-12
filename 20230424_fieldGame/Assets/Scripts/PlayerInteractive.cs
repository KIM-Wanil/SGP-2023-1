using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractive : MonoBehaviour
{
    public bool check_box;
    public bool gameClear;

    public int openBoxCount;

    PlayerControl player;

    GameObject closestJewel = null;
    public GameObject carriedJewel = null;
    GameObject ghost;
    bool canOffer = false; 
    [SerializeField]float interact_distance;
    public Transform closestHideout;
    public bool gameGenerating = false;
    //20230515 김완일추가
    
    void Start()
    {
        ghost = null;
        player = GameObject.Find("Player").GetComponent<PlayerControl>();

        interact_distance = 0.5f;
        check_box = false;
        gameClear = false;

        openBoxCount = 0;
    }

    void Update()
    {
        if(!player.isActionProgress && !GameManager.instance.usedEscape)
        {
            CheckCol();
            pick_or_drop_control();

            if (GameManager.instance.talismanCount > 0 && Input.GetKeyDown(KeyCode.W))
            {
                try
                {
                    ghost = GameObject.FindWithTag("Ghost").gameObject;
                }
                catch
                {
                    ghost = null;
                    return;
                }
                if (ghost == null) return;

                if (Vector3.Distance(player.transform.position, ghost.transform.position) < 3f)
                {
                    Debug.Log("Destroy ghost by talisman");
                    Destroy(ghost.gameObject);
                    ghost.GetComponent<GhostAI>().DestroyGhost();
                    GameManager.instance.talismanCount--;
                }
            }
        }
    }

    void CheckCol()
    {
        var colliders = Physics.OverlapSphere(this.transform.position, interact_distance);
        foreach(Collider col in colliders)
        {
            if (col.CompareTag("Box"))
            {
                if (Input.GetKeyDown(KeyCode.F) && !GameManager.instance.lanternOn)
                    GameManager.instance.explainBoxOpenRule();

                if (Input.GetKeyDown(KeyCode.F) && !GameManager.instance.hide && GameManager.instance.lanternOn && !GameManager.instance.usedEscape)
                {
                    Invoke("gameGenerate", 0.1f);
                    if(gameGenerating)
                        col.transform.GetComponent<MiniGameManager>().RandomGameGenerate();
                }
            }

            else if (col.CompareTag("Hideout"))
            {
                closestHideout = col.transform;
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Invoke("TurnOnHide", 0.1f);
                }
            }
        }
    }

    void gameGenerate()
    {
        gameGenerating = true;
    }

    void TurnOnHide()
    {
        GameManager.instance.hide = true;
    }
    private void OnTriggerStay(Collider other)
    {
        GameObject other_go = other.gameObject;

        if (other.CompareTag("Jewel"))
        {
            if(closestJewel == null)
            {
                if(is_other_in_view(other_go))
                    closestJewel = other_go;
            }
            else if(closestJewel == other_go)
            {
                if(!is_other_in_view(other_go))
                    closestJewel = null;
            }
        }

        if(other.CompareTag("Altar"))
        {
            if(carriedJewel != null)
            {
                canOffer = true;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (closestJewel == other.gameObject)
            closestJewel = null;
    }

    private void pick_or_drop_control()
    {
        do
        {
            if (!Input.GetKeyDown(KeyCode.F))
                break;
            if (carriedJewel == null)
            { // 들고 있는 아이템이 없고.
                if (closestJewel == null)
                {// 주목 중인 아이템이 없으면.
                    break;
                }
                // 주목 중인 아이템을 들어올린다.
                this.carriedJewel = this.closestJewel;
                // 들고 있는 아이템을 자신의 자식으로 설정.
                this.carriedJewel.transform.parent = this.transform;
                // 2.0f 위에 배치(머리 위로 이동).
                this.carriedJewel.transform.localPosition = Vector3.up * 3.0f;
                // 주목 중 아이템을 없앤다.
                this.closestJewel = null;
            }
            else if (carriedJewel != null)
            { // 들고 있는 아이템이 있을 경우.
              // 들고 있는 아이템을 약간(1.0f) 앞으로 이동시켜서.
                if(!canOffer)
                {
                    this.carriedJewel.transform.localPosition = Vector3.forward * 1.0f;
                    this.carriedJewel.transform.parent = null;// 자식 설정을 해제.
                    this.carriedJewel = null; // 들고 있던 아이템을 없앤다.
                }
                else
                {
                    Destroy(carriedJewel);
                    carriedJewel = null;
                    GameManager.instance.GetJewel();
                    GameManager.instance.spawnTime -= 3f;
                    canOffer = false;
                }
            }
        } while (false);
    }

    private bool is_other_in_view(GameObject other)
    {
        bool ret = false;
        do
        {
            Vector3 heading = // 자신이 현재 향하고 있는 방향을 보관.
            this.transform.TransformDirection(Vector3.forward);
            Vector3 to_other = // 자신 쪽에서 본 아이템의 방향을 보관.
            other.transform.position - this.transform.position;
            heading.y = 0.0f;
            to_other.y = 0.0f;
            heading.Normalize(); // 길이를 1로 하고 방향만 벡터로.
            to_other.Normalize(); // 길이를 1로 하고 방향만 벡터로.
            float dp = Vector3.Dot(heading, to_other); // 양쪽 벡터의 내적을 취득.
            if (dp < Mathf.Cos(45.0f))
            { // 내적이 45도인 코사인 값 미만이면.
                break; // 루프를 빠져나간다.
            }
            ret = true; // 내적이 45도인 코사인 값 이상이면 정면에 있다.
        } while (false);
        return (ret);
    }
}

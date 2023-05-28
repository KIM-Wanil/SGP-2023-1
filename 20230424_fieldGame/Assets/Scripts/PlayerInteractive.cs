using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractive : MonoBehaviour
{
    public bool check_box, pickUpJewel;
    public bool gameClear;
    public bool createJewel;

    public int openBoxCount;

    PlayerControl player;
    GhostSpawner ghostSpawner;

    GameObject closestJewel = null;
    GameObject carriedJewel = null;
    GameObject ghost;
    bool canOffer = false; 
    [SerializeField]float interact_distance;
    public Transform closestHideout;
    //20230515 ������߰�
    
    void Start()
    {
        ghost = null;
        player = GameObject.Find("Player").GetComponent<PlayerControl>();
        ghostSpawner = GameObject.Find("GhostSpawner").GetComponent<GhostSpawner>();

        interact_distance = 1f;
        check_box = false;
        gameClear = false;

        openBoxCount = 0;
    }

    void Update()
    {
        if(!player.isActionProgress)
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

    void CheckCol()
    {
        var colliders = Physics.OverlapSphere(this.transform.position, interact_distance);
        foreach(Collider col in colliders)
        {
            if(col.CompareTag("Box"))
            {
                if (Input.GetKeyDown(KeyCode.F) && !GameManager.instance.hide && GameManager.instance.lanternOn && !GameManager.instance.usedEscape)
                    col.transform.GetComponent<MiniGameManager>().RandomGameGenerate();
            }

            if(col.CompareTag("Hideout"))
            {
                closestHideout = col.transform;
                if (Input.GetKeyDown(KeyCode.F))
                {
                    GameManager.instance.hide = true;
                }
            }
        }
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
            { // ��� �ִ� �������� ����.
                if (closestJewel == null)
                {// �ָ� ���� �������� ������.
                    break;
                }
                // �ָ� ���� �������� ���ø���.
                this.carriedJewel = this.closestJewel;
                // ��� �ִ� �������� �ڽ��� �ڽ����� ����.
                this.carriedJewel.transform.parent = this.transform;
                // 2.0f ���� ��ġ(�Ӹ� ���� �̵�).
                this.carriedJewel.transform.localPosition = Vector3.up * 2.0f;
                // �ָ� �� �������� ���ش�.
                this.closestJewel = null;
            }
            else
            { // ��� �ִ� �������� ���� ���.
              // ��� �ִ� �������� �ణ(1.0f) ������ �̵����Ѽ�.
                if(!canOffer)
                {
                    this.carriedJewel.transform.localPosition = Vector3.forward * 1.0f;
                    this.carriedJewel.transform.parent = null;// �ڽ� ������ ����.
                    this.carriedJewel = null; // ��� �ִ� �������� ���ش�.
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
            Vector3 heading = // �ڽ��� ���� ���ϰ� �ִ� ������ ����.
            this.transform.TransformDirection(Vector3.forward);
            Vector3 to_other = // �ڽ� �ʿ��� �� �������� ������ ����.
            other.transform.position - this.transform.position;
            heading.y = 0.0f;
            to_other.y = 0.0f;
            heading.Normalize(); // ���̸� 1�� �ϰ� ���⸸ ���ͷ�.
            to_other.Normalize(); // ���̸� 1�� �ϰ� ���⸸ ���ͷ�.
            float dp = Vector3.Dot(heading, to_other); // ���� ������ ������ ���.
            if (dp < Mathf.Cos(45.0f))
            { // ������ 45���� �ڻ��� �� �̸��̸�.
                break; // ������ ����������.
            }
            ret = true; // ������ 45���� �ڻ��� �� �̻��̸� ���鿡 �ִ�.
        } while (false);
        return (ret);
    }
}

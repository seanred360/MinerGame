using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    bool m_isMoving;
    public bool m_canMove = true;

    // Update is called once per frame
    void Update()
    {
        if ((Input.GetMouseButtonDown(0)) && m_canMove)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                m_isMoving = true;
                agent.SetDestination(hit.point + new Vector3(0, 0, 2));
            }
            else Debug.Log("no possible paths");
            if (hit.collider.tag == "Ore")
            {
                StartCoroutine(StartMining(hit.collider.gameObject));
                m_canMove = false;
            }
        }
            if (!agent.pathPending)
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                    {
                        m_isMoving = false;
                        this.GetComponentInChildren<Animator>().SetBool("isWalking", false);
                    }
                }
            }
            if (m_isMoving == true)
            {
                this.GetComponentInChildren<Animator>().SetBool("isWalking", true);
                this.GetComponentInChildren<Animator>().SetBool("isMining", false);
            }
        
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ore")
        {
            m_isMoving = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ore")
        {
            m_isMoving = true;
        }
    }
    IEnumerator StartMining(GameObject targetOre)
    {
        if(targetOre == null)
        {
            yield break;
        }
        while (m_isMoving == true)
        { 
              yield return null;
        }
        this.GetComponentInChildren<Animator>().SetBool("isMining", true);
        targetOre.GetComponent<Ore>().StartMineOre();
    }

    public void StopMining()
    {
        this.GetComponentInChildren<Animator>().SetBool("isMining", false);
    }
}
